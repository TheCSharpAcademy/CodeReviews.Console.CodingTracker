using System.Configuration;

string connectionString = ConfigurationManager.AppSettings.Get("connString");

Console.WriteLine("The value of the connString is " + connectionString);

Console.ReadLine();