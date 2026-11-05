using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace MyAutomationFramework.Tests
{
    public class LoginTests
    {
        private IWebDriver _driver = null!;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void OpenGoogleAndSearchTest()
        {
            _driver.Navigate().GoToUrl("https://www.google.com");

            IWebElement searchBox = _driver.FindElement(By.Name("q"));
            searchBox.SendKeys("Selenium C# Automation");
            searchBox.SendKeys(Keys.Enter);

            // Thêm lệnh chờ tối đa 10 giây cho đến khi tiêu đề trang thay đổi chứa từ khóa tìm kiếm
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Title.Contains("Selenium"));

            // Kiểm tra tiêu đề trang
            Assert.That(_driver.Title, Does.Contain("Selenium"));
        }

        [TearDown]
        public void Teardown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    }
}