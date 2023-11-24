using StorageApp.Models.NOSQL;

namespace StorageApp.CloudProvider.NOSQL
{
    public class NOSQLOptions
    {
        public List<NOSQLInfo> Azure { get; set; }
        public List<NOSQLInfo> AWS { get; set; }
        public List<NOSQLInfo> GCP { get; set; }
        public List<NOSQLInfo> NoCloud { get; set; }
    }
}


