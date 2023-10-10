namespace StorageApp.CloudProvider.Config
{
    public class AZUREOptions
    {
       
        public string BlobContainerName { get; set; }
        public string ListFilesQueryString { get; set; }
        public string SASToken { get; set; }
        public string StorageUrl { get; set; }
    }
}
