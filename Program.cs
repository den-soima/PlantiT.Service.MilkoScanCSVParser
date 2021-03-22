using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using PlantiT.Service.MilkoScanCSVParser.Helpers;
using PlantiT.Service.MilkoScanCSVParser.Services;

namespace PlantiT.Service.MilkoScanCSVParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(configureLogging => configureLogging.
                    AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "PlantIT MilkoScan Parser Service";
                            config.SourceName = "PlantIT MilkoScan Parser Service Source";
                        });
                    services.AddSingleton<ServiceSettings>();
                    services.AddSingleton<FileReader>();
                    services.AddSingleton<Repository>();
                    services.AddSingleton<FileHandler>();
                    services.AddSingleton<WorkerLogger>();
                }).UseWindowsService();
    }
}