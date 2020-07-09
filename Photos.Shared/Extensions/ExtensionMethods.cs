using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Photos.Shared.Extensions
{
    public static class ExtensionMethods
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static void LogInformationWithDate(this ILogger logger, string message, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(message))
            {
                logger.LogInformation($"{DateTime.UtcNow}: {message}", parameters);
            }
        }

        public static void LogErrorWithDate(this ILogger logger, string message = null, Exception ex = null, params object[] parameters)
        {
            if (ex != null || !string.IsNullOrEmpty(message))
            {
                logger.LogError(ex, $"{DateTime.UtcNow}: {message}", parameters);
            }
        }
    }
}
