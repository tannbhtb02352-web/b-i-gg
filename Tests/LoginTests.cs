using bai_thu.Pages;

namespace bai_thu.Tests;

[FixtureLifeCycle(LifeCycle.SingleInstance)]
public sealed class LoginTests : BaseTest
{
    [Test]
    public void ValidCredentials_OpenSecureArea()
    {
        var loginPage = new LoginPage(Driver, Settings.WaitSeconds)
            .Open(Settings.BaseUrl);
        loginPage.SubmitCredentials("tomsmith", "SuperSecretPassword!");
        var secureArea = new SecureAreaPage(Driver, Settings.WaitSeconds);

        Assert.Multiple(() =>
        {
            Assert.That(Driver.Url, Does.EndWith("/secure"));
            Assert.That(secureArea.GetFlashMessage(), Does.Contain("You logged into a secure area!"));
            Assert.That(secureArea.IsLogoutButtonVisible(), Is.True);
        });
    }

    [Test]
    public void InvalidCredentials_ShowErrorMessage()
    {
        var loginPage = new LoginPage(Driver, Settings.WaitSeconds)
            .Open(Settings.BaseUrl);

        loginPage.SubmitCredentials("wrong-user", "wrong-password");

        Assert.That(loginPage.GetFlashMessage(), Does.Contain("Your username is invalid!"));
        Assert.That(Driver.Url, Does.EndWith("/login"));
    }
}
