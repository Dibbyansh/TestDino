using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDino.Utilities;

namespace TestDino.Driver
{
    public static class DriverFactory
    {
        public static IWebDriver InitDriver()
        {

            switch (ConfigManager.Browser)
            {
                case "Edge":
                    var edgeOptions = new EdgeOptions();

                    if (ConfigManager.Headless)
                        edgeOptions.AddArgument("--headless=new");

                    // Performance / CI flags
                    edgeOptions.AddArgument("--disable-extensions");
                    edgeOptions.AddArgument("--no-sandbox");
                    edgeOptions.AddArgument("--disable-dev-shm-usage");

                    return new EdgeDriver(edgeOptions);

                default:
                    var chromeOptions = new ChromeOptions();

                    if (ConfigManager.Headless)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }

                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--disable-extensions");
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");

                    return new ChromeDriver(chromeOptions);
            }
        }
    }
}
