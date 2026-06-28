namespace Application.Common.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        public Task DeleteFileAsync(string fileUrl, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadFileAsync(Stream fileStream, string fileName, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
