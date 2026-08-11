using LowTideEnt.Application;
using LowTideEnt.Application.Authorization;
using LowTideEnt.Application.Authorization.Dto;
using LowTideEnt.Application.Services;
using LowTideEnt.Infrastructure;
using LowTideEnt.Infrastructure.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace LowTideEnt.API
{
    public class Startup(IConfiguration config)
    {
        private string AllowOrigins { get; set; } = string.Empty;
        public void ConfigureServices(IServiceCollection services)
        {
            AllowOrigins = "_allowSpecificOrigins";

            services.AddInfrastructure(config);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);     // datetime for psql
            services.AddExceptionHandler<ExceptionHandlerMiddleware>();
            services.AddApplication(config);
            services.AddProblemDetails();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")   // TODO: change to site url environment variable
                                      .AllowCredentials()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddScoped<ISessionContext, SessionContext>();
            services.AddHttpClient<AuthService>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "LowTideEnt";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.SlidingExpiration = true;

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });
            services.AddAuthorization();
            services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Low Tide Enterprise API",
                        Version = "v1"
                    });
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSession();
            app.UseExceptionHandler();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(AllowOrigins);
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
