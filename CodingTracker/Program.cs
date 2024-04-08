using System.Configuration;
using System.Collections.Specialized;

string sAttr;

sAttr = ConfigurationManager.AppSettings.Get("connectionString");

Console.WriteLine(sAttr);
