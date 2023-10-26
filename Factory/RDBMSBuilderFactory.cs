using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract;
using StorageApp.Services;

namespace StorageApp.Factory
{
    public class RDBMSBuilderFactory : IRDBMSBuilderFactory
    {
        public IRDBMSBuilder GetRDGMSBuiler(string cloudname)
        {
            switch (cloudname.ToLower())
            {
                case "azure":
                    return new AZUREBuilder();
                case "aws":
                    return new AWSBuilder();
                case "gcp":
                    return new GCPBuilder();
                case "nocloud":
                    return new NOCLOUDBuilder();
                default:
                    throw new ArgumentException("Invalid RDBMS cloud provider.");
            }
        }
    }
}
