using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.Hacker_735
{
    internal static class AppConfig
    {
        private static readonly IConfiguration _configuration;

        static AppConfig()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        internal static string ConnectionString =>
            _configuration.GetConnectionString("Default");
    }
}
