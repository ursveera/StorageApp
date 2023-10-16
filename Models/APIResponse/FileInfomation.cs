namespace StorageApp.Models
{
    public class FileInformation
    {
        public string ID { get; set; }
        public string fileName { get; set; }
        public string fileType { get; set; }
        public string createdOn { get; set; }
        public string createdBy { get; set; }
        public string access { get; set; }
        public ulong? size { get; set; }
        public string fileSize { get; set; }

    }
    public class FolderInformation
    {
        public string createdOn { get; set; }
        public string createdBy { get; set; }
        public string size { get; set; }
        public string folderName { get; set; }
    }
    public class FilesList
    {
        public List<FileInformation> fileInfo { get; set; }
        public List<FolderInformation> folderInfo { get; set; }
        public int? TotalItems { get; set; }
        public int? TotalPages { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
