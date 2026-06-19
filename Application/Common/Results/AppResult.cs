namespace Application.Common.Results
{
    public class AppResult<TValue> : IAppResult<TValue>
    {
        public TValue? Value { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorCode { get; }
        public object? ErrorData { get; }
        public Dictionary<string, List<string>>? Errors { get; }

        protected AppResult(
            TValue? value,
            bool isSuccess,
            string? errorCode,
            object? errorData,
            Dictionary<string, List<string>>? errors)
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            ErrorData = errorData;
            Errors = errors;
        }

        public static AppResult<TValue> Success(TValue value)
            => new(value, true, null, null, null);

        public static AppResult<TValue> Failure(string errorCode, object? errorData = null)
            => new(default, false, errorCode, errorData, null);

        public static AppResult<TValue> ValidationFailure(Dictionary<string, List<string>> errors)
            => new(default, false, "VALIDATION_ERROR", null, errors);
    }
}
