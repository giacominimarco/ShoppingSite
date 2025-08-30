using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            // Forçar HTTP
            builder.WebHost.UseUrls("http://localhost:5119");
            // Logging
            builder.AddDefaultLogging();

            // Controllers e Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Adiciona Health Checks
            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            // Configura��o do EF Core
            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            // JWT Authentication
            builder.Services.AddJwtAuthentication(builder.Configuration);

            // Registro de depend�ncias
            builder.RegisterDependencies();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            // Pipeline de validação
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // CORS para React Dev Server
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendDev", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Middleware de validação
            app.UseMiddleware<ValidationExceptionMiddleware>();

            app.UseCors("AllowFrontendDev");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseBasicHealthChecks();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}