using OpenQA.Selenium;
using FluentAssertions;
using SeleniumExtras.WaitHelpers;

namespace SeleniumNunitSandbox
{   
    // : in csharp is java's extends keyword for inheritance
    internal class MiscTests : BaseTest 
    {

        [Test, Description("Verify page URL is correct")]
        public void VerifyPageUrl()
        {
            string pageUrl = driver.Url;

            Console.WriteLine(pageUrl);

            Assert.That(pageUrl, Is.EqualTo(baseUrl), "Page Url should be {0}", baseUrl);
        }

        [Test, Description("Verify page title is correct")]
        public void VerifyPageTitle()
        {
            string pageTitle = driver.Title;

            Console.WriteLine(pageTitle);

            //fluent assertion
            pageTitle.Should().Be("Your Store", "because the page title is 'Your Store'");
                
            //nunit assertion
            //Assert.That(pageTitle, Is.EqualTo("Your Store"), "Page title should be 'Your Store'");
        }

        [Test, Description("Verify search input is displayed")]
        public void VerifySearchBarIsDisplayed()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));


            searchBar.Displayed.Should().BeTrue();
            //Assert.True(searchBar.Displayed);
        }

        [Test, Description("Verify search autocomplete suggestions are displayed")]
        public void VerifySearchAutocompleteIsDisplayed()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            string searchTerm = "phone";
            searchBar.SendKeys(searchTerm);

            IWebElement autocompleteContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#entry_217822 div#search div.dropdown ul[class='dropdown-menu autocomplete w-100']")));

            Assert.True(autocompleteContainer.Displayed, "Autocomplete suggestions should be displayed if any matches for valid search term");

            List<IWebElement> suggestions = autocompleteContainer.FindElements(By.CssSelector("li h4")).ToList();

            foreach (var suggestion in suggestions)
            {
                string suggestionText = suggestion.Text.ToLower();

                //fluent assertion - neat
                suggestionText.Should().Contain(searchTerm, $"Because we searched for {searchTerm}");

                //nunit assertion
                //Assert.True(suggestionText.Contains(searchTerm), "Autocomplete suggestions should contain search term");
            }
        }

        [Test, Description("Verify price within tolerance example")]
        public void VerifyPriceWithinTolerance()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            searchBar.SendKeys("iphone");
            searchBar.SendKeys(Keys.Enter);

            List<IWebElement> searchResultPriceEles = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("div.product-thumb span.price-new"))).ToList();

            foreach (var result in searchResultPriceEles)
            {
                double price = Double.Parse(result.Text.Replace("$", ""));
                //old syntax for equals within tolerance, can use Is.InRange instead
                //Assert.AreEqual(123, price, .5);
                Assert.That(price, Is.InRange(122.5, 123.5));

                //fluent assertion:
                price.Should().BeInRange(122.5, 123.5);
            }
        }

        [Test, Description("User should be able to filter items by price")]
        public void FilterItemsByPrice()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            searchBar.SendKeys(Keys.Enter);

            List<IWebElement> searchResultEles = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("div.product-thumb"))).ToList();

            Assert.That(searchResultEles.Count, Is.EqualTo(15));

            ////div[@id='mz-filter-panel-0-0']//input[contains(@aria-label, 'Minimum')]

            IWebElement minPriceField = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[id='mz-filter-panel-0-0'] input[placeholder='Minimum Price']")));

            minPriceField.Clear();
            minPriceField.SendKeys("300");
            minPriceField.SendKeys(Keys.Enter);

            IWebElement maxPriceField = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[id='mz-filter-panel-0-0'] input[placeholder='Maximum Price']")));

            maxPriceField.Clear();
            maxPriceField.SendKeys("400");
            maxPriceField.SendKeys(Keys.Enter);

            Pause(); //shouldn't sleep often but need to here bc unfiltered eles are already on dom before sending filter params to page, need filtered ele results to load again on dom to get correct results

            List<IWebElement> filteredResultEles = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("div.product-thumb"))).ToList();

            //fluent assertion:
            //filteredResultEles.Count.Should().BeLessThanOrEqualTo(15, "because we filtered results down and 15 items in the max # of results per page");

            Assert.That(filteredResultEles.Count, Is.AtMost(15), "Filtered results should contain at most 15 items");

            foreach (var result in filteredResultEles)
            {
                double price = Double.Parse(result.FindElement(By.CssSelector("span.price-new")).Text.Replace("$", ""));

                //fluent assertion:
                price.Should().BeInRange(300, 400, "because we filtered prices from 300 to 400");

                //nunit assertion:
                //Assert.That(price, Is.InRange(300, 400), "because we filtered prices from 300 to 400");
            }
        }
    }
}
