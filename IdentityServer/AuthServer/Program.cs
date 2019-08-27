using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace AuthServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel()
            .UseIISIntegration()
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
