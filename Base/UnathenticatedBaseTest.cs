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
    public class UnathenticatedBaseTest
    {
        protected IWebDriver _driver;

        [SetUp]  // Each test gets fresh state - REQUIRED for login tests
        public void Setup()
        {
            _driver = DriverFactory.InitDriver();
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
