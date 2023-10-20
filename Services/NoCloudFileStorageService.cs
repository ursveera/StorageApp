using Amazon.Runtime.Internal;
using Azure;
using Azure.Core;
using FluentFTP;
using FluentFTP.Helpers;
using FluentFTP.Rules;
using Google.Api.Gax.ResourceNames;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using StorageApp.Models.APIResponse;
using System.Drawing;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StorageApp.Services
{
    public class NoCloudFileStorageService : ICloudStorageService
    {
        private readonly CloudOptions? cloudoptions;
        public NoCloudFileStorageService(CloudOptions options)
        {
            cloudoptions = options;

        }

        public async Task<byte[]> DownloadFileAsync(string filename)
        {
            var request = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + filename);
            request.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            using (var response = (FtpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var memoryStream = new MemoryStream())
            {
                responseStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        public async Task<List<FileInformation>> ListAllFileAsync(string prefix)
        {
            return null;
        }

        public async Task UploadFileAsync(string filename, byte[] content)
        {
            var request = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + filename);
            request.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(content, 0, content.Length);
            }
            using (var response = (FtpWebResponse)request.GetResponse()) ;

        }

        public async Task<FilesList> ListAllFileAndFoldersAsync(string prefix)
        {
            List<FileInformation> fileInformationList = new List<FileInformation>();
            List<FolderInformation> folderinformationList = new List<FolderInformation>();
            var filesList = new FilesList
            {
                fileInfo = new List<FileInformation>(),
                folderInfo = new List<FolderInformation>()
            };
            var ftpWebRequest = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + prefix);
            var ftpWebRequest1 = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + prefix);
            ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            ftpWebRequest.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
            FtpWebResponse response = (FtpWebResponse)await ftpWebRequest.GetResponseAsync();
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string line = reader.ReadToEnd();
                string[] lines = line.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string singleline in lines)
                {
                    string[] tokens = singleline.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string fileName = tokens[8];
                    if (tokens.Length>9) fileName = tokens[8] +" "+ tokens[9];
                    string fileSize = tokens[4];
                    string fileCreatedOn = tokens[5] + " " + tokens[6] + " " + tokens[7];
                    if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                    {
                        filesList.folderInfo.Add(new FolderInformation
                        {
                            createdBy = cloudoptions.NoCloud.username,
                            createdOn = fileCreatedOn,
                            size = fileSize,
                            folderName = fileName
                        });
                    }
                    else
                    {
                        filesList.fileInfo.Add(new FileInformation
                        {
                            fileName = Path.GetFileName(fileName),
                            fileType = Path.GetFileName(fileName).Substring(Path.GetFileName(fileName).LastIndexOf('.') + 1),
                            createdOn = fileCreatedOn,
                            createdBy = cloudoptions.NoCloud.username,
                            access = "Full",
                            size = (ulong)Convert.ToInt32(fileSize),
                        });
                    }
                }
            }
            return filesList;
        }

        public async Task DeleteFileAsync(string filename)
        {
            if (Path.GetExtension(filename) != "")
            {
                var ftpWebRequest = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + filename);
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebRequest.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
                using (var response = (FtpWebResponse)await ftpWebRequest.GetResponseAsync()) ;
            }
            else
            {
                var ftpWebRequest = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + filename);
                ftpWebRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                ftpWebRequest.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
                using (var response = (FtpWebResponse)await ftpWebRequest.GetResponseAsync()) ;
            }

        }

        public async Task CreateFolderAsync(string folderpath)
        {
            var ftpWebRequest = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + folderpath);
            ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            ftpWebRequest.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
            using (var response = (FtpWebResponse)await ftpWebRequest.GetResponseAsync()) ;
        }

        public async Task<bool> CheckExists(string filename)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(cloudoptions.NoCloud.host + filename);
                request.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    var file = memoryStream.ToArray();
                    if (file.Length <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (System.Net.WebException e)
            {
                return false;
            }

        }
    }
}
