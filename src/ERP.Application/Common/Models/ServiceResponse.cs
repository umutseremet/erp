namespace ERP.Application.Common.Models
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResponse<T> SuccessResult(T data, string? message = null)
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResponse<T> ErrorResult(string errorMessage)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage }
            };
        }

        public static ServiceResponse<T> ErrorResult(List<string> errors)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Errors = errors,
                Message = errors.FirstOrDefault()
            };
        }
    }
}