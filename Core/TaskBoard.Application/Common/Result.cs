using System.ComponentModel;

namespace TaskBoard.Application.Common
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public ErrorCode ErrorCode { get; set; } = ErrorCode.None;

        //public static Result<T> Success(T data) => new()
        //{
        //    Succeeded = true,
        //    Data = data
        //};

        public static Result<T> Success(T data, string message = null)
        {
            return new Result<T> { Succeeded = true, Data = data, Message = message };
        }

        public static Result<T> Failure(string error, ErrorCode errorCode) => new()
        {
            Succeeded = false,
            Message = error,
            ErrorCode = errorCode
        };
    }

    public enum ErrorCode
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None,

        /// <summary>
        /// Resource not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Bad request or invalid input.
        /// </summary>
        BadRequest,

        /// <summary>
        /// Access to the resource is forbidden.
        /// </summary>
        Forbidden,

        /// <summary>
        /// Conflict with the current state of the resource.
        /// </summary>
        Conflict,

        /// <summary>
        /// Unauthorized access.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// Validation errors in the input data.
        /// </summary>
        ValidationError,

        /// <summary>
        /// Internal server error.
        /// </summary>
        InternalError
    }
}
