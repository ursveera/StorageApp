using Azure;
using Microsoft.Extensions.Options;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using StorageApp.Models.APIResponse;
using System.Xml.Serialization;

namespace StorageApp.Services
{
    public class AzureFileStorageService : ICloudStorageService
    {
        private string BaseUrl;
        private string listUrl;
        private readonly List<ContentHeader>? UploadHeaders;
        private List<ContentHeader>? DownloadHeaders;
        private readonly CloudOptions? cloudoptions;
        public AzureFileStorageService(CloudOptions options)
        {
            cloudoptions = options;
            BaseUrl = Path.Combine(cloudoptions.Azure.StorageUrl, cloudoptions.Azure.BlobContainerName, "{filename}" + "?" + cloudoptions.Azure.SASToken);
            listUrl = Path.Combine(cloudoptions.Azure.StorageUrl, cloudoptions.Azure.BlobContainerName + "?" +cloudoptions.Azure.ListFilesQueryString+cloudoptions.Azure.SASToken);
            UploadHeaders = new List<ContentHeader>() { new ContentHeader() { Key = "x-ms-blob-type", Value = "BlockBlob" } };

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
            var client = new RestClient(listUrl);
            var request = new RestRequest("", Method.Get);
            DownloadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            var resp = await client.ExecuteAsync(request);

            if (resp.Content != null)
            {
                var serializer = new XmlSerializer(typeof(AzureResponse));
                using (var reader = new StringReader(resp.Content))
                {
                    var azureResp = (AzureResponse)serializer.Deserialize(reader);
                    List<FileInformation> fileInformation = new List<FileInformation>();
                    azureResp.Blobs.ToList().ForEach(e =>
                    {

                        FileInformation f = new FileInformation();
                        f.FileName = e.Name;
                        f.CreatedOn = e.Properties.CreationTime;
                        f.FileType = Path.GetExtension(e.Name);
                        f.Access = e.Properties.AccessTier;
                        f.CreatedBy = "Admin";
                        fileInformation.Add(f);
                    });
                    return fileInformation;
                }
            }

            return null;
        }

        public async Task UploadFileAsync(string filename, byte[] content)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string fileContentsString = Convert.ToBase64String(content);
                string contentType = MimeMapping.GetContentTypeFromExtension(filename);
                ByteArrayContent byteArrayContent = new ByteArrayContent(content);
                byteArrayContent.Headers.Add("x-ms-blob-type", "BlockBlob");
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                await httpClient.PutAsync(BaseUrl.AddFileNameToBaseUrl(filename), byteArrayContent);
            }
            //var client = new RestClient(BaseUrl.AddFileNameToBaseUrl(filename));
            //var request = new RestRequest("", Method.Put);
            //UploadHeaders?.ForEach(e => { request.AddHeader(e.Key, e.Value); });
            //request.AddFile(string.Empty, content, string.Empty, "multipart/form-data");
            //await client.ExecuteAsync(request);
        }
    }
}
