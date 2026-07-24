using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false, true).Build();

        public static string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection") ?? "Data Source=coding-tracker.db";
        }
    }
}
