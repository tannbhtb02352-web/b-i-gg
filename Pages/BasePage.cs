using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace bai_thu.Pages;

public abstract class BasePage
{
    protected BasePage(IWebDriver driver, int waitSeconds = 10)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
    }

    protected IWebDriver Driver { get; }
    protected WebDriverWait Wait { get; }

    protected IWebElement FindVisible(By locator) =>
        Wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed ? element : null;
        })!;

    protected void EnterText(By locator, string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        var element = FindVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected void Click(By locator)
    {
        var element = Wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed && element.Enabled ? element : null;
        });
        element!.Click();
    }

    protected string GetText(By locator) => FindVisible(locator).Text;
}
