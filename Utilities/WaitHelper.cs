using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDino.Utilities
{
    public static class WaitHelper
    {
        private const int DefaultTimeoutSeconds = 20;

        public static void WaitForPageLoad(IWebDriver driver, int seconds = DefaultTimeoutSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            wait.Until(d =>
                ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState")!
                .Equals("complete"));
        }

        public static IWebElement WaitForElement(IWebDriver driver, By locator, int seconds = DefaultTimeoutSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException));

            return wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return (element.Displayed && element.Enabled) ? element : null;
            })!;
        }

        public static void WaitUntil(IWebDriver driver, Func<IWebDriver, bool> condition, int seconds = DefaultTimeoutSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException));

            wait.Until(d =>
            {
                return condition(d);
            });
        }

        public static void ClickWhenClickable(IWebDriver driver, By locator, int seconds = DefaultTimeoutSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException)
            );

            var element = wait.Until(d =>
            {
                var el = d.FindElement(locator);
                return (el.Displayed && el.Enabled) ? el : null;
            });

            var jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);

            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {

                jsExecutor.ExecuteScript("arguments[0].click();", element);
            }
        }
    }
}
