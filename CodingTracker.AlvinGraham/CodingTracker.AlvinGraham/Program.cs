using CodingTracker;
using Microsoft.Extensions.Configuration;


IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

string connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"]!;

var dataAccess = new DataAccess(connectionString);

dataAccess.CreateDatabase();
