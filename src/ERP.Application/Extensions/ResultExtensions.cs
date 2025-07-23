using ERP.Application.Common.Models;

namespace ERP.Application.Extensions
{
    public static class ResultExtensions
    {
        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
        {
            if (!result.IsSuccess)
                return Result<TOut>.Failure(result.ErrorMessage ?? "Unknown error");

            try
            {
                var mappedValue = mapper(result.Data!);
                return Result<TOut>.Success(mappedValue);
            }
            catch (Exception ex)
            {
                return Result<TOut>.Failure($"Mapping failed: {ex.Message}");
            }
        }

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mapper)
        {
            if (!result.IsSuccess)
                return Result<TOut>.Failure(result.ErrorMessage ?? "Unknown error");

            try
            {
                var mappedValue = await mapper(result.Data!);
                return Result<TOut>.Success(mappedValue);
            }
            catch (Exception ex)
            {
                return Result<TOut>.Failure($"Async mapping failed: {ex.Message}");
            }
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess && result.Data != null)
            {
                action(result.Data);
            }
            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
        {
            if (!result.IsSuccess)
            {
                action(result.ErrorMessage ?? "Unknown error");
            }
            return result;
        }
    }
}
