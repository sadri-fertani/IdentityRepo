using ApiApp.Data;
using App.Metrics;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiApp.Services;
using Hangfire;
using System;
using ClientBackApi.MiddlewareExtensions;
using ClientBackApi.Jobs;
using ApiApp.Models;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;
using System.Collections.Generic;

namespace ApiApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAppConfiguration(Configuration);

            services
                .AddScoped<IJob, CheckDataJob>();

            services
                .AddMemoryCache();

            services
                .AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = Configuration.GetConnectionString("CacheDatabaseConnection");
                    options.SchemaName = "dbo";
                    options.TableName = "CacheTable";
                });

            services
                .AddHangfire(options =>
                {
                    options.UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"));
                });

            services
                .AddDbContext<CampContext>();

            services
                .AddMetrics(AppMetrics.CreateDefaultBuilder().Build())
                .AddMetricsTrackingMiddleware()
                .AddMetricsReportingHostedService();

            services
                .AddAutoMapper(typeof(Startup));

            services
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                });

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddMvcCore(options =>
                {
                    //options.Filters.Add(new MetricsResourceFilter(new MyCustomMetricsRouteNameResolver()));
                })
                .AddMetricsCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services
                .AddIdentity<IdentityUser, IdentityRole>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServerAddress"];
                    options.ApiName = Configuration["NameAPI"];
                    options.RequireHttpsMetadata = false;
                });

            services
                .AddTransient<IEmailSender, EmailSender>()
                .Configure<EmailSettings>(options => Configuration.GetSection("SendGrid").Bind(options));

            services
                .AddCors(options =>
                {
                    options.AddPolicy("default", policy =>
                    {
                        policy
                        .WithOrigins(Configuration["ClientAddress"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });

            services
                .AddSwaggerGen(options =>
                {
                    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    options.SwaggerDoc("v1", new Info
                    {
                        Title = "ClientBack API Swagger",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Name = "Sadri FERTANI",
                            Email = "sadri.fertani@live.fr",
                            Url = "https://github.com/sadri-fertani/"
                        }
                    });

                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", new string[] { }},
                    };

                    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    options.AddSecurityRequirement(security);
                });
        }

        public void Configure(IApplicationBuilder app, IServiceProvider srv)
        {
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(srv));

            app
                .UseCors(policyName: "default")
                .UseHttpsRedirection()
                .UseAuthentication()
                .UseMetricsAllMiddleware()
                .UseHangfireDashboard()
                .UseHangfireServer()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientBack API Swagger");
                })
                .UseMvc();

            RecurringJob.RemoveIfExists(nameof(CheckDataJob));
            RecurringJob.AddOrUpdate<IJob>((job) => job.ExecuteAsync(), "*/1 * * * *");
        }
    }
}
