using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDino.Base;
using TestDino.Driver;
using TestDino.Locators;
using TestDino.Pages;
using TestDino.Utilities;

namespace TestDino.Tests
{
    [TestFixture]
    public class SignUpTest : UnauthenticatedBaseTest
    {
        [SetUp]
        public void Setup()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/signup");
            WaitHelper.WaitForPageLoad(_driver);
        }

        [Test]
        public void Signup_NewUser()
        {
            var signupPage = new SignupPage(_driver);

            string title = WaitHelper.WaitForElement(_driver, SignUpLocators.Title_CreateAccount).Text;
            Assert.That(title, Is.EqualTo("Create Account"));

            var credentials = SignupPage.GenerateRandomCredentials();

            signupPage.FillSignupForm(
                credentials["firstName"],
                credentials["lastName"],
                credentials["email"],
                credentials["password"]
            );

            WaitHelper.ClickWhenClickable(_driver, SignUpLocators.Btn_CreateAccount);

            string toastMessage = WaitHelper.WaitForElement(_driver, SignUpLocators.AccountCreated_ToastMessage).Text;
            Assert.That(toastMessage, Does.Contain("Account created successfully!"), "Toast Message was not as expected.");

            // assert that the user is redirected to the login page after successful signup
            WaitHelper.WaitUntil(_driver, d => d.Url.Contains("/login"));
            
        }

        // user already exists error message test
        [Test]
        public void Signup_UserAlreadyExists()
        {
            var signupPage = new SignupPage(_driver);

            string title = WaitHelper.WaitForElement(_driver, SignUpLocators.Title_CreateAccount).Text;
            Assert.That(title, Is.EqualTo("Create Account"));

            // use the same credentials as the previous test to trigger the user already exists error
            // read from credentials.json file
            var allnewUsers = JsonHelper.GetTestData<List<Dictionary<string, string>>>("Credentials.json", "NewUserCreated");
            var credentials = allnewUsers.Last(); // get the last created user

            signupPage.FillSignupForm(
                credentials["firstName"], 
                credentials["lastName"], 
                credentials["email"], 
                credentials["password"]
            );

            WaitHelper.ClickWhenClickable(_driver, SignUpLocators.Btn_CreateAccount);

            string toastMessage = WaitHelper.WaitForElement(_driver, SignUpLocators.AccountCreated_ToastMessage).Text;
            Assert.That(toastMessage, Does.Contain("User already Exist"), "Toast Message was not as expected.");
        }
    }
}
