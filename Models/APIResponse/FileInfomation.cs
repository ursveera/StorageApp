namespace StorageApp.Models
{
    public class FileInformation
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Access { get; set; }
        public ulong? Size { get; set; }
        public string FileSize { get; set; }
       
    }
    public class FolderInformation
    {
        public string createdOn { get; set; }
        public string createdBy { get; set; }
        public string Size { get; set; }
        public string folderName { get; set; }
    }
    public class FilesList {
        public List<FileInformation> fileInfo { get; set; }
        public List<FolderInformation> folderInfo { get; set; }
    }
}
