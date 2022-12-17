using MP.Core.Domain.Enums;
using System;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Logger repository
    /// </summary>
    public interface ILoggerRepository
    {
        void Insert(LogTypes logType, string message, Exception exception = null);
    }
}