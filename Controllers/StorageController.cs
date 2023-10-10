using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Interfaces;
using StorageApp.Models;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace StorageApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StorageController : ControllerBase
    {
        private readonly string temppath;
        private readonly ILogger<StorageController> _logger;
        private readonly IConfiguration _config;
        private readonly CloudOptions cloudoptions;
        private readonly ICloudStorageServiceFactory _cloudStorageServiceFactory;

        public StorageController(ILogger<StorageController> logger,IConfiguration config, ICloudStorageServiceFactory cloudStorageServiceFactory, IOptionsMonitor<CloudOptions> options)
        {
            _logger = logger;
            //cloudstorage = storage;
            _config = config;
            cloudoptions = options.CurrentValue ;
            temppath = config["TempStoragePath"];
            _cloudStorageServiceFactory= cloudStorageServiceFactory;
            //    List<FileInformation> files = new List<FileInformation>();
            //    var a = new FileInformation() {ID=1, FileName="Filename1", FileType=".png", CreatedOn=DateTime.Now, CreatedBy="Personname", Access="Access Detail" };
            //    files.Add(a);
            //    files.Add(a);
            //    files.Add(a);
            //    files.Add(a);
            //    files.Add(a);
            //    string jsondata=JsonConvert.SerializeObject(files);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string filename)
        {
            var Response = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
            var _file = await Response.DownloadFileAsync(filename);
            byte[]? data = _file.RawBytes;
            System.IO.File.WriteAllBytes(Path.Combine(temppath, filename), data);
            string DownloadFileName = Path.GetFileNameWithoutExtension(filename) + Guid.NewGuid().ToString() + "_" + Path.GetExtension(filename);
            return File(data, MimeMapping.GetContentTypeFromExtension(filename), DownloadFileName);
        }
        [HttpPost]
        public string Put(IFormFile file)
        {
            byte[] datum = null;
            using (var data = file.OpenReadStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    data.CopyToAsync(ms);
                    datum = ms.ToArray();
                }
                var resp = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target).UploadFileAsync(file.FileName, datum);
            }
            return "Upload Success";
        }
        [HttpPost]
        public async Task<IActionResult> List()
        {
            var response = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target).ListAllFileAsync().Result;
            return Ok(response);
        }
        //[HttpPost]
        //public string MoveFile_AzureTOAWS_AWSTOAZURE(string filename)
        //{
        //    var _file = cloudstorage.DownloadFileAsync(filename).Result;
        //    byte[]? data = _file.RawBytes;
        //    cloudstorage.UploadFileAsync(filename, data);
        //    return "OK";
        //}
    }
}