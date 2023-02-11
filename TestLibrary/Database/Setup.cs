using Microsoft.Extensions.Configuration;

namespace TestLibrary.Database
{
    [TestClass]
    internal class Setup
    {
        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            string? gamesConnectionString = Environment.GetEnvironmentVariable("NHL_DATABASE");

            if (gamesConnectionString == null)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.Local.json").Build();
                gamesConnectionString = config.GetConnectionString("NHL_DATABASE");
            }
            if (gamesConnectionString == null)
                throw new Exception("Connection String Null");

            // Create database
            var databaseUtils = new DatabaseUtils(gamesConnectionString);

            databaseUtils.DropDatabaseIfExists();
            databaseUtils.CreateDatabase();
        }
    }
}
