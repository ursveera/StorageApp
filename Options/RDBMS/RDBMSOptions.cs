using StorageApp.Models.RDBMS;

namespace StorageApp.CloudProvider.RDBMS.Builder
{
    public class RDBMSOptions
    {
        public List<RDBMSInfo> Azure { get; set; }
        public List<RDBMSInfo> AWS { get; set; }
        public List<RDBMSInfo> GCP { get; set; }
        public List<RDBMSInfo> NoCloud { get; set; }
    }
}


