using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spectre.Console;

internal static class Config
{
    public static string ConnectionString { get; set; }
    private const string AppSettingsFile = "appsettings.json";
    public const string DateFormat = "yyyy-MM-dd";
    public const string TimeFormat = "HH:mm";
    public static string DateParsingFormat = "yyyy-MM-dd HH:mm:ss";

    static Config()
    {
        CheckConfigFile();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettingsFile)
            .Build();
        ConnectionString = configuration.GetConnectionString("Database") ?? "";

        SqlMapper.AddTypeHandler(new TimeSpanHandler());
    }

    private static void CheckConfigFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFile);

        if (!File.Exists(filePath))
        {
            var config = new
            {
                ConnectionStrings = new
                {
                    Database = "Data Source=sessions.db;"
                }
            };

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            try
            {
                File.WriteAllText(filePath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                AnsiConsole.MarkupLine($"[red]Failed to write to \"{AppSettingsFile}\" file.[/]");
                AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
                DisplayInfoHelpers.PressAnyKeyToContinue();
            }
        }
    }
}
