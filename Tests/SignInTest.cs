using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDino.Base;
using TestDino.Locators;
using TestDino.Utilities;

namespace TestDino.Tests
{
    [TestFixture]
    public class SignInTest : UnauthenticatedBaseTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/login");
            WaitHelper.WaitForPageLoad(_driver);
        }

        [Test]
        public void SignIn_ValidCredentials()
        {
            string title = WaitHelper.WaitForElement(_driver, SignInLocators.Title_SignIn).Text;
            Assert.That(title, Is.EqualTo("Sign In"));
            
            // read credentials from JSON file
            var credentials = JsonHelper.GetTestData<Dictionary<string, string>>("Credentials.json", "ValidCredentials");

            // fill the signin form using the credentials
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_email).SendKeys(credentials["email"]);
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_password).SendKeys(credentials["password"]);

            WaitHelper.ClickWhenClickable(_driver, SignInLocators.Btn_SignIn);

            //assert toast message is displayed after successful signin
            string toastMessage = WaitHelper.WaitForElement(_driver, SignInLocators.SignIn_ToastMessage).Text;
            Assert.That(toastMessage, Is.EqualTo("Logged in successfully"));

            // assert that the user is redirected to the home page after successful signin
            WaitHelper.WaitUntil(_driver, d => d.Url.Equals(ConfigManager.BaseUrl + "/"));
        }

        [Test]
        public void LogOut_AfterSignIn()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/account");
            
            WaitHelper.ClickWhenClickable(_driver, By.CssSelector("div[div = 'menu-item'] p"));

            //assert toast message is displayed after successful signin
            string toastMessage = WaitHelper.WaitForElement(_driver, SignInLocators.SignIn_ToastMessage).Text;
            Assert.That(toastMessage, Is.EqualTo("Logged out successfully"));

            WaitHelper.WaitUntil(_driver, d => d.Url.Equals(ConfigManager.BaseUrl + "/login"));
        }
    }

    [TestFixture]
    public class SignInNegativeTest : UnauthenticatedBaseTest
    {
        [SetUp]
        public void Setup()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/login");
            WaitHelper.WaitForPageLoad(_driver);
        }

        [Test]
        public void SignIn_InvalidCredentials()
        {
            string title = WaitHelper.WaitForElement(_driver, SignInLocators.Title_SignIn).Text;
            Assert.That(title, Is.EqualTo("Sign In"));

            // fill the signin form with invalid credentials
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_email).SendKeys("test@example.com");
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_password).SendKeys("User1234");

            WaitHelper.ClickWhenClickable(_driver, SignInLocators.Btn_SignIn);

            //assert toast message is displayed after successful signin
            string toastMessage = WaitHelper.WaitForElement(_driver, SignInLocators.SignIn_ToastMessage).Text;
            Assert.That(toastMessage, Is.EqualTo("Invalid credentials"));
        }
    }
}
