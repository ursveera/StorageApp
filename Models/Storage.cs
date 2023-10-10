namespace StorageApp.Models
{
    public class Storage
    {
        public string filename_or_blobname { get; set; }
        public string bucket_or_containername { get; set; }
        public string storage_provider { get; set; }
        public string FROM_storage_provider { get; set; }
        public string TO_storage_provider { get; set; }
    }
}
