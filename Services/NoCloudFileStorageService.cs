using Azure;
using FluentFTP;
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
        private readonly string storageConnectionString;
        private readonly string containerName;
        public NoCloudFileStorageService(CloudOptions options)
        {
            cloudoptions = options;
        }

        public async Task<byte[]> DownloadFileAsync(string filename)
        {
            string host = "waws-prod-blu-445.ftp.azurewebsites.windows.net";
            string username = cloudoptions.NoCloud.username; // replace with your username
            string password = cloudoptions.NoCloud.password; // replace with your password
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            string PureFileName = new FileInfo("TotalAmount").Name;
            String uploadUrl = String.Format("{0}/{1}/{2}", "ftp://waws-prod-blu-445.ftp.azurewebsites.windows.net", "site/wwwroot","veera.json");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(username, password);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Copy the contents of the file to the request stream.  
            StreamReader sourceStream = new StreamReader(@"C:\Users\Ursve\Downloads\OK_r.txt");
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
           
            return null;
        }
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Add logic here to validate the SSL certificate, if required
            return  true; // For this example, we are accepting any certificate
        }

        public async Task<List<FileInformation>> ListAllFileAsync(string prefix)
        {
            return null;
        }

        public async Task UploadFileAsync(string filename, byte[] content)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(cloudoptions.NoCloud.username, cloudoptions.NoCloud.password);
                client.UploadData(cloudoptions.NoCloud.ftpsendpoint + "/" + filename, content);
            }
        }

        public async Task<FilesList> ListAllFileAndFoldersAsync(string prefix)
        {

            return null;
        }

        public Task DeleteFileAsync(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
