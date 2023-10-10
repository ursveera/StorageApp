using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using StorageApp;
using StorageApp.Models;
using StorageApp.CloudOperations;
using Microsoft.Extensions.Options;
using StorageApp.CloudProvider.Config;
using StorageApp.Models.APIResponse;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using System.Xml.Serialization;

public class CloudStorageService : ICloudStorageService
{
    private readonly string _azureStorageUrl;
    private readonly string _awsS3Url;
    private readonly string target;
    private readonly CloudStorageHelper _cloudStorageHelper;
    private readonly CloudOptions _cloudoptions;
    private readonly string _providerName;

    public CloudStorageService(IOptionsMonitor<CloudOptions> options, CloudStorageHelper cloudStorageHelper)
    {
        _cloudoptions = options.CurrentValue;
        _azureStorageUrl = _cloudoptions.Azure.StorageUrl;
        _awsS3Url = _cloudoptions.AWS.uploadUrl;
        target = _cloudoptions.Target;
        _cloudStorageHelper = cloudStorageHelper;
    }

    public async Task UploadFileAsync(string filename, byte[] content)
    {
        switch (target.ToLower())
        {
            case "azure":
                if (!string.IsNullOrEmpty(_azureStorageUrl))
                {
                    string azureBlobUrl = Path.Combine(_cloudoptions.Azure.StorageUrl, _cloudoptions.Azure.BlobContainerName, filename + "?" + _cloudoptions.Azure.SASToken);
                    List<ContentHeader> header = new List<ContentHeader>() { new ContentHeader() { Key = "x-ms-blob-type", Value = "BlockBlob" } };
                    await _cloudStorageHelper.UploadToCloud(azureBlobUrl, content, header);
                }
                break;
            case "aws":
                if (!string.IsNullOrEmpty(_awsS3Url))
                {
                    string awsS3Url = Path.Combine(_awsS3Url, _cloudoptions.AWS.Stage, _cloudoptions.AWS.BucketName, filename);
                    List<ContentHeader> header = new List<ContentHeader>() { new ContentHeader() { Key = "Content-Type", Value = MediaTypeNames.Application.Octet } };
                    await _cloudStorageHelper.UploadToCloud(awsS3Url, content, header);
                }
                break;
        }

    }

    public async Task<RestResponse> DownloadFileAsync(string filename)
    {
        switch (target.ToLower())
        {
            case "azure":
                if (!string.IsNullOrEmpty(_azureStorageUrl))
                {
                    string azureBlobUrl = Path.Combine(_cloudoptions.Azure.StorageUrl, _cloudoptions.Azure.BlobContainerName, filename + "?" + _cloudoptions.Azure.SASToken);
                    return await _cloudStorageHelper.DownloadFromCloud(azureBlobUrl);
                }
                break;
            case "aws":
                if (!string.IsNullOrEmpty(_awsS3Url))
                {
                    string awsS3Url = Path.Combine(_awsS3Url, _cloudoptions.AWS.Stage, _cloudoptions.AWS.BucketName, filename);
                    List<ContentHeader> header = new List<ContentHeader>() { new ContentHeader() { Key = "x-api-key", Value = _cloudoptions.AWS.XAPIKEY } };
                    return await _cloudStorageHelper.DownloadFromCloud(awsS3Url, header);
                }
                break;
        }
        throw new InvalidOperationException("No cloud storage URL configured.");
    }



    public async Task<FileInformation> ListAllFileAsync(string bucketname)
    {
        switch (target.ToLower())
        {
            case "azure":
                if (!string.IsNullOrEmpty(_azureStorageUrl))
                {
                    string azureListFileUrl = Path.Combine(_cloudoptions.Azure.StorageUrl, bucketname + "?" + _cloudoptions.Azure.ListFilesQueryString+_cloudoptions.Azure.SASToken);
                    var response= await _cloudStorageHelper.DownloadFromCloud(azureListFileUrl);
                    if (response.Content != null)
                    {
                        var serializer = new XmlSerializer(typeof(AzureResponse));
                        using (var reader = new StringReader(response.Content))
                        {
                            var azureResp = (AzureResponse)serializer.Deserialize(reader);
                            
                            return null;
                        }
                    }
                    return null;
                }
                break;
            case "aws":
                if (!string.IsNullOrEmpty(_awsS3Url))
                {
                    string awsS3Url = Path.Combine(_awsS3Url, _cloudoptions.AWS.Stage, bucketname);
                    List<ContentHeader> header = new List<ContentHeader>() { new ContentHeader() { Key = "x-api-key", Value = _cloudoptions.AWS.XAPIKEY } };
                    var response=await _cloudStorageHelper.DownloadFromCloud(awsS3Url, header);
                    return null;
                }
                break;
        }
        throw new InvalidOperationException("No cloud storage URL configured.");
    }

}