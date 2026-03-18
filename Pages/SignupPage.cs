using OpenQA.Selenium;
using TestDino.Locators;
using TestDino.Utilities;

namespace TestDino.Pages
{
    public class SignupPage
    {
        private readonly IWebDriver _driver;

        public SignupPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public static Dictionary<string, string> GenerateRandomCredentials()
        {
            string firstName = "Test" + Guid.NewGuid().ToString("N")[..3];
            string lastName = "User" + Guid.NewGuid().ToString("N")[..3];
            string email = $"test{Guid.NewGuid().ToString("N")[..5]}@example.com";
            string password = "TestPassword" + Guid.NewGuid().ToString("N")[..5];

            var credentials = new Dictionary<string, string>
            {
                { "firstName", firstName },
                { "lastName",  lastName  },
                { "email",     email     },
                { "password",  password  }
            };

            // Save only into the NewUserCreated section, preserving the rest of the file
            JsonHelper.AppendToSection("Credentials.json", "NewUserCreated", credentials);

            return credentials;
        }

        public void FillSignupForm(string firstName, string lastName, string email, string password)
        {
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_firstname).SendKeys(firstName);
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_lastname).SendKeys(lastName);
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_email).SendKeys(email);
            WaitHelper.WaitForElement(_driver, SignUpLocators.Input_password).SendKeys(password);
        }

        public void ClickCreateAccountBtn() => WaitHelper.ClickWhenClickable(_driver, SignUpLocators.Btn_CreateAccount);

        public void VerifyToastMessage(string expectedMessage)
        {
            string toastMessage = WaitHelper.WaitForElement(_driver, SignUpLocators.AccountCreated_ToastMessage).Text;
            Assert.That(toastMessage, Does.Contain(expectedMessage), "Toast Message was not as expected.");
        }
    }
}