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
        public async Task<IActionResult> Delete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Filename is missing.");
            }
            var Response = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
            await Response.DeleteFileAsync(fileName);
            return Ok("File Deleted");
        }
        [HttpPost]
        public async Task<IActionResult> Create(string folderpath)
        {
            if (string.IsNullOrEmpty(folderpath))
            {
                return BadRequest("Folderpath is missing.");
            }
            var Response = _cloudStorageServiceFactory.GetFileStorageService(cloudoptions.Target);
            await Response.CreateFolderAsync(folderpath);
            return Ok("Folder Created");
        }
        [HttpPost]
        public async Task<IActionResult> Put(IFormFile[] files, bool? overridefile = false)
        {
            int TotalFilesCount = 0;
            
            foreach (var file in files)
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
                    if (cloudStorageService.CheckExists(fileName).Result && overridefile == false)
                    {
                        return Ok("File Exists");
                    }
                    await cloudStorageService.UploadFileAsync(fileName, fileData);
                    TotalFilesCount++;
                }
            }
            if (TotalFilesCount == files.Count())
            {
                return Ok("Upload Success");
            }
            return Ok("No File Uploaded");
        }
        [HttpPost]
        public async Task<IActionResult> List(string cloudName, string? prefix = "", string? filterByColumn = "", string? filterValue = "", string? orderBy = "asc", int? page = 0, int? pageSize = 0)
        {
            var cloudStorageService = _cloudStorageServiceFactory.GetFileStorageService(cloudName);
            var response = await cloudStorageService.ListAllFileAsync(prefix);
            List<FileInformation> allFiles = response.ToList().OderByDynamicProperty<FileInformation>(filterByColumn, orderBy).FilterByDynamicProperty<FileInformation>(filterByColumn, filterValue).FormatFileSize();
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
                List<FolderInformation> folders = filesAndFolders.folderInfo.Skip(startIndex).Take(pageSize ?? defaultpageSize).ToList();
                List<FileInformation> files = allFiles.Skip(startIndex).Take((pageSize- folders.Count()) ?? defaultpageSize).ToList();
                filesAndFolders.fileInfo = files;
                filesAndFolders.folderInfo = folders;
                filesAndFolders.TotalItems = totalItems;
                filesAndFolders.TotalPages = totalPages;
                filesAndFolders.Page = page;
                filesAndFolders.PageSize = pageSize;
                return Ok(filesAndFolders);
            }
            else
            {
                filesAndFolders.fileInfo = allFiles;
            }
            return Ok(filesAndFolders);
        }
    }
}