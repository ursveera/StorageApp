using Newtonsoft.Json.Serialization;

namespace StorageApp.CloudProvider.Config
{
    public class AZUREOptions
    {
        public string StorageUrl { get; set; }
        public string BlobContainerName { get; set; }
        public string ListFilesQueryString { get; set; }
        public string SASToken { get; set; }
    }
}
