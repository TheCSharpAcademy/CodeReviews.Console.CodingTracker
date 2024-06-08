using System.Collections.Specialized;

namespace CodingTracker.Arashi256.Config
{
    internal class AppManager
    {
        public string? DatabaseConnectionString { get; private set; }
        private NameValueCollection? _appConfig;

        public AppManager()
        {
            try
            {
                _appConfig = System.Configuration.ConfigurationManager.AppSettings;
                if (_appConfig.Count == 0)
                {
                    Console.WriteLine("\nERROR: AppSettings is empty or cannot be read.\n");
                }
                else
                {
                    DatabaseConnectionString = _appConfig.Get("ConnectionString");
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("\nERROR: Could not read app settings\n");
            }
        }
    }
}
