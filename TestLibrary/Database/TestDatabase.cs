using Microsoft.Data.SqlClient;

namespace TestLibrary.Database
{
    public class TestDatabase
    {
        public TestDatabase(string connectionString )
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            connectionStringBuilder.Pooling = false;
            var connectionStringWithoutPooling = connectionStringBuilder.ConnectionString;

            var databaseName = connectionStringBuilder.InitialCatalog;
            var databaseUtils = new DatabaseUtils(connectionStringWithoutPooling);
        }
    }
}
