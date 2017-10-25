namespace Backuper.Console
{
    using System;
    using System.Threading.Tasks;
    using Backuper.Library;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NLog;
    using NLog.Extensions.Logging;

    /// <summary>
    /// Console entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// NLog logger instance
        /// </summary>
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Console arguments</param>
        /// <returns>Async Task</returns>
        public static async Task Main(string[] args)
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler.UnhandledExceptionTrapper;
#endif
            Logger.Info("Application started");

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Logger.Trace("Loading configuration...");
            var configBuilder = new ConfigurationBuilder().AddJsonFile("backup-config.json");
            IConfigurationRoot backuperConfig = configBuilder.Build();
            Logger.Trace("Configuration loaded");

            Logger.Trace("Initializing services...");
            var services = new ServiceCollection();
            services.AddLogging(x => x.AddNLog())
                    .AddOptions()
                    .AddBackuper(backuperConfig)
                    .AddSingleton(new NLogLoggerFactory().CreateLogger("Backuper"));

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            
            Logger.Trace("Running the backuper task");

            BackuperTask task = serviceProvider.GetService<BackuperTask>();
            await task.RunAsync();

            Logger.Info("Application ended");
        }
    }
}