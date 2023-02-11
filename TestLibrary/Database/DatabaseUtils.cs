using DbUp;
using Microsoft.Data.SqlClient;

namespace TestLibrary.Database
{
    public class DatabaseUtils
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;
        private readonly string _databaseName;
        public DatabaseUtils(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            _connectionString = connectionString;
            _masterConnectionString = builder.ConnectionString;
            _databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }
        public void CreateDatabase()
        {
            EnsureDatabase.For.SqlDatabase(_connectionString);
        }
        public void DropDatabaseIfExists()
        {
            string command = $"IF EXISTS(SELECT * FROM sys.databases WHERE name='{_databaseName}') DROP DATABASE [{_databaseName}] ";

            RunCommand(_masterConnectionString, command);
        }
        private static void RunCommand(string connectionString, string commandText)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
