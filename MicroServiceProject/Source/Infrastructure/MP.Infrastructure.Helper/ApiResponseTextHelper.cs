using System;

namespace MP.Infrastructure.Helper
{
    /// <summary>
    /// Api response text helper
    /// </summary>
    [Serializable]
    public static class ApiResponseTextHelper
    {
        public static readonly string Success = "Success";
        public static readonly string Error = "Error";
        public static readonly string DataNotFound = "DataNotFound";
    }
}