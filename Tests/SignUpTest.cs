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
        private SignupPage _signupPage;
       
        [SetUp]
        public void Setup()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/signup");
            WaitHelper.WaitForPageLoad(_driver);
            _signupPage = new SignupPage(_driver);
        }

        [Test]
        public void Signup_NewUser()
        {
            string title = WaitHelper.WaitForElement(_driver, SignUpLocators.Title_CreateAccount).Text;
            Assert.That(title, Is.EqualTo("Create Account"));

            //new user credentials will be generated and saved in the NewUserCreated section of the credentials.json file for future reference
            var credentials = SignupPage.GenerateRandomCredentials();

            _signupPage.FillSignupForm(
                credentials["firstName"],
                credentials["lastName"],
                credentials["email"],
                credentials["password"]
            );

            _signupPage.ClickCreateAccountBtn();

            _signupPage.VerifyToastMessage("Account created successfully");

            WaitHelper.WaitUntil(_driver, d => d.Url.Contains("/login"));      
        }

        [Test]
        public void Signup_UserAlreadyExists()
        {
            string title = WaitHelper.WaitForElement(_driver, SignUpLocators.Title_CreateAccount).Text;
            Assert.That(title, Is.EqualTo("Create Account"));

            // read the last created user credentials from the NewUserCreated section of the credentials.json file
            var allnewUsers = JsonHelper.GetTestData<List<Dictionary<string, string>>>("Credentials.json", "NewUserCreated");
            var credentials = allnewUsers.Last();

            _signupPage.FillSignupForm(
                credentials["firstName"], 
                credentials["lastName"], 
                credentials["email"], 
                credentials["password"]
            );

            _signupPage.ClickCreateAccountBtn();

            _signupPage.VerifyToastMessage("User already exists");
        }
    }
}
