using bai_thu.Configuration;
using bai_thu.Driver;
using OpenQA.Selenium;

namespace bai_thu.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver { get; private set; } = null!;
    protected TestSettings Settings { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Settings = TestSettings.Load();
        Driver = DriverFactory.Create(Settings);
    }

    [TearDown]
    public void TearDown()
    {
        if (Driver is null)
        {
            return;
        }

        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed
                && Driver is ITakesScreenshot screenshotDriver)
            {
                var screenshotsDirectory = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    "artifacts",
                    "screenshots");
                Directory.CreateDirectory(screenshotsDirectory);

                var testName = string.Concat(
                    TestContext.CurrentContext.Test.Name.Select(character =>
                        Path.GetInvalidFileNameChars().Contains(character) ? '_' : character));
                var screenshotPath = Path.Combine(screenshotsDirectory, $"{testName}.png");
                screenshotDriver.GetScreenshot().SaveAsFile(screenshotPath);
                TestContext.AddTestAttachment(screenshotPath);
            }
        }
        finally
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
