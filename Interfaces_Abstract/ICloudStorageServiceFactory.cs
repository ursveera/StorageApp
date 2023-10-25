namespace StorageApp.Interfaces
{
    public interface ICloudStorageServiceFactory
    {
        ICloudStorageService GetFileStorageService(string provider);
    }
}
