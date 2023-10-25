using StorageApp.CloudProvider.RDBMS.Builder;
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
    }
}
