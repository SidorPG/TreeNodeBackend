using Api.Exceptions;
using Data;
using Data.Models;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly ApplicationDbContext dbContext;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerProvider loggerProvider)
    {
        _next = next;
        _logger = loggerProvider.CreateLogger("ExceptionHandlingMiddleware");
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        try
        {
            try { context.Request.EnableBuffering(); } catch { }
            await _next(context);
        }
        catch (Exception exception)
        {
            var journal_event = new journal_event()
            {
                Created = DateTime.UtcNow,
            };
            var r = await dbContext.JournalEvents.AddAsync(journal_event);
            await dbContext.SaveChangesAsync();
            var journal_message = new journal_message()
            {
                EventId = r.Entity.Id,
                Type = exception is SecureException ? "Secure" : exception.GetType().FullName
            };
            var data = new List<string>();
            data.Add($"Path = {context.Request.Path}");
            foreach (var item in context.Request.Query.Keys)
            {
                data.Add($"{item} = {context.Request.Query[item]}");
            }

            try
            {
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using StreamReader stream = new(context.Request.Body);
                string body = stream.ReadToEnd();
                data.Add($"{body}");
            }
            catch { }
            data.Add($"{exception.GetType().FullName}: {exception.Message}");
            data.Add(exception.StackTrace);

            journal_message.Data = string.Join("\r\n", data);
            await dbContext.AddAsync(journal_message);
            await dbContext.SaveChangesAsync();

            var problemDetails = new
            {
                type = exception is SecureException ? "Secure" : exception.GetType().FullName,
                id = journal_event.Id,
                data = new { message = exception is SecureException se ? $"{se.Message}" : $"Internal server error ID = {r.Entity.Id}." },
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
