using ApiApp;
using ApiApp.Data;
using ClientBackApi.Models;
using ClientBackApi.Models.Rules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services
                .Configure<MonikerSettings>(Configuration.GetSection("Moniker"))
                .TryAddSingleton<IMonikerSettings>(options => options.GetRequiredService<IOptions<MonikerSettings>>().Value);

            services
                .Scan(scan =>
                {
                    scan
                        .FromAssemblyOf<IMonikerRule>()
                        .AddClasses(c => c.AssignableTo<IMonikerRule>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime();
                });

            services
                .AddTransient<IMonikerRuleProcessor, MonikerRuleProcessor>();

            services
                .AddScoped<ICampRepository, CampRepository>()
                .TryAddScoped(typeof(IReferenceRepository<>), typeof(ReferenceRepository<>));

            return services;
        }
    }
}
