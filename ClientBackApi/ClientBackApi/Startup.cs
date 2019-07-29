﻿using ApiApp.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddDbContext<CampContext>();

            services
                .AddScoped<ICampRepository, CampRepository>();

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
                .AddMvc(opt => opt.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(policyName: "default");
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}