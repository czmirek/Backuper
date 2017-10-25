namespace Backuper.Library
{
    using Backuper.Code.DateTimeResolution;
    using Backuper.Library.Configuration;
    using Backuper.Library.Downloaders;
    using Backuper.Library.Services;
    using Backuper.Library.Uploaders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures dependency injection for the Backuper.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="options">Backuper configuration</param>
        /// <returns>Configured service collection</returns>
        public static IServiceCollection AddBackuper(this IServiceCollection services, IConfiguration options)
        {
            services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>()
                    .AddSingleton<IBackupNamingService, DefaultNamingService>()
                    .AddSingleton(x => new BackuperConfiguration(options))
                    .AddSingleton<DownloaderFactory>()
                    .AddSingleton<UploaderFactory>()
                    .AddTransient<BackuperTask>();

            return services;
        }
    }
}
