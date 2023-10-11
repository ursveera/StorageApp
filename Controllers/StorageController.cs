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
    [CustomExceptionFilter]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IConfiguration _config;
        private readonly CloudOptions cloudoptions;
        private readonly ICloudStorageServiceFactory _cloudStorageServiceFactory;

        public StorageController(ILogger<StorageController> logger, IConfiguration config, ICloudStorageServiceFactory cloudStorageServiceFactory, IOptionsMonitor<CloudOptions> options)
        {
            _logger = logger;
            _config = config;
            cloudoptions = options.CurrentValue;
            _cloudStorageServiceFactory = cloudStorageServiceFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest("Filename is missing.");
            }
            var Response = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
            byte[] _file = await Response.DownloadFileAsync(filename);
            if (_file == null || _file == null || _file.Length == 0)
            {
                return NotFound("File not found.");
            }
            byte[]? data = _file;
            string DownloadFileName = Path.GetFileNameWithoutExtension(filename) + Guid.NewGuid().ToString() + "_" + Path.GetExtension(filename);
            return File(data, MimeMapping.GetContentTypeFromExtension(filename), DownloadFileName);
        }
        [HttpPost]
        public async Task<IActionResult> Put(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                string fileName = file.FileName;
                var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
                await cloudStorageService.UploadFileAsync(fileName, fileData);
                return Ok("Upload Success");
            }
        }
        [HttpPost]
        public async Task<IActionResult> List()
        {
            var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
            var response = await cloudStorageService.ListAllFileAsync();
            return Ok(response);
        }
    }
}