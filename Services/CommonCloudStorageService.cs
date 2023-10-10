using RestSharp;
using StorageApp.Models;
using static StorageApp.Models.APIResponse.AWSResponse;
using System.Reflection.PortableExecutable;
using StorageApp.Extensions;
using Azure;
using StorageApp.Models.APIResponse;
using System.Xml.Serialization;

namespace StorageApp.Services
{
    public class CommonCloudStorageService: ICloudStorageService
    {
        public string BaseUrl { get; set; }
        public List<ContentHeader> UploadHeaders { get; set; }
        public List<ContentHeader> DownloadHeaders { get; set; }
        public CommonCloudStorageService()
        {

        }

        public async Task<RestResponse> DownloadFileAsync(string filename)
        {
            var client = new RestClient(BaseUrl.AddFileNameToBaseUrl(filename));
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp= await client.ExecuteAsync(request);
            return resp;
        }

        public async Task<FileInformation> ListAllFileAsync(string bucketname)
        {
            var client = new RestClient(BaseUrl.AddBucketName(bucketname));
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp= await client.ExecuteAsync(request);
            
            return null;
        }

        public async Task UploadFileAsync(string filename, byte[] content)
        {
            var client = new RestClient(BaseUrl.AddFileNameToBaseUrl(filename));
            var request = new RestRequest("", Method.Put);
            UploadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            request.AddFile(string.Empty, content, string.Empty, "multipart/form-data");
            await client.ExecuteAsync(request);
        }
    }
}
