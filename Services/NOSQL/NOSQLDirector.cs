using StorageApp.Interfaces;
using StorageApp.Models.NOSQL;

namespace StorageApp.Services.NOSQL
{
    public class NOSQLDirector
    {
        private readonly INOSQLBuilder _builder;

        public NOSQLDirector(INOSQLBuilder builder)
        {
            _builder = builder;
        }
        public void Construct(NOSQLInfo info)
        {
            _builder.AddDataBase(info);
        }
        public void ConstructConnection(Connections connections,string dbname)
        {
            _builder.AddNewConnection(connections, dbname);
        }
        public List<Connections> GetConnections(string dbname)
        {
            return  _builder.GetListOfConnections(dbname);
        }
        public List<NOSQLInfo> GetDB()
        {
            return _builder.GetListDataBase();
        }
        public void DeleteDB(string name)
        {
            _builder.DeleteDataBase(name); 
        }
        public void DeleteConnection(string dbname, string connectionname) {
            _builder.DeleteConnection(dbname, connectionname);
        }
        public void UpdateDB(NOSQLInfo rdbmsinfo)
        {
            _builder.UpdateDataBase(rdbmsinfo);
        }
        public void UpdateConnection(Connections connectionInfo, string dbname) 
        { 
            _builder.UpdateConnection(connectionInfo,dbname);
        }
    }
}
