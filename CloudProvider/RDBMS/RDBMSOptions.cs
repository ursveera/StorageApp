namespace StorageApp.CloudProvider.RDBMS
{
    public class RDBMSOptions
    {
        public MSSQLOptions MicrosoftSQLServer { get; set; }
        public MySQLOptions MySQL { get; set; }
        public string Target { get; set; }
    }
}
