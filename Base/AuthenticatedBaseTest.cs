using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDino.Driver;
using TestDino.Utilities;

namespace TestDino.Base
{
    public class AuthenticatedBaseTest
    {
        protected static IWebDriver _driver;  // Static = shared across all tests

        [OneTimeSetUp]  // Runs ONCE - login happens once
        public void OneTimeSetup()
        {
         
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
