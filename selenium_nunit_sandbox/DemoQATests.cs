using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace SeleniumNunitSandbox
{
    internal class DemoQATests : BaseTest
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

            act.MoveToElement(hoverBtn).Perform();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='mousehover']/following-sibling::div/a[contains(text(),'Top')]"))).Click();

            wait.Until(ExpectedConditions.UrlContains("#top"));

            Assert.That(driver.Url, Is.EqualTo("https://rahulshettyacademy.com/AutomationPractice/#top"));
        }

        [Test, Description("drag and drop")]
        public void DragAndDrop()
        {
            driver.Url = "https://demoqa.com/droppable";
            WaitForPageLoad();

            IWebElement draggableEle = driver.FindElement(By.CssSelector("#draggable"));
            IWebElement droppableEle = driver.FindElement(By.CssSelector("div#simpleDropContainer div#droppable"));


            act.DragAndDrop(draggableEle, droppableEle).Perform();

            //acts.ClickAndHold(draggableEle).Release(droppableEle).Perform();

            Assert.That(droppableEle.Text, Is.EqualTo("Dropped!"));
            Assert.That(droppableEle.GetAttribute("class").Contains("ui-state-highlight"));
            Assert.That(droppableEle.GetCssValue("background-color").Contains("rgba(70, 130, 180, 1"));
        }

        [Test, Description("drag and drop 2: unacceptable drag ele")]
        public void DragAndDrop2()
        {
            driver.Url = "https://demoqa.com/droppable";
            WaitForPageLoad();

            IWebElement acceptTab = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a#droppableExample-tab-accept")));
            
            acceptTab.Click();

            IWebElement dragEleGood = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#acceptable")));
            IWebElement dragEleBad = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#notAcceptable")));
            IWebElement dropEle = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#acceptDropContainer div#droppable")));

            act.ClickAndHold(dragEleBad).Release(dropEle).Perform();

            Assert.That(dropEle.GetAttribute("class"), Is.EqualTo("drop-box ui-droppable"));
            Assert.That(dropEle.Text, Is.EqualTo("Drop here"));

            act.DragAndDrop(dragEleGood, dropEle).Perform();

            Assert.That(dropEle.GetAttribute("class").Contains("ui-state-highlight"));
            Assert.That(dropEle.Text, Is.EqualTo("Dropped!"));
        }

        [Test, Description("Switch to new tab")]
        public void SwitchToNewTab()
        {
            driver.Url = "https://demoqa.com/browser-windows";

            string mainTab = driver.CurrentWindowHandle;

            IWebElement newTabBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#tabButton")));

            newTabBtn.Click();

            IList<String> handles = driver.WindowHandles;
            foreach(String handle in handles) 
            {
                if (!handle.Equals(mainTab))
                {
                    driver.SwitchTo().Window(handle);
                }
            }

            TestContext.Progress.WriteLine(driver.Url);
            Assert.That(driver.Url, Is.EqualTo("https://demoqa.com/sample"));
            driver.Close();

            driver.SwitchTo().Window(mainTab);
            TestContext.Progress.WriteLine(driver.Url);
        }

        [Test, Description("Switch to new window")]
        public void SwitchToNewWindow()
        {
            driver.Url = "https://demoqa.com/browser-windows";

            string mainWindow = driver.CurrentWindowHandle;

            IWebElement newWindowBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#windowButton")));

            newWindowBtn.Click();

            IList<String> handles = driver.WindowHandles;
            foreach (String handle in handles)
            {
                if (!handle.Equals(mainWindow))
                {
                    driver.SwitchTo().Window(handle);
                }
            }

            Console.WriteLine(driver.Url);
            Assert.That(driver.Url, Is.EqualTo("https://demoqa.com/sample"));
            driver.Close();

            driver.SwitchTo().Window(mainWindow);
            Console.WriteLine(driver.Url);
        }

    }
}
