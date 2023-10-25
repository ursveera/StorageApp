using RestSharp;
using StorageApp.Models;

namespace StorageApp
{
    public interface ICloudStorageService
    {
        Task UploadFileAsync(string filename, byte[] content);
        Task DeleteFileAsync(string filename);
        Task CreateFolderAsync(string folderpath);
        Task<bool> CheckExists(string filename);
        Task<byte[]> DownloadFileAsync(string filename);
        Task<List<FileInformation>> ListAllFileAsync(string prefix);
        Task<FilesList> ListAllFileAndFoldersAsync(string prefix);

    }
       
}
