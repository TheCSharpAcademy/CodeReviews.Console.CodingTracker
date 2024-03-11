using System.Configuration;

namespace CodingTracker;

public class Configuration
{
    public string GetConfigurationItemByKey(string key)
    {
        return ConfigurationManager.AppSettings.Get(key) ?? throw new KeyNotFoundException();
    }
}