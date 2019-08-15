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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using ClientBackApi.Models;
using Microsoft.Extensions.Options;
using ClientBackApi.Models.Rules;

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
                .Configure<MonikerSettings>(Configuration.GetSection("Moniker"))
                .TryAddSingleton<IMonikerSettings>(options => options.GetRequiredService<IOptions<MonikerSettings>>().Value);

            services
                .AddSingleton<IMonikerRule, AlphaNumericNameRule>()
                .AddSingleton<IMonikerRule, UpperCaseNameRule>();

            services
                .AddTransient<IMonikerRuleProcessor, MonikerRuleProcessor>();

            services
                .AddMetrics(AppMetrics.CreateDefaultBuilder().Build())
                .AddMetricsTrackingMiddleware()
                .AddMetricsReportingHostedService();

            services
                .AddScoped<ICampRepository, CampRepository>()
                .TryAddScoped(typeof(IReferenceRepository<>), typeof(ReferenceRepository<>));

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
                .UseMvc();

            RecurringJob.RemoveIfExists(nameof(CheckDataJob));
            RecurringJob.AddOrUpdate<IJob>((job) => job.ExecuteAsync(), "*/1 * * * *");
        }
    }
}
