using Microsoft.Extensions.Logging;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using System;

namespace MP.Infrastructure.Logger
{
    /// <summary>
    /// Logger repository
    /// Using Microsoft.Extensions.Logging
    /// </summary>
    [Serializable]
    public class LoggerRepository : ILoggerRepository
    {
        private readonly ILogger Logger;

        public LoggerRepository(ILogger logger)
        {
            Logger = logger;
        }

        public void Insert(LogTypes logType, string message, Exception exception = null, params object[] methodParameters)
        {
            switch (logType)
            {
                case LogTypes.Information:
                    Logger.LogInformation(message, methodParameters);
                    break;
                case LogTypes.Warning:
                    Logger.LogWarning(message, methodParameters);
                    break;
                case LogTypes.Error:
                    Logger.LogError(exception, message, methodParameters);
                    break;
                case LogTypes.Critical:
                    Logger.LogCritical(exception, message, methodParameters);
                    break;
                default:
                    break;
            }
        }
    }
}