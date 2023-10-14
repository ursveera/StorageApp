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
        public async Task<IActionResult> Get(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Filename is missing.");
            }
            var Response = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
            byte[] _file = await Response.DownloadFileAsync(fileName);
            if (_file == null || _file == null || _file.Length == 0)
            {
                return NotFound("File not found.");
            }
            byte[]? data = _file;
            string DownloadFileName = Path.GetFileNameWithoutExtension(fileName) + Guid.NewGuid().ToString() + "_" + Path.GetExtension(fileName);
            return File(data, MimeMapping.GetContentTypeFromExtension(fileName), DownloadFileName);
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
        public async Task<IActionResult> List(string cloudName,string? prefix="",string? filterByColumn= "",string? filterValue="",string? orderBy="asc", int? page = 0, int? pageSize = 0)
        {
            var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudName);
            var response = await cloudStorageService.ListAllFileAsync(prefix);
            List<FileInformation> allFiles = response.ToList().OderByDynamicProperty<FileInformation>(filterByColumn, orderBy).FilterByDynamicProperty<FileInformation>(filterByColumn, filterValue).FormatFileSize();
            int defaultpageSize = 10;
            int defaultPage = 1;
            if (page > 0 && pageSize > 0)
            {
                int totalItems = allFiles.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize?? defaultpageSize);
                page = Math.Max(1, Math.Min(totalPages, page?? defaultPage));
                pageSize = Math.Max(1, pageSize?? defaultpageSize);
                int startIndex = (page - 1) * pageSize?? defaultpageSize;
                List<FileInformation> files = allFiles.Skip(startIndex).Take(pageSize?? defaultpageSize).ToList();
                var result = new
                {
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Page = page,
                    PageSize = pageSize,
                    Files = files
                };
                return Ok(result);
            }
            return Ok(allFiles);
        }
        [HttpPost]
        public async Task<IActionResult> ListFilesAndFolders(string cloudName, string? folderName = "", string? filterByColumn = "FileName", string? filterValue = "", string? orderBy = "asc", string? oderByColumn = "FileName", int? page = 0, int? pageSize = 0)
        {
            var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudName);
            var filesAndFolders = await cloudStorageService.ListAllFileAndFoldersAsync(folderName);
            List<FileInformation> allFiles = filesAndFolders.fileInfo.OderByDynamicProperty<FileInformation>(oderByColumn, orderBy).FilterByDynamicProperty<FileInformation>(filterByColumn, filterValue).FormatFileSize();
            int defaultpageSize = 10;
            int defaultPage = 1;
            if (page > 0 && pageSize > 0)
            {
                int totalItems = allFiles.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize ?? defaultpageSize);
                page = Math.Max(1, Math.Min(totalPages, page ?? defaultPage));
                pageSize = Math.Max(1, pageSize ?? defaultpageSize);
                int startIndex = (page - 1) * pageSize ?? defaultpageSize;
                List<FileInformation> files = allFiles.Skip(startIndex).Take(pageSize ?? defaultpageSize).ToList();
                var result = new
                {
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Page = page,
                    PageSize = pageSize,
                    Files = files
                };
                return Ok(result);
            }
            filesAndFolders.fileInfo = allFiles;
            return Ok(filesAndFolders);
        }
        //[HttpPost]
        //public async Task<IActionResult> ListByAsc(string columnname)
        //{
        //    var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
        //    var response = await cloudStorageService.ListAllFileAsync();
        //    List<FileInformation> files = response.ToList().OderByDynamicProperty<FileInformation>(columnname, "asc").FormatFileSize();
        //    return Ok(files);
        //}        
        //[HttpPost]
        //public async Task<IActionResult> Filter(string filterbycolumn, string value,string order)
        //{
        //    var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
        //    var response = await cloudStorageService.ListAllFileAsync();
        //    List<FileInformation> files = response.ToList().FilterByDynamicProperty<FileInformation>(filterbycolumn, value).FormatFileSize();
        //    return Ok(files);
        //}
        //[HttpPost]
        //public async Task<IActionResult> ListPaginated(int page = 1, int pageSize = 10)
        //{
        //    var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
        //    var response = await cloudStorageService.ListAllFileAsync();
        //    List<FileInformation> allFiles = response.FormatFileSize();

        //    // Calculate the total number of items and the number of pages.
        //    int totalItems = allFiles.Count;
        //    int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    // Ensure that page and pageSize are within valid ranges.
        //    page = Math.Max(1, Math.Min(totalPages, page));
        //    pageSize = Math.Max(1, pageSize);

        //    // Calculate the starting index for the current page.
        //    int startIndex = (page - 1) * pageSize;

        //    // Get the files for the current page.
        //    List<FileInformation> files = allFiles.Skip(startIndex).Take(pageSize).ToList();

        //    // Create a response object that includes pagination information.
        //    var result = new
        //    {
        //        TotalItems = totalItems,
        //        TotalPages = totalPages,
        //        Page = page,
        //        PageSize = pageSize,
        //        Files = files
        //    };

        //    return Ok(result);
        //}
    }
}