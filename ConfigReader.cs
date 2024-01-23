using Microsoft.Extensions.Configuration;

namespace CodingTracker
{
    public class ConfigReader
    {
        public IConfigurationRoot Configuration { get; }

        public ConfigReader()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //presence of the file is not optional, file reloads on change
                .Build();
        }
        public string GetConnectionString() => Configuration.GetConnectionString("DefaultConnection");
        public string GetFileNameString() => Configuration.GetConnectionString("FileName");

    }
}