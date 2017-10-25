namespace Backuper.Console
{
    using System;
    using NLog;

    /// <summary>
    /// Exception handler for the console.
    /// </summary>
    internal sealed class ExceptionHandler
    {
        /// <summary>
        /// Traps an unhandled exception and logs it.
        /// </summary>
        /// <param name="sender">Sender source.</param>
        /// <param name="e">Exception args.</param>
        public static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ILogger logger = LogManager.GetCurrentClassLogger();
            Exception exception = e.ExceptionObject as Exception;
            logger.Fatal(exception, $"Unhandled exception: {exception.Message}");
        }
    }
}
