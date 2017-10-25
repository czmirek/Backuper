namespace Backuper.Code.DateTimeResolution
{
    using System;

    /// <summary>
    /// Service for providing current date time
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets a current date time.
        /// </summary>
        DateTime Now { get; }
    }
}
