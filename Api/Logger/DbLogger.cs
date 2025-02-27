using Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Diagnostics.CodeAnalysis;

namespace Api.Logger;

public class DbLogger : ILogger
{
    /// <summary>  
    /// Instance of <see cref="DbLoggerProvider" />.  
    /// </summary>  
    private readonly DbLoggerProvider _dbLoggerProvider;
    /// <summary>  
    /// Creates a new instance of <see cref="FileLogger" />.  
    /// </summary>  
    /// <param name="fileLoggerProvider">Instance of <see cref="FileLoggerProvider" />.</param>  
    public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
    {
        _dbLoggerProvider = dbLoggerProvider;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    /// <summary>  
    /// Whether to log the entry.  
    /// </summary>  
    /// <param name="logLevel"></param>  
    /// <returns></returns>  
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }


    /// <summary>  
    /// Used to log the entry.  
    /// </summary>  
    /// <typeparam name="TState"></typeparam>  
    /// <param name="logLevel">An instance of <see cref="LogLevel"/>.</param>  
    /// <param name="eventId">The event's ID. An instance of <see cref="EventId"/>.</param>  
    /// <param name="state">The event's state.</param>  
    /// <param name="exception">The event's exception. An instance of <see cref="Exception" /></param>  
    /// <param name="formatter">A delegate that formats </param>  
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            // Don't log the entry if it's not enabled.  
            return;
        }

        var threadId = Thread.CurrentThread.ManagedThreadId; // Get the current thread ID to use in the log file.   

        // Store record.  
        using (var connection = new NpgsqlConnection(_dbLoggerProvider.Options.ConnectionString))
        {
            connection.Open();

            // Add to database.  

            // LogLevel  
            // ThreadId  
            // EventId  
            // Exception Message (use formatter)  
            // Exception Stack Trace  
            // Exception Source  

            var values = new JObject();

            if (_dbLoggerProvider?.Options?.LogFields?.Any() ?? false)
            {
                foreach (var logField in _dbLoggerProvider.Options.LogFields)
                {
                    switch (logField)
                    {
                        case "LogLevel":
                            if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                            {
                                values["LogLevel"] = logLevel.ToString();
                            }
                            break;
                        case "ThreadId":
                            values["ThreadId"] = threadId;
                            break;
                        case "EventId":
                            values["EventId"] = eventId.Id;
                            break;
                        case "EventName":
                            if (!string.IsNullOrWhiteSpace(eventId.Name))
                            {
                                values["EventName"] = eventId.Name;
                            }
                            break;
                        case "Message":
                            if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                            {
                                values["Message"] = formatter(state, exception);
                            }
                            break;
                        case "ExceptionMessage":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                            {
                                values["ExceptionMessage"] = exception?.Message;
                            }
                            break;
                        case "ExceptionStackTrace":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                            {
                                values["ExceptionStackTrace"] = exception?.StackTrace;
                            }
                            break;
                        case "ExceptionSource":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                            {
                                values["ExceptionSource"] = exception?.Source;
                            }
                            break;
                    }
                }
            }
            if (exception != null && exception is SecureException se)
            {
                if (se.QueryParameters != null)
                    values["QueryParameters"] = se.QueryParameters;
                if (se.BodyParameters != null)
                    values["BodyParameters"] = se.BodyParameters;
            }

            using (var command = new NpgsqlCommand())
            {

                command.Connection = connection;

                command.CommandType = System.Data.CommandType.Text;
                //command.CommandText = string.Format("SELECT EXISTS (  SELECT 1   FROM pg_tables  WHERE schemaname = 'public'  AND tablename = '{0}');", _dbLoggerProvider.Options.LogTable);

                //var x = command.ExecuteScalar();
                //if (x != null && (bool)x && exception!=null)
                if (exception != null)
                {

                    command.CommandText = string.Format("INSERT INTO {0} (data,event_id, created, type) VALUES (:values,:event_id, :created, :type)", _dbLoggerProvider.Options.LogTable);

                    command.Parameters.Add(new NpgsqlParameter("values",

                        JsonConvert.SerializeObject(values, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            DefaultValueHandling = DefaultValueHandling.Ignore,
                            Formatting = Formatting.None
                        })
                    ));
                    command.Parameters.Add(new NpgsqlParameter("event_id", eventId.Id));
                    command.Parameters.Add(new NpgsqlParameter("created", DateTimeOffset.UtcNow));
                    command.Parameters.Add(new NpgsqlParameter("type", exception is SecureException ? "Secure" : eventId.Name ?? exception.ToString()));

                    command.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
    }
}
