using StorageApp.CloudProvider.RDBMS.Builder;
using StorageApp.Models.RDBMS;

namespace StorageApp.Interfaces
{
    public interface IRDBMSBuilder
    {
        List<RDBMSInfo> GetListDataBase();
        void AddDataBase(RDBMSInfo info);

        List<Connections> GetListOfConnections(string dbname);
        void AddNewConnection(Connections connectionInfo,string dbname);

    }
}
