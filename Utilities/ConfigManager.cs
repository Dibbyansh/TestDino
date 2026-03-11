using Microsoft.Extensions.Configuration;

namespace TestDino.Utilities
{
    public static class ConfigManager
    {
        private static readonly IConfigurationRoot _config;

        static ConfigManager()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public static string BaseUrl => _config["BrowserSettings:BaseUrl"]!;
        public static string Browser => _config["BrowserSettings:Browser"]!;
        public static bool Headless => _config.GetValue<bool>("BrowserSettings:Headless");
        public static int ImplicitWait => _config.GetValue<int>("BrowserSettings:ImplicitWait");
        public static int ExplicitWait => _config.GetValue<int>("BrowserSettings:ExplicitWait");

        // Default timeout for helpers that don't specify a timeout.
        // If DefaultTimeoutSeconds isn't defined, fall back to ExplicitWait.
        public static int DefaultTimeoutSeconds => _config.GetValue<int?>("BrowserSettings:DefaultTimeoutSeconds") ?? ExplicitWait;
    }
}
