using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        // Private constructor to enforce use of factory methods
        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        // Success
        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        // Failure
        public static Result<T> Failure(string error) => new Result<T>(false, default, error);
    }
}
