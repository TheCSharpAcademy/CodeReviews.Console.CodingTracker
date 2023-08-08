using System.Configuration;

using CodingTracker.MartinL_no.DAL;
using CodingTracker.MartinL_no.Models;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

var repo = new CodingSessionRepository(connString, dbPath);
var startTime = DateTime.Now;
var endTime = DateTime.Now.AddDays(1);

var codingSession = new CodingSession(3, startTime, endTime);

//var success = repo.InsertCodingSession(codingSession);
var success = repo.DeleteCodingSession(codingSession);

Console.WriteLine(success);