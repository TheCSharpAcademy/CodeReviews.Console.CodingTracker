// Set up the configuration
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Build();

// Initialize the database using the connection string from the configuration
DatabaseInitializer.InitializeDatabase(configuration.GetConnectionString("DefaultConnection"));

// Continue with the rest of your application logic