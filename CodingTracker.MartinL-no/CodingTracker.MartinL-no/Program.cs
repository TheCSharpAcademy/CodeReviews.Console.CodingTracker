using System.Configuration;
using System.Collections.Specialized;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

Console.WriteLine(connString + dbPath);
