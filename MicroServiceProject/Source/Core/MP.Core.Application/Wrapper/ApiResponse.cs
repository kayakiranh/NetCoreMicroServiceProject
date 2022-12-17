using MP.Infrastructure.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MP.Core.Application.Wrapper
{
    [Serializable]
    public class ApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Created { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        public object Result { get; set; }
        public int ResultCount { get; set; }

        public static ApiResponse ErrorResponse(Exception ex)
        {
            return new ApiResponse
            {
                Status = false,
                Message = JsonConvert.SerializeObject(ex),
                Result = null,
                ResultCount = 0
            };
        }

        public static ApiResponse ErrorResponse(string errorMessage)
        {
            return new ApiResponse
            {
                Status = false,
                Message = errorMessage,
                Result = null,
                ResultCount = 0
            };
        }

        public static ApiResponse SuccessResponse(List<object> data)
        {
            return new ApiResponse
            {
                Status = true,
                Message = data.Count > 0 ? ApiResponseTextHelper.Success : ApiResponseTextHelper.DataNotFound,
                Result = data.Count > 0 ? JsonConvert.SerializeObject(data) : null,
                ResultCount = data.Count
            };
        }

        public static ApiResponse SuccessResponse(object data)
        {
            int dataCount = 0;

            if (data != null)
            {
                PropertyInfo property = typeof(ICollection).GetProperty("Count");
                dataCount = (int)property.GetValue(data, null);
            }

            return new ApiResponse
            {
                Status = data != null,
                Message = data != null ? ApiResponseTextHelper.Success : ApiResponseTextHelper.Error,
                Result = data != null ? JsonConvert.SerializeObject(data) : null,
                ResultCount = dataCount
            };
        }
    }
}