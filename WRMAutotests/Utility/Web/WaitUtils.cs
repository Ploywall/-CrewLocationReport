using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace WRMAutotests.Utility.Web
{
    public class WaitUtils
    {

        private IWebDriver driver;
        private WebDriverWait wait;
        private int DEFAULT_EXPLICITY_WAIT_SECONDS = 120;

        public WaitUtils(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_EXPLICITY_WAIT_SECONDS));
        }

        public void WaitForElementPresentByLocator(By locator)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
        }

        public void WaitForElementInvisible(By locator, int secondsForWait = 120)
        {
            var defaultWait = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(secondsForWait);
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = defaultWait;
            }
        }

        public IWebElement WaitForElementAtrivuteEqual(IWebElement webElement, String attributeName, String attributeValue)
        {
            int sizeOfWaitInSeconds = 1;
            for (int i = 0; i < (int)DEFAULT_EXPLICITY_WAIT_SECONDS / sizeOfWaitInSeconds; i++)
            {
                if (webElement.GetAttribute(attributeName).Equals(attributeValue))
                {
                    return webElement;
                }
                else
                {
                    Thread.Sleep(sizeOfWaitInSeconds * 1000);
                }
            }
            throw new AssertionException("For target Web element attribute " + attributeName + " equeal " + webElement.GetAttribute(attributeName) + ", instead expected value: " + attributeValue);
        }

        public IWebElement WaitForElementClicable(IWebElement webElement)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(webElement));
            return webElement;
        }

        public void waitForElementAbsent(IWebElement element)
        {
            for (int i = 0; i < 50; i++)
            {
                try
                {
                    if (element.Displayed)
                    {
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(2000);
                    return;
                }
            }
            throw new AssertionException("Element still visible");
        }

        public void WaitForAllListElementsPresent(IList<IWebElement> elements)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(new ReadOnlyCollection<IWebElement>(elements)));
        }

        public void WaitForAllElementsInvisible(By locatorOfCollection, int waitInSeconds = 120)
        {
            int numberOFIterations = 20;
            for (int i = 0; i < numberOFIterations; i++)
            {
                bool isFoundDisplayedElement = false;
                IList<IWebElement> loadingPanels;
                try
                {
                    loadingPanels = driver.FindElements(locatorOfCollection);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(((int)waitInSeconds / numberOFIterations) * 1000);
                    continue;
                }

                //if we cant find loading panel elements, lets wait a little bit and try again
                if (i == 0)
                {
                    if (loadingPanels.Count == 0)
                    {
                        Thread.Sleep(5000);
                        continue;
                    }
                }


                foreach (IWebElement loadingPanel in loadingPanels)
                {
                    bool elementDisplayed = false;
                    try
                    {
                        elementDisplayed = loadingPanel.Displayed;
                    }
                    catch (OpenQA.Selenium.StaleElementReferenceException ex)
                    {
                        Thread.Sleep(3000);
                        break;
                    }

                    if (elementDisplayed)
                    {
                        Thread.Sleep(((int)waitInSeconds / numberOFIterations) * 1000);
                        isFoundDisplayedElement = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!isFoundDisplayedElement)
                {
                    return;
                }
            }
            throw new AssertionException("Loading panel still present after waiting: " + waitInSeconds + " seconds");

        }

        public void WaitForLoadingPanelAbsent(int timeForWait = 120)
        {
            Thread.Sleep(4000);
            WaitForAllElementsInvisible(By.CssSelector(".dxgvLoadingPanel_DevEx, .dxpnlLoadingDivWithContent_DevEx "), timeForWait);
            Thread.Sleep(4000);
        }

    }


}
