using Azure;
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

        public async Task<byte[]> DownloadFileAsync(string filename)
        {
            var client = new RestClient(BaseUrl.AddFileNameToBaseUrl(filename));
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp = await client.ExecuteAsync(request);
            return resp.RawBytes;
        }

        public async Task<List<FileInformation>> ListAllFileAsync()
        {
            var client = new RestClient(BaseUrl.AddBucketName(""));
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp = await client.ExecuteAsync(request);
            if (!string.IsNullOrEmpty(resp.Content))
            {
                var serializer = new XmlSerializer(typeof(ListBucketResult));
                using (var reader = new StringReader(resp.Content))
                {
                    var awsResp = (ListBucketResult)serializer.Deserialize(reader);
                    var fileInformation = awsResp.Contents.Select(e => new FileInformation
                    {
                        FileName = e.Key,
                        CreatedOn = e.LastModified.ToString(),
                        FileType = Path.GetExtension(e.Key),
                        Access = e.Owner.ID,
                        CreatedBy = "Admin",
                        Size=MimeMapping.FormatFileSize(e.Size)
                    }).ToList();
                    return fileInformation;
                }
            }
            return new List<FileInformation>();
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
