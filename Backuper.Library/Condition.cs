namespace Backuper
{
    using System;

    /// <summary>
    /// Tool for validating input parameters.
    /// </summary>
    internal class Validate
    {
        /// <summary>
        /// Checks whether given string is not null or empty
        /// </summary>
        /// <param name="value">Value of the parameter</param>
        /// <param name="paramName">Name of the parameter</param>
        public static void NotNull(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Checks whether given string is not null
        /// </summary>
        /// <param name="value">Value of the parameter</param>
        /// <param name="paramName">Name of the parameter</param>
        public static void NotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
