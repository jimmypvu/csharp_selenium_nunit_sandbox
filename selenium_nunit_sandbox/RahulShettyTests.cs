using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

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
            driver.FindElement(By.CssSelector("#terms")).Click();

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
                Console.WriteLine(option.Text);
                Console.WriteLine(option.GetAttribute("value"));
                //staticSelect.SelectByText(option.Text);
                //Assert.That(staticSelect.SelectedOption.Text.Equals(option.Text));
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
            //if not using ToList() need to assign to an IList<T> variable, IList is useful for when you need to declare your own custom type, IList is interface and List is the concrete implementation of IList; general rule: use the generic IList interface when accepting a value and use the concrete implementation for returns, easier to maintain
            //or use List when you need access to the List specific methods

            //List<IWebElement> radioBtns = driver.FindElements(By.CssSelector("input[type='radio']")).ToList();

            IList<IWebElement> radioBtns = driver.FindElements(By.CssSelector("input[type='radio']"));

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
    }
}
