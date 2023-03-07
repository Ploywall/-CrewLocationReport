using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Chrome;

namespace WRMAutotests.Utility
{
    public class BaseInformation
    {
        private IWebDriver driver;
        private AndroidDriver<AndroidElement> androidDriver;
        private bool makeScreenshootForEveryStep = false;

        public BaseInformation(AndroidDriver<AndroidElement> driver)
        {
            androidDriver = driver;
        }

        public BaseInformation(AndroidDriver<AndroidElement> driver, bool makeScreenshootForEveryStep)
        {
            androidDriver = driver;
            this.makeScreenshootForEveryStep = makeScreenshootForEveryStep;
        }


        public BaseInformation(IWebDriver driver)
        {
            this.driver = driver;
        }

        public BaseInformation(IWebDriver driver, bool makeScreenshootForEveryStep)
        {
            this.driver = driver;
            this.makeScreenshootForEveryStep = makeScreenshootForEveryStep;
        }

        public IWebDriver GetDriver()
        {
            return driver;
        }

        public AndroidDriver<AndroidElement> GetAndroidDriver()
        {
            return androidDriver;
        }

        public bool makeScreenshootEveryStep()
        {
            return makeScreenshootForEveryStep;
        }

        public string GetSessionId()
        {
            if (driver != null)
            {
                return ((ChromeDriver)driver).SessionId.ToString().Substring(0, 6);
            }
            else if (androidDriver != null)
            {
                return androidDriver.SessionId.ToString().Substring(0, 6);
            }
            else
            {
                return "ERROR: Unexpected Driver";
            }

        }

    }
}
