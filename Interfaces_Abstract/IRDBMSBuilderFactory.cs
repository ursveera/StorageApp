using StorageApp.Interfaces;

namespace StorageApp.Interfaces_Abstract
{
    public interface IRDBMSBuilderFactory
    {
        IRDBMSBuilder GetRDGMSBuiler(string cloudname);
    }
}
