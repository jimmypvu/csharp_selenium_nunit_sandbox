using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V110.DOM;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SeleniumNunitSandbox
{
    internal class RahulShettyTests : BaseTest
    {
        [Test, Description("Login with invalid credentials, with attribute checks")]
        public void InvalidLogin()
        {
            driver.FindElement(By.Id("username")).SendKeys("rahulshettyacademy");

            IWebElement adminBtn = driver.FindElement(By.XPath("//span[contains(text(),'Admin')]/following-sibling::input"));
            Assert.That(adminBtn.GetAttribute("checked"), Is.EqualTo("true"));

            IWebElement userBtn = driver.FindElement(By.XPath("//span[contains(text(),'User')]/following-sibling::input"));
            Assert.That(userBtn.GetAttribute("checked"), Is.Null);

            userBtn.Click();
            Assert.That(userBtn.GetAttribute("checked"), Is.EqualTo("true"));

            IWebElement modalPopup = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".modal-body")));

            Assert.That(modalPopup.Displayed, Is.True);
            driver.FindElement(By.CssSelector("#okayBtn")).Click();

            Assert.That(userBtn.Selected);

            IWebElement termsCheckbox = driver.FindElement(By.CssSelector("#terms"));
            Assert.That(!termsCheckbox.Selected);
            termsCheckbox.Click();
            Assert.That(termsCheckbox.Selected);

            driver.FindElement(By.Id("password")).SendKeys("badpassword" + Keys.Enter);

            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[class='alert alert-danger col-md-12']")));

            TestContext.Progress.WriteLine(errorMessage.Text);

            Assert.That(errorMessage.Displayed);
            Assert.That(errorMessage.GetAttribute("style").Contains("display: block"), "element attribute style should not be 'display: none;' should be visible");
        }

        [Test, Description("Login with valid credentials")]
        public void ValidLogin()
        {
            driver.FindElement(By.Id("username")).SendKeys("rahulshettyacademy");
            driver.FindElement(By.Id("password")).SendKeys("learning" + Keys.Enter);

            wait.Until(ExpectedConditions.UrlMatches("https://rahulshettyacademy.com/angularpractice/shop"));

            Assert.That(driver.Url, Is.EqualTo("https://rahulshettyacademy.com/angularpractice/shop"));

            Assert.That(wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[class='nav-link btn btn-primary']"))).Displayed);
        }

        [Test, Description("Select option from static dropdown")]
        public void StaticDropdown()
        {
            IWebElement staticDropdown = driver.FindElement(By.CssSelector("select.form-control"));
            SelectElement staticSelect = new SelectElement(staticDropdown);

            IList<IWebElement> options = staticSelect.Options;

            foreach(var option in options)
            {
                staticSelect.SelectByText(option.Text);
                Assert.That(staticSelect.SelectedOption.Text.Equals(option.Text));
            }

            staticSelect.SelectByText("Teacher");
            Assert.That(staticSelect.SelectedOption.Text.Equals("Teacher"));
            staticSelect.SelectByValue("stud");
            Assert.That(staticSelect.SelectedOption.Text.Equals("Student"));
            staticSelect.SelectByIndex(2);
            Assert.That(staticSelect.SelectedOption.Text.Equals("Consultant"));
        }

        [Test, Description("Radio buttons")]
        public void ClickRadioBtn()
        {   
            //if not using ToList() need to assign to an IList<T> variable, IList is interface and List is the concrete implementation of IList; general rule: use the generic IList interface when accepting a value and use the concrete implementation for returns, easier to maintain
            //or use List when you need access to the List specific methods
            //IList is useful for when you need to declare your own custom type

            //List<IWebElement> radioBtns = driver.FindElements(By.CssSelector("input[type='radio']")).ToList();

            IList <IWebElement> radioBtns = driver.FindElements(By.CssSelector("input[type='radio']"));

            //looping through eles and checking for some expected attribute is more robust than using locators with index
            foreach (var btn in radioBtns) 
            {
                if (btn.GetAttribute("value").Equals("user"))
                {   
                    //fluent assertion:
                    btn.GetAttribute("checked").Should().Be(null);
                    //Assert.That(btn.GetAttribute("checked"), Is.Null);
                    btn.Click();
                    Assert.That(btn.GetAttribute("checked"), Is.EqualTo("true"));
                }
            }
        }

        [Test, Description("user should be able to add items to shopping cart")]
        public void AddItemsToCart()
        {
            driver.FindElement(By.Id("username")).SendKeys("rahulshettyacademy");
            driver.FindElement(By.Id("password")).SendKeys("learning" + Keys.Enter);

            string[] productsToAdd = { "iphone X", "Samsung Note 8", "Blackberry" };

            List<IWebElement> productCards = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//div[@class='card h-100']"))).ToList();

            foreach (var item in productCards)
            {
                //when using // in xpath, it searched from the root of the dom, not from the current node and printed the first item name 4 times instead of the 4 children's item names individually, so to fix this use .// to search with that xpath from the current node or use css selector

                string itemTitle = item.FindElement(By.XPath(".//h4[@class='card-title']/a")).Text;
                //string itemName = item.FindElement(By.CssSelector("h4.card-title")).Text;

                if (productsToAdd.Contains(itemTitle))
                {
                    //item.FindElement(By.XPath("./div[@class='card-footer']/button")).Click();
                    item.FindElement(By.CssSelector("div.card-footer button")).Click();
                }
            }

            IWebElement checkoutBtn = driver.FindElement(By.CssSelector("[class='nav-link btn btn-primary']"));

            string itemCountBadgeText = Regex.Replace(checkoutBtn.Text, @"[^0-9]", "");
            int itemCount = int.Parse(itemCountBadgeText);
            
            Assert.That(itemCount, Is.EqualTo(productsToAdd.Length));

            checkoutBtn.Click();

            IList<IWebElement> cartItems = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("h4.media-heading")));

            foreach (var item in cartItems)
            {
                Assert.That(productsToAdd.Contains(item.Text));
            }
        }
    }
}
