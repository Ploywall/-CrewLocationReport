using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using WRMAutotests.Utility;
using WRMAutotests.Utility.Mobile;
using WRMAutotests.Utility.Web;

namespace WRMAutotests.PageObjects.Mobile.Android
{
    public class BaseMobilePageObject
    {
        private BaseInformation baseInformation;
        private ReportUtils reportUtils;
        private By rootLocator = null;
        private AndroidElement rootAndroidElement = null;
        private By loadingCircleRootElement = By.XPath("//android.widget.FrameLayout[@content-desc='Loading Component']/android.widget.ProgressBar");

        public BaseMobilePageObject(BaseInformation baseInformation, ReportUtils reportUtils)
        {
            this.baseInformation = baseInformation;
            this.reportUtils = reportUtils;
        }

        public BaseMobilePageObject(BaseInformation baseInformation, By rootLocator, ReportUtils reportUtils)
        {
            this.baseInformation = baseInformation;
            this.rootLocator = rootLocator;
            this.reportUtils = reportUtils;
        }

        public BaseMobilePageObject(BaseInformation baseInformation, AndroidElement rootElement, ReportUtils reportUtils)
        {
            this.baseInformation = baseInformation;
            this.rootAndroidElement = rootElement;
            this.reportUtils = reportUtils;
        }

        public void WaitThatLoadinCircleAbsent()
        {
            GetMobileElementsUtils().WaitForElementDisapper(loadingCircleRootElement);
        }

        public AndroidElement GetAndroidElement(By locator)
        {
            if (rootLocator != null)
            {
                return (AndroidElement)baseInformation.GetAndroidDriver().FindElement(rootLocator).FindElement(locator);
            }
            else if (rootAndroidElement != null)
            {
                return (AndroidElement)rootAndroidElement.FindElement(locator);
            }
            else
            {
                return baseInformation.GetAndroidDriver().FindElement(locator);
            }
        }

        public IList<AndroidElement> GetAndroidElements(By locator)
        {
            if (rootLocator != null)
            {
                return (IList<AndroidElement>)baseInformation.GetAndroidDriver().FindElement(rootLocator).FindElements(locator);
            }
            else if (rootAndroidElement != null)
            {
                return (IList<AndroidElement>)rootAndroidElement.FindElements(locator);
            }
            else
            {
                return baseInformation.GetAndroidDriver().FindElements(locator);
            }
        }

        public void ScrollAndClickOnElementByText(String text)
        {
            baseInformation.GetAndroidDriver()
                .FindElementByAndroidUIAutomator("new UiScrollable(new UiSelector().scrollable(true).instance(0)).scrollIntoView(new UiSelector().textContains(\"" + text + "\").instance(0))").Click();
        }

        public BaseInformation GetBaseInformation()
        {
            return this.baseInformation;
        }

        public ReportUtils GetReportUtils()
        {
            return reportUtils;
        }

        public WaitUtils GetWaitUtils()
        {
            return new WaitUtils(GetBaseInformation().GetAndroidDriver());
        }

        public MobileElementsWaitUtils GetMobileElementsUtils()
        {
            return new MobileElementsWaitUtils(GetBaseInformation().GetAndroidDriver(), 60);
        }

    }
}
