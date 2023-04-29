using AngleSharp.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_nunit_sandbox
{
    [TestFixture]
    public class Tests
    {
        public IWebDriver driver;
        public WebDriverWait wait;
        public IJavaScriptExecutor jse;
        public string baseUrl = "https://ecommerce-playground.lambdatest.io/";

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            jse = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl(baseUrl);
            WaitForPageLoad();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

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

            Assert.That(pageTitle, Is.EqualTo("Your Store"), "Page title should be 'Your Store'");
        }

        [Test, Description("Verify search input is displayed")]
        public void VerifySearchBarIsDisplayed()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            Assert.True(searchBar.Displayed);
        }

        [Test, Description("Verify search autocomplete suggestions are displayed")]
        public void VerifySearchAutocompleteIsDisplayed()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            string searchTerm = "phone";

            searchBar.SendKeys(searchTerm);

            IWebElement autocompleteContainer = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div#entry_217822 div#search div.dropdown ul[class='dropdown-menu autocomplete w-100']")));

            Assert.True(autocompleteContainer.Displayed, "Autocomplete suggestions should be displayed if any matches for valid search term");

            List<IWebElement> suggestions = autocompleteContainer.FindElements(By.CssSelector("li h4")).ToList();

            foreach(var item in suggestions)
            {
                Assert.True(item.Text.ToLower().Contains(searchTerm), "Autocomplete suggestions should contain search term");
            }
        }

        [Test, Description("Verify price within tolerance example")]
        public void VerifyPriceWithinTolerance()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            searchBar.SendKeys("iphone");
            searchBar.SendKeys(Keys.Enter);

            List<IWebElement> searchResultPriceEles = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("div.product-thumb span.price-new"))).ToList();

            foreach(var result in searchResultPriceEles)
            {
                //old syntax for equals within tolerance, can use Is.InRange instead
                Assert.AreEqual(123, Double.Parse(result.Text.Replace("$", "")), .5);
            }
        }

        [Test, Description("User should be able to filter items by price")]
        public void FilterItemsByPrice()
        {
            IWebElement searchBar = driver.FindElement(By.XPath("//div[@id='entry_217822']//input[@name='search']"));

            searchBar.SendKeys(Keys.Enter);

            List<IWebElement> searchResultEles = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("div.product-thumb"))).ToList();

            Assert.That(searchResultEles.Count, Is.EqualTo(15));

            IWebElement minPriceField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[id='mz-filter-panel-0-0'] input[placeholder='Minimum Price']")));

            minPriceField.Clear();
            minPriceField.SendKeys("300");
            minPriceField.SendKeys(Keys.Enter);

            IWebElement maxPriceField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[id='mz-filter-panel-0-0'] input[placeholder='Maximum Price']")));

            maxPriceField.Clear();
            maxPriceField.SendKeys("400");
            maxPriceField.SendKeys(Keys.Enter);

            Pause();

            List<IWebElement> filteredResultEles = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("div.product-thumb"))).ToList();

            Assert.That(filteredResultEles.Count, Is.AtMost(15), "Filtered results should contain at most 15 items");

            foreach(var result in filteredResultEles)
            {
                Assert.That(Double.Parse(result.FindElement(By.CssSelector("span.price-new")).Text.Replace("$", "")), Is.InRange(300, 400));
            }
        }

        public bool IsPageLoadComplete()
        {   
            return jse.ExecuteScript("return document.readyState").Equals("complete");
        }

        public void WaitForPageLoad()
        {
            while (!IsPageLoadComplete())
            {
                IsPageLoadComplete();
            }

            Console.WriteLine("Page load complete");
        }

        public void Pause()
        {
            Thread.Sleep(2500);
        }

        public void Pause(int millis)
        {
            Thread.Sleep(millis);
        }
    }
}