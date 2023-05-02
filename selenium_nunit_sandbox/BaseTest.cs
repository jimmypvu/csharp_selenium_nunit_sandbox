using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumNunitSandbox
{
    [TestFixture, Description("Setup and Teardown hooks")]
    public class BaseTest
    {
        public IWebDriver driver;
        public WebDriverWait wait;
        public DefaultWait<IWebDriver> fluentWait;
        public Actions act;
        public IJavaScriptExecutor jse;
        public string baseUrl = "https://ecommerce-playground.lambdatest.io/";
        public string baseUrl2 = "https://rahulshettyacademy.com/loginpagePractise/";

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());

            string testName = TestContext.CurrentContext.Test.Name;
            TestContext.Progress.WriteLine($"Test: {TestContext.CurrentContext.Test.ClassName} > {testName}");
            TestContext.Progress.WriteLine($"Setup for {testName} test");

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            TestContext.Progress.WriteLine("Driver: " + driver);
            TestContext.Progress.WriteLine("Window size: " + driver.Manage().Window.Size);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Message = "Could not find element";

            //no fluent waits in C# selenium, just explicit WebDriverWait and DefaultWait (wdw inherits from defaultwait), you can simulate fluentWait though DefaultWait<IWebDriver>(driver) this looks just like an explicit wait imo
            fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(10);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element to be searched could not be found";

            act = new Actions(driver);

            jse = (IJavaScriptExecutor)driver;

            _= TestContext.CurrentContext.Test.ClassName.Contains("MiscTests") ? driver.Url = baseUrl : driver.Url = baseUrl2;

            //driver.Navigate().GoToUrl(baseUrl);
            //driver.Url = baseUrl;

            WaitForPageLoad();

            //string pageSource = driver.PageSource;  //gets html for page
            //Console.WriteLine(pageSource);
        }

        [TearDown]
        public void Teardown()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            TestContext.Progress.WriteLine($"Teardown for {testName} test");

            driver.Quit();
        }

        public bool IsPageLoadComplete()
        {   
            return jse.ExecuteScript("return document.readyState").Equals("complete");
        }

        public void WaitForPageLoad()
        {
            bool pageLoadComplete = false;

            do
            {
                pageLoadComplete = IsPageLoadComplete();
            } while (!pageLoadComplete);

            TestContext.Progress.WriteLine("Page load complete");
        }

        public void ScrollIntoView(IWebElement element)
        {
            jse.ExecuteScript("arguments[0].scrollIntoView()", element);
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