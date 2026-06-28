namespace Application.Common.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, CancellationToken ct);
        Task DeleteFileAsync(string fileUrl, CancellationToken ct);
    }
}
