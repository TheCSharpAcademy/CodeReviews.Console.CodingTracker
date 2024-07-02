using CodingTracker;
using Microsoft.Extensions.Configuration;


IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

string connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"]!;

Console.WriteLine(connectionString);

var dataAccess = new DataAccess(connectionString);

dataAccess.CreateDatabase();

UserInterface.MainMenu();