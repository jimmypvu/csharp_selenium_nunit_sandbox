using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_nunit_sandbox
{
    [TestFixture, Description("Setup and Teardown hooks")]
    public class BaseTest
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
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            Console.WriteLine(driver.Manage().Window.Size);

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