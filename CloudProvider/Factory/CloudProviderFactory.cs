using StorageApp.CloudProvider.Providers;

namespace StorageApp.CloudProvider.Factory
{
    public class CloudProviderFactory: ICloudProviderFactory
    {
        private readonly AzureProvider _azureProvider;
        private readonly AWSProvider _awsProvider;
        public CloudProviderFactory(AzureProvider azureProvider, AWSProvider awsProvider)
        {
            _azureProvider = azureProvider;
            _awsProvider = awsProvider;
        }
        public ICloudProvider GetCloudProvider(string target)
        {
            if (target == "azure")
            {
                return _azureProvider;
            }
            else if (target == "aws")
            {
                return _awsProvider;
            }
            return null;
        }
    }
}
