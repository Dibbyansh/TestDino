using OpenQA.Selenium;
using TestDino.Driver;

namespace TestDino.Base
{
    [TestFixture]
    public class UnauthenticatedBaseTest
    {
        protected IWebDriver _driver = null!;

        [SetUp]
        public void SetUp()
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