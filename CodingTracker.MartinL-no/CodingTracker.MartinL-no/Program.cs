using System.Configuration;

using CodingTracker.MartinL_no.Models;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

Console.WriteLine(connString + dbPath);

var start = DateTime.Now.AddDays(1);
var end = DateTime.Now.AddDays(2);
var session = new CodingSession(start, end);
