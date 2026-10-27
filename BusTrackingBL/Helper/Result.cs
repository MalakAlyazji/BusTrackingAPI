using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Helper
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int? ErrorCode { get; set; }

        public static Result<T> Success(T data) => new()
        {
            IsSuccess = true,
            Data = data
        };

        public static Result<T> Failure(string errorMessage, int? errorCode = null) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode
        };
    }
}