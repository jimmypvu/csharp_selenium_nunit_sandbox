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
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

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

            Assert.True(pageUrl.Equals(baseUrl));
        }

        [Test, Description("Verify page title is correct")]
        public void VerifyPageTitle() 
        {
            string pageTitle = driver.Title;

            Console.WriteLine(pageTitle);

            Assert.True(pageTitle.Equals("Your Store"));
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

            Assert.True(autocompleteContainer.Displayed);

            List<IWebElement> suggestions = autocompleteContainer.FindElements(By.CssSelector("li h4")).ToList();

            foreach(var item in suggestions)
            {
                Assert.True(item.Text.ToLower().Contains(searchTerm));
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