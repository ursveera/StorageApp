using StorageApp.Models.NOSQL;

namespace StorageApp.Interfaces
{
    public interface INOSQLBuilder
    {
        List<NOSQLInfo> GetListDataBase();
        void AddDataBase(NOSQLInfo info);
        void DeleteDataBase(string name);
        void UpdateDataBase(NOSQLInfo info);

        List<Connections> GetListOfConnections(string dbname);
        void AddNewConnection(Connections connectionInfo,string dbname);
        void DeleteConnection(string dbname,string connectionname);
        void UpdateConnection(Connections connectionInfo,string dbname);

    }
}
