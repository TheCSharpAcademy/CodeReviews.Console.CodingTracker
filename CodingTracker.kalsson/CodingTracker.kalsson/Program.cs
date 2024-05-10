using ConfigurationProvider = CodingTracker.kalsson.Configuration.ConfigurationProvider;

// Initialize configuration
IConfiguration configuration = ConfigurationProvider.CreateConfiguration();

// Initialize the database using the connection string from the configuration
DatabaseInitializer.InitializeDatabase(configuration.GetConnectionString("DefaultConnection"));

// Continue with the rest of your application logic