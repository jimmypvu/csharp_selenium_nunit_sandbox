using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace SeleniumNunitSandbox
{
    internal class PracticePage1 : BaseTest
    {
        [Test, Description("switch to popup alert")]
        public void SwitchToPopupAlert()
        {
            driver.Url = "https://rahulshettyacademy.com/AutomationPractice/";

            WaitForPageLoad();

            string name = "shy guy";
            driver.FindElement(By.CssSelector("#name")).SendKeys(name);
            driver.FindElement(By.Id("confirmbtn")).Click();
            string alertText = driver.SwitchTo().Alert().Text;
            Console.WriteLine(alertText);

            driver.SwitchTo().Alert().Accept();

            Assert.That(alertText.Contains(name));
            Assert.That(alertText, Is.EqualTo($"Hello {name}, Are you sure you want to confirm?"));
        }

        [Test, Description("autocomplete suggestions")]
        public void DynamicDropdownAutocomplete()
        {
            driver.Url = "https://rahulshettyacademy.com/AutomationPractice/";
            WaitForPageLoad();

            string searchTerm = "unit";
            IWebElement inputField = driver.FindElement(By.CssSelector("input#autocomplete"));
            inputField.SendKeys(searchTerm);

            IWebElement autocompleteContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ui-id-1")));

            IList<IWebElement> suggestionEles = autocompleteContainer.FindElements(By.CssSelector("li.ui-menu-item div"));

            TestContext.Progress.WriteLine($"# of autocomplete suggestions: {suggestionEles.Count}");

            foreach(var item in suggestionEles)
            {
                string suggestionText = item.Text;
                TestContext.Progress.WriteLine(suggestionText);
                Assert.That(suggestionText.ToLower().Contains(searchTerm));
            }

            foreach(var item in suggestionEles)
            {
                if(item.Text.Equals("United States (USA)"))
                {
                    item.Click();
                }
            }

            TestContext.Progress.WriteLine($"input text after clicking autocomplete item: {inputField.GetAttribute("value")}");

            Assert.That(inputField.GetAttribute("value"), Is.EqualTo("United States (USA)"));
        }

        [Test, Description("Mouse over element to reveal hover menu")]
        public void ActionsHoverOnElement()
        {
            driver.Url = "https://rahulshettyacademy.com/AutomationPractice/";
            WaitForPageLoad();

            IWebElement hoverBtn = driver.FindElement(By.CssSelector("button#mousehover"));

            ScrollIntoView(hoverBtn);

            acts.MoveToElement(hoverBtn).Perform();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='mousehover']/following-sibling::div/a[contains(text(),'Top')]"))).Click();

            wait.Until(ExpectedConditions.UrlContains("#top"));

            Assert.That(driver.Url, Is.EqualTo("https://rahulshettyacademy.com/AutomationPractice/#top"));
        }
    }
}
