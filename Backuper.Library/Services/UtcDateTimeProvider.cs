namespace Backuper.Code.DateTimeResolution
{
    using System;

    /// <summary>
    /// Service implementing current UTC datetime.
    /// </summary>
    internal class UtcDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Current UTC datetime
        /// </summary>
        DateTime IDateTimeProvider.Now => DateTime.UtcNow;
    }
}
