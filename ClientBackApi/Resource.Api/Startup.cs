using App.Metrics;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resource.Api.Data;
using Resource.Api.Jobs;
using Resource.Api.MiddlewareExtensions;
using Resource.Api.Models;
using Resource.Api.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Resource.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                
            }).AddJwtBearer(o =>
            {
                o.Authority = Configuration["IdentityServerAddress"];
                o.Audience = "resourceapi";
                o.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope", "api.scope"));
                options.AddPolicy("ApiAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "ng_client_1.admin"));
            });

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddTransient<IEmailSender, EmailSender>()
                .Configure<EmailSettings>(options => Configuration.GetSection("SendGrid").Bind(options));

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider srv)
        {
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(srv));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
                .UseAuthentication()
                .UseMetricsAllMiddleware()
                .UseHangfireDashboard()
                .UseHangfireServer()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "ClientBack API Swagger");
                })
                .UseMvc();

            RecurringJob.RemoveIfExists(nameof(CheckDataJob));
            RecurringJob.AddOrUpdate<IJob>((job) => job.ExecuteAsync(), "*/1 * * * *");
        }
    }
}
