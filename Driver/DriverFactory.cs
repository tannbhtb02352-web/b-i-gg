using bai_thu.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace bai_thu.Driver;

public static class DriverFactory
{
    public static IWebDriver Create(TestSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        IWebDriver driver = settings.Browser switch
        {
            "chrome" => new ChromeDriver(CreateChromeOptions(settings)),
            "firefox" => new FirefoxDriver(CreateFirefoxOptions(settings)),
            "edge" => new EdgeDriver(CreateEdgeOptions(settings)),
            _ => throw new ArgumentOutOfRangeException(
                nameof(settings),
                settings.Browser,
                "Unsupported browser. Use chrome, firefox, or edge.")
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        if (!settings.Headless)
        {
            driver.Manage().Window.Maximize();
        }

        return driver;
    }

    private static ChromeOptions CreateChromeOptions(TestSettings settings)
    {
        var options = new ChromeOptions();
        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1440,1000");
        }

        return options;
    }

    private static FirefoxOptions CreateFirefoxOptions(TestSettings settings)
    {
        var options = new FirefoxOptions();
        if (settings.Headless)
        {
            options.AddArgument("-headless");
        }

        return options;
    }

    private static EdgeOptions CreateEdgeOptions(TestSettings settings)
    {
        var options = new EdgeOptions();
        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1440,1000");
        }

        return options;
    }
}
