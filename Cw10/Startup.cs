using System.Linq;
using Cw10.Middlewares;
using Cw10.Models.StudentsDatabase;
using Cw10.Services.AuthenticationServices;
using Cw10.Services.DatabaseServices;
using Cw10.Services.EncryptionServices;
using Cw10.Services.LoggingServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cw10
{
    public class Startup
    {
        private const string IndexHeader = "Index";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            TokenValidationParametersGenerator.AddConfiguration(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters =
                    TokenValidationParametersGenerator.GenerateTokenValidationParameters());

            services.AddTransient<ILoggingService, FileLoggingService>();
            services.AddSingleton<IDbStudentService, EntityFrameworkCoreDbStudentService>();
            services.AddSingleton<StudentsDbContext, StudentsDbContext>();
            services.AddSingleton<IAuthenticationService, MssqlDbAuthenticationService>();
            services.AddSingleton<IEncryptionService, SaltedHashEncryptionService>();
            services.AddControllers();

            // Register the Swagger generator, defining Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Students API",
                    Version = "v1",
                    Description = "Students API prepared by Paweł Rutkowski (s18277) for purposes of APBD.",
                    Contact = new OpenApiContact {Name = "Paweł Rutkowski s18277"}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbStudentService dbService)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Students API"); });

            // Logging middleware is enabled after Swagger one to avoid logging requests for documentation.
            app.UseMiddleware<LoggingMiddleware>();

            app.Use(async (context, next) =>
            {
                if (dbService.GetAllStudents().ToList().Count == 0)
                {
                    await next();
                    return;
                }

                if (!context.Request.Headers.ContainsKey(IndexHeader))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Brak nagłówka z numerem indeksu!");
                    return;
                }

                var index = context.Request.Headers[IndexHeader].ToString();
                if (dbService.GetStudent(index) == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Przekazany numer indeksu nie istnieje w bazie danych!");
                    return;
                }

                await next();
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}