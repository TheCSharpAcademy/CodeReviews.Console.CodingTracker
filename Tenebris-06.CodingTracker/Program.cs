using System;
using Dapper;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

class Program {
    static void Main(string[] args)
    {
    
    SqlMapper.AddTypeHandler(new TimeSpanHandler());

    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

    DataAccess db = new DataAccess(configuration["Database:ConnectionString"]);
    db.Initialize();

    var UI = new Menu(db);
    UI.MainMenu();

    }
}