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
using StorageApp.CloudProvider.Providers;
using StorageApp.CloudProvider.Config;

//public class CloudStorageServiceTest
//{
//    private readonly string _azureStorageUrl;
//    private readonly string _awsS3Url;
//    private readonly string target;
//    private readonly CloudStorageHelper _cloudStorageHelper;
//    private readonly CloudOptions _cloudoptions;
//    private readonly ICloudProvider _cloudProvider;

//    public CloudStorageServiceTest(IOptionsMonitor<CloudOptions> options, CloudStorageHelper cloudStorageHelper)
//    {
//        _cloudoptions = options.CurrentValue;
//        _azureStorageUrl = _cloudoptions.Azure.StorageUrl;
//        _awsS3Url = _cloudoptions.AWS.uploadUrl;
//        target = _cloudoptions.Target;
//        _cloudStorageHelper = cloudStorageHelper;
//    }

//    public async Task UploadFileAsync(string bucket_or_containername,string filename_or_blobname, byte[] content)
//    {
//        switch (target.ToLower())
//        {
//            case "azure":
//                if (!string.IsNullOrEmpty(_azureStorageUrl))
//                {
//                    string azureBlobUrl = _azureStorageUrl.Replace("{containername}", bucket_or_containername).Replace("{filename}", filename_or_blobname);
//                    await _cloudStorageHelper.UploadToCloud(azureBlobUrl, content, "x-ms-blob-type", "BlockBlob");
//                }
//                break;
//            case "aws":
//                if (!string.IsNullOrEmpty(_awsS3Url))
//                {
//                    string contentvalue = MediaTypeNames.Application.Octet;
//                    string awsS3Url = _awsS3Url.Replace("{stage}", _cloudoptions.AWS.Stage).Replace("{bucketname}", bucket_or_containername).Replace("{filename}", filename_or_blobname);
//                    await _cloudStorageHelper.UploadToCloud(awsS3Url, content, "Content-Type", contentvalue);
//                }
//                break;
//        }
       
//    }

//    public async Task<RestResponse> DownloadFileAsync(Storage storage)
//    {
//        switch (target.ToLower())
//        {
//            case "azure":
//                if (!string.IsNullOrEmpty(_azureStorageUrl))
//                {
//                    string azureBlobUrl = _azureStorageUrl.Replace("{containername}", storage.bucket_or_containername).Replace("{filename}", storage.filename_or_blobname);
//                    return await _cloudStorageHelper.DownloadFromCloud(azureBlobUrl);
//                }
//                break;
//            case "aws":
//                if (!string.IsNullOrEmpty(_awsS3Url))
//                {
//                    string awsS3Url = _awsS3Url.Replace("{stage}", _cloudoptions.AWS.Stage).Replace("{bucketname}", storage.bucket_or_containername).Replace("{filename}", storage.filename_or_blobname);
//                    return await _cloudStorageHelper.DownloadFromCloud(awsS3Url);
//                }
//                break;
//        }
//        throw new InvalidOperationException("No cloud storage URL configured.");
//    }

   

//    public async Task<RestResponse> ListAllFileAsync(Storage storage)
//    {
//        switch (target.ToLower())
//        {
//            case "azure":
//                if (!string.IsNullOrEmpty(_azureStorageUrl))
//                {
//                    string azureBlobUrl = _azureStorageUrl.Replace("{containername}", storage.bucket_or_containername).Replace("{filename}", storage.filename_or_blobname);
//                    return await _cloudStorageHelper.DownloadFromCloud(azureBlobUrl);
//                }
//                break;
//            case "aws":
//                if (!string.IsNullOrEmpty(_awsS3Url))
//                {
//                    string awsS3Url = _awsS3Url.Replace("{stage}", _cloudoptions.AWS.Stage).Replace("{bucketname}", storage.bucket_or_containername).Replace("{filename}", storage.filename_or_blobname);
//                    return await _cloudStorageHelper.DownloadFromCloud(awsS3Url);
//                }
//                break;
//        }
//        throw new InvalidOperationException("No cloud storage URL configured.");
//    }


//    //private string generateAzureSignature()
//    //{
//    //    string verb = "GET";
//    //    string contentEncoding = "";
//    //    string contentLanguage = "";
//    //    string contentLength = "";
//    //    string contentMD5 = "";
//    //    string contentType = "";
//    //    string date = "Sun, 03 Oct 2023 14:35:00 GMT"; // Replace with the actual date
//    //    string ifModifiedSince = "";
//    //    string ifMatch = "";
//    //    string ifNoneMatch = "";
//    //    string ifUnmodifiedSince = "";
//    //    string range = "";
//    //    string canonicalizedHeaders = "x-ms-date:" + date + "\n" + "x-ms-version:2019-12-12"; // Include relevant headers
//    //    string canonicalizedResource = "/testproject/testcontainer1/r.txt"; // Replace with your actual Azure Storage account, container, and blob details

//    //    // Build the StringToSign
//    //    string stringToSign = $"{verb}\n" +
//    //        $"{contentEncoding}\n" +
//    //        $"{contentLanguage}\n" +
//    //        $"{contentLength}\n" +
//    //        $"{contentMD5}\n" +
//    //        $"{contentType}\n" +
//    //        $"{date}\n" +
//    //        $"{ifModifiedSince}\n" +
//    //        $"{ifMatch}\n" +
//    //        $"{ifNoneMatch}\n" +
//    //        $"{ifUnmodifiedSince}\n" +
//    //        $"{range}\n" +
//    //        $"{canonicalizedHeaders}\n" +
//    //        $"{canonicalizedResource}";

//    //    byte[] keyBytes = Encoding.UTF8.GetBytes("TdCuLYpPzxj3DA7GpjjXy4E+E13QFDH17WnGHc9t75vpwkLFrAWy840s2yuP5nIOoR27OsdLZ4Kt+AStlu1gfw==");
//    //    byte[] inputBytes = Encoding.UTF8.GetBytes(stringToSign);
//    //    string authorizationValue = "";
//    //    using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
//    //    {
//    //        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
//    //        string hmacSha256 = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
//    //        authorizationValue = $"SharedKey testproject:{hmacSha256}";
//    //    }
//    //    return authorizationValue;
//    //}

//}