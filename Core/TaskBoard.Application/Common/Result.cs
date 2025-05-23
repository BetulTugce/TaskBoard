namespace TaskBoard.Application.Common
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }

        //public static Result<T> Success(T data) => new()
        //{
        //    Succeeded = true,
        //    Data = data
        //};

        public static Result<T> Success(T data, string message = null)
        {
            return new Result<T> { Succeeded = true, Data = data, Message = message };
        }

        public static Result<T> Failure(string error) => new()
        {
            Succeeded = false,
            Message = error
        };
    }
}
