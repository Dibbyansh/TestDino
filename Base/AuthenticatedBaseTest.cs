using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDino.Driver;
using TestDino.Locators;
using TestDino.Utilities;

namespace TestDino.Base
{
    public class AuthenticatedBaseTest
    {
        protected IWebDriver _driver;

        [OneTimeSetUp]  // Runs ONCE - login happens once
        public void OneTimeSetup()
        {
            _driver = DriverFactory.InitDriver();
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/login");
            WaitHelper.WaitForPageLoad(_driver);

            string email = JsonHelper.GetTestData<string>("Credentials.json", "ValidCredentials.email");
            string pass = JsonHelper.GetTestData<string>("Credentials.json", "ValidCredentials.password");

            WaitHelper.WaitForElement(_driver, SignInLocators.Input_email).SendKeys(email);
            WaitHelper.WaitForElement(_driver, SignInLocators.Input_password).SendKeys(pass);
            WaitHelper.ClickWhenClickable(_driver, SignInLocators.Btn_SignIn);

            // Wait until redirected to home — confirms login succeeded before any test runs
            WaitHelper.WaitUntil(_driver, d => d.Url.Equals(ConfigManager.BaseUrl + "/"));
        }

        [TearDown]
        public void TearDown()
        {
           
        }

        [OneTimeTearDown]  // Cleanup ONCE after all tests
        public void OneTimeTearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
