namespace ERP.Application.Common.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }
        public List<string> Errors { get; private set; } = new List<string>();

        protected Result(bool isSuccess, T? data, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        protected Result(bool isSuccess, T? data, List<string> errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Errors = errors;
            ErrorMessage = errors.FirstOrDefault();
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, (string?)null);
        }

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(false, default(T), errorMessage);
        }

        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T>(false, default(T), errors);
        }

        public static implicit operator bool(Result<T> result)
        {
            return result.IsSuccess;
        }
    }

    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public List<string> Errors { get; private set; } = new List<string>();

        protected Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        protected Result(bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            ErrorMessage = errors.FirstOrDefault();
        }

        public static Result Success()
        {
            return new Result(true, (string?)null);
        }

        public static Result Failure(string errorMessage)
        {
            return new Result(false, errorMessage);
        }

        public static Result Failure(List<string> errors)
        {
            return new Result(false, errors);
        }

        public static implicit operator bool(Result result)
        {
            return result.IsSuccess;
        }
    }
}