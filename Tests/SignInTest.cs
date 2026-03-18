using OpenQA.Selenium;
using TestDino.Base;
using TestDino.Locators;
using TestDino.Utilities;

namespace TestDino.Tests
{
    [TestFixture]
    public class SignInTest : UnauthenticatedBaseTest
    {
        [SetUp]
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

            // fill the signin form using valid credentials
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_email).SendKeys(credentials["email"]);
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_password).SendKeys(credentials["password"]);

            WaitHelper.ClickWhenClickable(_driver, SignInLocators.Btn_SignIn);

            string toastMessage = WaitHelper.WaitForElement(_driver, SignInLocators.SignIn_ToastMessage).Text;
            Assert.That(toastMessage, Is.EqualTo("Logged in successfully"));

            WaitHelper.WaitUntil(_driver, d => d.Url.Equals(ConfigManager.BaseUrl + "/"));
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

            string toastMessage = WaitHelper.WaitForElement(_driver, SignInLocators.SignIn_ToastMessage).Text;
            Assert.That(toastMessage, Is.EqualTo("Invalid credentials"));
        }
    }

    [TestFixture]
    public class LogOutTest : AuthenticatedBaseTest
    {
        [Test]
        public void LogOut_AfterSignIn()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/account");

            // username displayed in the UI (firstName + lastName)
            string username = WaitHelper.WaitForElement(_driver, By.CssSelector("h2[data-testid= 'user-profile-name']")).Text;
            // expected username from JSON file
            string firstname = JsonHelper.GetTestData<string>("Credentials.json", "ValidCredentials.firstName");
            string lastname = JsonHelper.GetTestData<string>("Credentials.json", "ValidCredentials.lastName");
            // assert that the displayed username matches the expected username
            Assert.That(username, Is.EqualTo($"{firstname} {lastname}"));

            WaitHelper.ClickWhenClickable(_driver, By.CssSelector("div[div = 'menu-item'] p"));

            //assert toast message is displayed after successful signin
            string toastMessage = WaitHelper.WaitForElement(_driver, SignInLocators.SignIn_ToastMessage).Text;
            Assert.That(toastMessage, Is.EqualTo("Logged out successfully"));

            WaitHelper.WaitUntil(_driver, d => d.Url.Equals(ConfigManager.BaseUrl + "/login"));
        }
    }
}
