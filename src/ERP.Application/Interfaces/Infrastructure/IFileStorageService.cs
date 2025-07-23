namespace ERP.Application.Interfaces.Infrastructure
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<byte[]> DownloadFileAsync(string filePath);
        Task<bool> DeleteFileAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);
        Task<string> GenerateFileUrlAsync(string filePath, TimeSpan? expiration = null);
        Task<IEnumerable<string>> ListFilesAsync(string directoryPath);
        Task<bool> CreateDirectoryAsync(string directoryPath);
        Task<bool> DeleteDirectoryAsync(string directoryPath);
        Task<long> GetFileSizeAsync(string filePath);
        Task<DateTime> GetFileLastModifiedAsync(string filePath);
    }
}