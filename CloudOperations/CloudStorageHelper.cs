using RestSharp;
using StorageApp.Models;

namespace StorageApp.CloudOperations
{
    public class CloudStorageHelper
    {
        public async Task UploadToCloud(string cloudUrl, byte[] content,List<ContentHeader> header=null)
        {
            var client = new RestClient(cloudUrl);
            var request = new RestRequest("", Method.Put);
            header?.ForEach(e =>{request.AddHeader(e.Key, e.Value);});
            request.AddFile(string.Empty, content, string.Empty, "multipart/form-data");
            await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> DownloadFromCloud(string cloudUrl, List<ContentHeader> header=null)
        {
            var client = new RestClient(cloudUrl);
            var request = new RestRequest("", Method.Get);
            header?.ForEach(e =>{request.AddHeader(e.Key, e.Value);});
            return await client.ExecuteAsync(request);
        }
    }
}
