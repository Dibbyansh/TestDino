using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDino.Base;
using TestDino.Driver;
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
        
        public void FillSignupForm(string firstName, string lastName, string email, string password)
        {
            WaitHelper.WaitForElement(_driver, SignUpSignInLocators.Input_firstname).SendKeys(firstName);
            WaitHelper.WaitForElement(_driver, SignUpSignInLocators.Input_lastname).SendKeys(lastName);
            WaitHelper.WaitForElement(_driver, SignUpSignInLocators.Input_email).SendKeys(email);
            WaitHelper.WaitForElement(_driver, SignUpSignInLocators.Input_password).SendKeys(password);
        }

        public static Dictionary<string,string> randomCredentials()
        {
            // generate random firstname and lastname
            string firstName = "Test" + Guid.NewGuid().ToString("N").Substring(0, 3);
            string lastName = "User" + Guid.NewGuid().ToString("N").Substring(0, 3);

            // generate random email
            string email = $"test{Guid.NewGuid().ToString("N").Substring(0, 5)}@example.com";

            // genrate random password
            string password = "TestPassword" + Guid.NewGuid().ToString("N").Substring(0, 5);

            // store these in a dictionary for easy access
            var credentials = new Dictionary<string, string>
            {
                { "firstName", firstName },
                { "lastName", lastName },
                { "email", email },
                { "password", password }
            };

            JsonHelper.SaveCredentials("Credentials.json", credentials);
            return credentials;
        }
    }
}
