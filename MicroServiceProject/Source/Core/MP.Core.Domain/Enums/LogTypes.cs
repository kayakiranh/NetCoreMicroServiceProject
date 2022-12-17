using System;

namespace MP.Core.Domain.Enums
{
    /// <summary>
    /// Log types enum for Microsoft Logging Extension
    /// </summary>
    [Serializable]
    public enum LogTypes
    {
        Information,
        Warning,
        Error,
        Critical
    }
}