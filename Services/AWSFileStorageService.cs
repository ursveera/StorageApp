using Microsoft.Extensions.Options;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using StorageApp.Models.APIResponse;
using System.Net.Mime;
using System.Xml.Serialization;

namespace StorageApp.Services
{
    public class AWSFileStorageService : ICloudStorageService
    {
        private readonly string BaseUrl;
        private readonly List<ContentHeader>? UploadHeaders;
        private List<ContentHeader>? DownloadHeaders;
        private readonly CloudOptions? cloudoptions;
        public AWSFileStorageService(CloudOptions options)
        {
            cloudoptions = options;
            BaseUrl = Path.Combine(cloudoptions.AWS.uploadUrl, cloudoptions.AWS.Stage, cloudoptions.AWS.BucketName, "{filename}");

            DownloadHeaders = new List<ContentHeader>() { new ContentHeader() { Key = "x-api-key", Value = cloudoptions.AWS.XAPIKEY } };
        }

        public async Task<RestResponse> DownloadFileAsync(string filename)
        {
            var client = new RestClient(BaseUrl.AddFileNameToBaseUrl(filename));
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp = await client.ExecuteAsync(request);
            return resp;
        }

        public async Task<List<FileInformation>> ListAllFileAsync()
        {
            var client = new RestClient(BaseUrl.AddBucketName(""));
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp = await client.ExecuteAsync(request);
            if (resp.Content != null)
            {
                var serializer = new XmlSerializer(typeof(ListBucketResult));
                using (var reader = new StringReader(resp.Content))
                {
                    var awsResp = (ListBucketResult)serializer.Deserialize(reader);
                    List<FileInformation> fileInformation = new List<FileInformation>();
                    awsResp.Contents.ToList().ForEach(e =>
                    {
                        FileInformation f = new FileInformation();
                        f.FileName = e.Key;
                        f.CreatedOn = e.LastModified.ToString();
                        f.FileType = Path.GetExtension(e.Key);
                        f.Access = e.Owner.ID;
                        f.CreatedBy = "Admin";
                        fileInformation.Add(f);
                    });
                    return fileInformation;
                }
            }
            return null;
        }

        public async Task UploadFileAsync(string filename, byte[] bytecontent)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string fileContentsString = Convert.ToBase64String(bytecontent);
                string contentType = MimeMapping.GetContentTypeFromExtension(filename);
                ByteArrayContent byteArrayContent = new ByteArrayContent(bytecontent);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                await httpClient.PutAsync(BaseUrl.AddFileNameToBaseUrl(filename), byteArrayContent);
            }
        }
    }
}

//using (HttpClient httpClient = new HttpClient())
//{
//    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl.AddFileNameToBaseUrl(filename));
//    DownloadHeaders?.ForEach(e => { request.Headers.Add(e.Key, e.Value); });
//    HttpResponseMessage response = await httpClient.SendAsync(request);
//    return response;
//}