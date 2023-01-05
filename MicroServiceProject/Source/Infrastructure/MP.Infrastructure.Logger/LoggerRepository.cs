using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using Serilog;
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
        private readonly IConfiguration _configuration;

        public LoggerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Insert(LogTypes logType, string message, Exception exception = null, params object[] methodParameters)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(_configuration.GetSection("Serilog:WriteTo:0:Args:path").Value)
                .CreateLogger();

            switch (logType)
            {
                case LogTypes.Information:
                    Log.Information(message);
                    break;

                case LogTypes.Warning:
                    Log.Warning(message, methodParameters);
                    break;

                case LogTypes.Error:
                    Log.Error(exception,message, methodParameters);
                    break;

                default:
                    break;
            }
        }
    }
}