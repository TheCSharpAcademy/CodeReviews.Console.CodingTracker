using System.Configuration;

using CodingTracker.MartinL_no.DAL;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

var repo = new CodingSessionRepository(connString, dbPath);
var codingSessions = repo.GetCodingSessions();
Console.WriteLine();