using RestSharp;
using StorageApp.Models;

namespace StorageApp
{
    public interface ICloudStorageService
    {
        Task UploadFileAsync(string filename, byte[] content);
        Task<byte[]> DownloadFileAsync(string filename);
        Task<List<FileInformation>> ListAllFileAsync();

    }
       
}
