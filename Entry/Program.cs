using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Entry;

var dataGetter = new DataGetter();

// Build service collection
var collection = new ServiceCollection();
var sp = collection.BuildServiceProvider();

// Get logger and run main
using (var scope = sp.CreateScope())
{
    string? gamesConnectionString = Environment.GetEnvironmentVariable("NHL_DATABASE");

    if (gamesConnectionString == null)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.Local.json").Build();
        gamesConnectionString = config.GetConnectionString("GAMES_DATABASE");
    }
    if (gamesConnectionString == null)
        throw new Exception("Connection String Null");

    await dataGetter.Main(gamesConnectionString);
}