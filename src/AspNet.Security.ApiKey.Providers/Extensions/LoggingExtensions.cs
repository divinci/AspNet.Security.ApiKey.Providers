using System;
using Microsoft.Extensions.Logging;

namespace AspNet.Security.ApiKey.Providers.Extensions
{
    internal static class LoggingExtensions
    {
        private static readonly Action<ILogger, Exception> apiKeyValidationFailed;
        private static readonly Action<ILogger, Exception> apiKeyValidationSucceeded;
        private static readonly Action<ILogger, Exception> errorProcessingMessage;

        static LoggingExtensions()
        {
            apiKeyValidationFailed = LoggerMessage.Define(
                eventId: 1,
                logLevel: LogLevel.Information,
                formatString: "Failed to validate the API key.");
            apiKeyValidationSucceeded = LoggerMessage.Define(
                eventId: 2,
                logLevel: LogLevel.Information,
                formatString: "Successfully validated the API key.");
            errorProcessingMessage = LoggerMessage.Define(
                eventId: 3,
                logLevel: LogLevel.Error,
                formatString: "Exception occurred while processing message.");
        }

        public static void ApiKeyValidationFailed(this ILogger logger)
            => apiKeyValidationFailed(logger, null);

        public static void ApiKeyValidationSucceeded(this ILogger logger)
            => apiKeyValidationSucceeded(logger, null);

        public static void ErrorProcessingMessage(this ILogger logger, Exception ex)
            => errorProcessingMessage(logger, ex);
    }
}
