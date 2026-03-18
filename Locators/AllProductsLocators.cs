using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDino.Locators
{
    public class AllProductsLocators
    {
        public static By PageTitleLocator = By.CssSelector("h1[data-testid='all-products-title']");

        public static By GridViewBtnLocator = By.CssSelector("button[data-testid='all-products-view-switcher-grid']");
        public static By ListViewBtnLocator = By.CssSelector("button[data-testid='all-products-view-switcher-list']");

        public static By GridContainerLocator = By.CssSelector("div.grid.grid-cols-1:nth-child(3)");
        public static By ListContainerLocator = By.CssSelector("div.flex.flex-col.gap-4:nth-child(3)");

        public static By CardLocator = By.CssSelector(":scope > a");
        public static By CardImageLocator = By.CssSelector("a.relative img.object-contain");
        public static By CardHeaderLocator = By.CssSelector("h2[data-testid='all-products-header']");
        public static By CardPriceLocator = By.CssSelector("p[data-testid='all-products-price']");

        public static By SearchInputLocator = By.CssSelector("input[data-testid='all-products-search-input']");
        public static By NoProductsMessageLocator = By.CssSelector("h3[data-testid='all-products-no-products-found-title']");
    }
}
