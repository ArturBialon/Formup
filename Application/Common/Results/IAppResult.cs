namespace Application.Common.Results
{
    public interface IAppResult<TValue>
    {
        TValue? Value { get; }
        bool IsSuccess { get; }
        bool IsFailure { get; }
        string? ErrorCode { get; }
        object? ErrorData { get; }
        Dictionary<string, List<string>>? Errors { get; }
    }
}
