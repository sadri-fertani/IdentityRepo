using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Json;
using App.Metrics.Health;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static MetricsReportingInfluxDbOptions InfluxDbOptions
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                    .AddUserSecrets<Program>(true).Build();

                var influxOptions = new MetricsReportingInfluxDbOptions();
                configuration.GetSection(nameof(MetricsReportingInfluxDbOptions)).Bind(influxOptions);
                influxOptions.InfluxDb.Password = configuration["MetricsReportingInfluxDbOptions:InfluxDb:Password"];

                return influxOptions;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureMetricsWithDefaults(
                    builder =>
                    {
                        var filter = new MetricsFilter()
                        .WhereType(
                            MetricType.Apdex,
                            MetricType.Counter,
                            MetricType.Gauge,
                            MetricType.Histogram,
                            MetricType.Meter,
                            MetricType.Timer
                        );

                        builder.Report.ToConsole(
                        options =>
                        {
                            options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                            options.FlushInterval = TimeSpan.FromSeconds(2);
                        });
                        builder.Report.ToTextFile(
                        options =>
                        {
                            options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                            options.AppendMetricsToTextFile = true;
                            options.Filter = filter;
                            options.FlushInterval = TimeSpan.FromSeconds(20);
                            options.OutputPathAndFileName = @".\Reports\Metrics.txt";
                        });
                        builder.Report.ToInfluxDb(InfluxDbOptions);
                    })
                .ConfigureHealthWithDefaults(
                    builder =>
                    {
                        builder
                            .HealthChecks.AddCheck("DatabaseConnected", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")))
                            .HealthChecks.AddProcessPrivateMemorySizeCheck("Private Memory Size", 200)
                            .HealthChecks.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 200)
                            .HealthChecks.AddProcessPhysicalMemoryCheck("Working Set", 200)
                            .HealthChecks.AddPingCheck("Network", "google.com", TimeSpan.FromSeconds(10));
                    })
                .UseHealth()
                .UseMetricsWebTracking()
                .UseMetrics()
                .UseStartup<Startup>()
                .UseSerilog(
                    (hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration);
                    }
                );
        }
    }
}
