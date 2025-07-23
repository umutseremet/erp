using FluentValidation.Results;

namespace ERP.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Validation hatası oluştu")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string propertyName, string errorMessage) : base($"Validation hatası: {errorMessage}")
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }

        public IDictionary<string, string[]> Errors { get; }

        public bool HasErrors => Errors.Any();

        public string GetFirstError()
        {
            if (!HasErrors)
                return string.Empty;

            var firstError = Errors.First();
            return firstError.Value.FirstOrDefault() ?? string.Empty;
        }

        public IEnumerable<string> GetAllErrors()
        {
            return Errors.Values.SelectMany(errors => errors);
        }
    }
}