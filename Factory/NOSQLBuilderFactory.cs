using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract;
using StorageApp.Services;

namespace StorageApp.Factory
{
    public class NOSQLBuilderFactory : INOSQLBuilderFactory
    {
        public INOSQLBuilder GetNOSQLBuilder(string cloudname)
        {
            switch (cloudname.ToLower())
            {
                case "azure":
                    return new nosql_AZUREBuilder();
                case "aws":
                    return new nosql_AWSBuilder();
                case "gcp":
                    return new nosql_GCPBuilder();
                case "nocloud":
                    return new nosql_NOCLOUDBuilder();
                default:
                    throw new ArgumentException("Invalid NOSQL cloud provider.");
            }
        }
    }
}
