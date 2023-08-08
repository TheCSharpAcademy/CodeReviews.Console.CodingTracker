using System.Configuration;

using CodingTracker.MartinL_no.DAL;
using CodingTracker.MartinL_no.Models;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

var repo = new CodingSessionRepository(connString, dbPath);
var startTime = DateTime.Now;
var endTime = DateTime.Now.AddDays(1);
var success = repo.InsertCodingSession(new CodingSession(startTime, endTime));
Console.WriteLine(success);