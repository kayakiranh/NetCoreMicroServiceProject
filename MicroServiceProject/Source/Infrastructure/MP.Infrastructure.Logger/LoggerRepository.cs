using Microsoft.Extensions.Logging;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using Newtonsoft.Json;
using System;

namespace MP.Infrastructure.Logger
{
    public class LoggerRepository : ILoggerRepository
    {
        private readonly ILogger Logger;

        public LoggerRepository(ILogger logger)
        {
            Logger = logger;
        }

        public void Insert(LogTypes logType, string message, Exception exception = null)
        {
            switch (logType)
            {
                case LogTypes.Information:
                    Logger.LogInformation(message);
                    break;
                case LogTypes.Warning:
                    Logger.LogWarning(message);
                    break;
                case LogTypes.Error:
                    Logger.LogError(message, JsonConvert.SerializeObject(exception));
                    break;
                case LogTypes.Critical:
                    Logger.LogCritical(message, JsonConvert.SerializeObject(exception));
                    break;
                default:
                    Logger.LogTrace(message);
                    break;
            }
        }
    }
}