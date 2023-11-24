using StorageApp.Interfaces;

namespace StorageApp.Interfaces_Abstract
{
    public interface INOSQLBuilderFactory
    {
        INOSQLBuilder GetNOSQLBuilder(string cloudname);
    }
}
