using OpenQA.Selenium;
using TestDino.Locators;
using TestDino.Utilities;

namespace TestDino.Pages.AllProducts
{
    /// <summary>Holds all issues found on a single product card.</summary>
    public record CardIssue(int CardNumber, string CardName, bool ImageBroken, bool HeaderMissing, bool PriceMissing)
    {
        public bool HasIssues => ImageBroken || HeaderMissing || PriceMissing;

        public override string ToString()
        {
            var issues = new List<string>();
            if (ImageBroken) issues.Add("broken image");
            if (HeaderMissing) issues.Add("missing header");
            if (PriceMissing) issues.Add("missing price");
            return $"Card {CardNumber} \"{CardName}\": {string.Join(", ", issues)}";
        }
    }

    public class ProductPage
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _js;

        public ProductPage(IWebDriver driver)
        {
            _driver = driver;
            _js = (IJavaScriptExecutor)driver;
        }

        public void SwitchToGridView() => WaitHelper.ClickWhenClickable(_driver, AllProductsLocators.GridViewBtnLocator);
        public void SwitchToListView() => WaitHelper.ClickWhenClickable(_driver, AllProductsLocators.ListViewBtnLocator);

        /// <summary>
        /// Clears the search box, types the query, then waits for results to settle —
        /// either cards or the no-results message must appear.
        /// </summary>
        public void Search(string productname)
        {
            var input = WaitHelper.WaitForElement(_driver, AllProductsLocators.SearchInputLocator);
            input.Clear();
            input.SendKeys(productname);

            WaitHelper.WaitUntil(_driver, d =>
                d.FindElements(AllProductsLocators.CardHeaderLocator).Count > 0 ||
                d.FindElements(AllProductsLocators.NoProductsMessageLocator).Count > 0);
        }
        /// <summary>
        /// Returns the href values of all card anchor tags in the given container.
        /// Each href is the relative product detail URL e.g. /product/rode-nt1-a
        /// </summary>
        public List<string> GetCardHrefs(By containerLocator)
        {
            var container = WaitHelper.WaitForElement(_driver, containerLocator);
            return container
                .FindElements(AllProductsLocators.CardLocator)
                .Select(a => a.GetAttribute("href") ?? string.Empty)
                .Where(href => !string.IsNullOrWhiteSpace(href))
                .ToList();
        }

        /// <summary>Clicks the card at the given 0-based index in the given container.</summary>
        public void ClickCardAt(By containerLocator, int index)
        {
            var container = WaitHelper.WaitForElement(_driver, containerLocator);
            var cards = container.FindElements(AllProductsLocators.CardLocator);

            if (index >= cards.Count)
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Index {index} is out of range. Only {cards.Count} card(s) found.");

            var js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", cards[index]);
            cards[index].Click();
        }

        /// <summary>Returns names of all visible product cards in the given container.</summary>
        public List<string> GetVisibleCardNames(By containerLocator)
        {
            var container = WaitHelper.WaitForElement(_driver, containerLocator);
            return container
                .FindElements(AllProductsLocators.CardHeaderLocator)
                .Select(h => h.Text.Trim())
                .ToList();
        }

        public List<CardIssue> GetCardIssues(By containerLocator)
        {
            var container = WaitHelper.WaitForElement(_driver, containerLocator);
            var cards = container.FindElements(AllProductsLocators.CardLocator);

            return cards
                .Select((card, index) =>
                {
                    var headers = card.FindElements(AllProductsLocators.CardHeaderLocator);
                    bool headerMissing = headers.Count == 0 || string.IsNullOrWhiteSpace(headers[0].Text);
                    string cardName = headerMissing ? $"Unknown (card {index + 1})" : headers[0].Text;

                    var imgs = card.FindElements(AllProductsLocators.CardImageLocator);
                    bool imgBroken = imgs.Count == 0 ||
                                     Convert.ToInt32(_js.ExecuteScript("return arguments[0].naturalWidth;", imgs[0])) == 0;

                    var prices = card.FindElements(AllProductsLocators.CardPriceLocator);
                    bool priceMissing = prices.Count == 0 || string.IsNullOrWhiteSpace(prices[0].Text);

                    return new CardIssue(index + 1, cardName, imgBroken, headerMissing, priceMissing);
                })
                .Where(c => c.HasIssues)
                .ToList();
        }

        public int GetCardCount(By containerLocator)
        {
            var container = WaitHelper.WaitForElement(_driver, containerLocator);
            return container.FindElements(AllProductsLocators.CardLocator).Count;
        }
    }
}