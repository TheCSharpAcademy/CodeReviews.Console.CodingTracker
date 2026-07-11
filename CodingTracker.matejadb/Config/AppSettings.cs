using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.Config;

public static class AppSettings {
    public static string ConnectionString { get; }
    public static string DateFormat { get; }

    static AppSettings() {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
        DateFormat = configuration["DateFormat"] ?? "yyyy-MM-dd HH:mm";
    }

}
