using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace bai_thu.Pages;

public sealed class LoginPage : BasePage
{
    private static readonly By UsernameInput = By.Id("username");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By LoginButton = By.CssSelector("button[type='submit']");
    private static readonly By FlashMessage = By.Id("flash");

    public LoginPage(IWebDriver driver, int waitSeconds = 10) : base(driver, waitSeconds)
    {
    }

    public LoginPage Open(string baseUrl)
    {
        Driver.Navigate().GoToUrl($"{baseUrl}/login");
        Wait.Until(driver => driver.FindElement(UsernameInput).Displayed);
        return this;
    }

    public void SubmitCredentials(string username, string password)
    {
        EnterText(UsernameInput, username);
        EnterText(PasswordInput, password);
        Click(LoginButton);
    }

    public string GetFlashMessage() => GetText(FlashMessage).Trim();
}

public sealed class SecureAreaPage : BasePage
{
    private static readonly By FlashMessage = By.Id("flash");
    private static readonly By LogoutButton = By.CssSelector("a.button[href='/logout']");

    public SecureAreaPage(IWebDriver driver, int waitSeconds = 10) : base(driver, waitSeconds)
    {
    }

    public string GetFlashMessage() => GetText(FlashMessage).Trim();

    public bool IsLogoutButtonVisible()
    {
        try
        {
            return FindVisible(LogoutButton).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
