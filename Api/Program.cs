using Api;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            builder.Logging.AddDbLogger(options =>
            {
                builder.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options").Bind(options);
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {  // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // pick comments from classes, including controller summary comments
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                // _OR_ enable the annotations on Controller classes [SwaggerTag], if no class comments present
                c.EnableAnnotations();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Apply migration
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                    var logger = app.Services.GetService<ILogger<ApplicationDbContext>>();
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
