﻿using StorageApp.CloudProvider.RDBMS.Builder;
using StorageApp.Interfaces;
using StorageApp.Models.RDBMS;

namespace StorageApp.Services.RDBMS
{
    public class RDBMSDirector
    {
        private readonly IRDBMSBuilder _builder;

        public RDBMSDirector(IRDBMSBuilder builder)
        {
            _builder = builder;
        }
        public void Construct(RDBMSInfo info)
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
        public List<RDBMSInfo> GetDB()
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
        public void UpdateDB(RDBMSInfo rdbmsinfo)
        {
            _builder.UpdateDataBase(rdbmsinfo);
        }
        public void UpdateConnection(Connections connectionInfo, string dbname) 
        { 
            _builder.UpdateConnection(connectionInfo,dbname);
        }
    }
}
