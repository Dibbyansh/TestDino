using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V143.HeapProfiler;
using System.Security.Cryptography.X509Certificates;
using TestDino.Base;
using TestDino.Locators;
using TestDino.Pages.AllProducts;
using TestDino.Utilities;

namespace TestDino.Tests.AllProducts
{
    [TestFixture]
    public class ProductTest : AuthenticatedBaseTest
    {
        private ProductPage _productPage;

        [SetUp]
        public void Setup()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl + "/products");
            WaitHelper.WaitForPageLoad(_driver);
            _productPage = new ProductPage(_driver);
        }

        [Test]
        public void VerifyPageTitle()
        {
            string actualTitle = WaitHelper.WaitForElement(_driver, AllProductsLocators.PageTitleLocator).Text;
            Assert.That(actualTitle, Is.EqualTo("All Products"), "Page title does not match expected value.");
        }

        [Test]
        public void VerifyGridView_CardContent()
        {
            _productPage.SwitchToGridView();

            var issues = _productPage.GetCardIssues(AllProductsLocators.GridContainerLocator);

            Assert.That(issues, Is.Empty, $"Grid view — {issues.Count} card(s) with issues:\n{string.Join("\n", issues)}");
        }

        [Test]
        public void VerifyListView_CardContent()
        {
            _productPage.SwitchToListView();

            var issues = _productPage.GetCardIssues(AllProductsLocators.ListContainerLocator);

            Assert.That(issues, Is.Empty, $"List view — {issues.Count} card(s) with issues:\n{string.Join("\n", issues)}");
        }

        [Test]
        public void VerifyViewSwitch_SameCardCount()
        {
            _productPage.SwitchToGridView();
            int gridCount = _productPage.GetCardCount(AllProductsLocators.GridContainerLocator);

            _productPage.SwitchToListView();
            int listCount = _productPage.GetCardCount(AllProductsLocators.ListContainerLocator);

            Assert.That(listCount, Is.EqualTo(gridCount), $"Card count mismatch — grid: {gridCount}, list: {listCount}.");
        }

        [Test]
        public void Search_RandomValidProduct()
        {
            // Pick a random product from the list each run
            var productlist = JsonHelper.GetTestData<List<string>>("AllProducts.json", "Products");

            // pick random product from the list
            string product = productlist[Random.Shared.Next(productlist.Count)];

            _productPage.Search(product);

            var container = WaitHelper.WaitForElement(_driver, AllProductsLocators.GridContainerLocator);
            List<string> visibleCardNames = container.FindElements(AllProductsLocators.CardHeaderLocator).Select(h => h.Text.Trim()).ToList();

            Assert.Multiple(() =>
            {
                // to check at least one product card is visible after search
                Assert.That(visibleCardNames, Is.Not.Empty,
                    $"No products were returned for search term: \"{product}\"");

                // to check the visible product cards contain the search term
                Assert.That(visibleCardNames, Has.Some.Contains(product).IgnoreCase,
                    $"Search results did not contain \"{product}\".\nFound: {string.Join(", ", visibleCardNames)}");
            });
        }

        [Test]
        public void Search_RandomInvalidProduct()
        {
            // Pick a random invalid term each run
            var productlist = JsonHelper.GetTestData<List<string>>("AllProducts.json", "InvalidSearchTerms");

            // pick random product from the list
            string invalidTerm = productlist[Random.Shared.Next(productlist.Count)];

            TestContext.WriteLine($"Searching for invalid term: \"{invalidTerm}\"");

            _productPage.Search(invalidTerm);

            Assert.Multiple(() =>
            {
                // to check invalid search, did not find any product cards
                Assert.That(_driver.FindElements(AllProductsLocators.NoProductsMessageLocator).Any(e => e.Displayed), Is.True,
                    $"Expected 'no results' message for \"{invalidTerm}\" but it was not shown.");

                // to check the message text is correct
                Assert.That(WaitHelper.WaitForElement(_driver, AllProductsLocators.NoProductsMessageLocator).Text, Does.Contain("No products found"),
                    "No results message text did not match expected value.");
            });
        }

        [Test]
        public void NavigateToProductPage()
        {
            // Read all card hrefs before clicking (href = /product/rode-nt1-a)
            var cardHrefs = _productPage.GetCardHrefs(AllProductsLocators.GridContainerLocator);
            int randomIndex = Random.Shared.Next(cardHrefs.Count);
            string expectedUrl = cardHrefs[randomIndex];

            TestContext.WriteLine($"Clicking card at index {randomIndex}, expected URL: {expectedUrl}");

            // Click the card using its index
            _productPage.ClickCardAt(AllProductsLocators.GridContainerLocator, randomIndex);
            WaitHelper.WaitForPageLoad(_driver);

            // Assert the current URL matches the href we read from the card
            WaitHelper.WaitUntil(_driver, d => d.Url.Contains("/product/"));
            Assert.That(_driver.Url, Is.EqualTo(expectedUrl),
                $"Expected to navigate to \"{expectedUrl}\" but landed on \"{_driver.Url}\".");
        }


    }
}