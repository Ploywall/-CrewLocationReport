using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace WRMAutotests.Utility.Mobile
{
    public class MobileDriverUtils
    {
        private TimeSpan defaultComandTimeForDriver = TimeSpan.FromSeconds(180);
        private TimeSpan defaultImplicityWait = TimeSpan.FromSeconds(120);

        public AndroidDriver<AndroidElement> GetAndroidDriver(String Url, String deviceName, String fullPathToApp)
        {
            AppiumOptions desiredCapabilities = new AppiumOptions();
            //Base capabilities
            desiredCapabilities.AddAdditionalCapability("appium:deviceName", deviceName);
            desiredCapabilities.AddAdditionalCapability("platformName", "Android");
            desiredCapabilities.AddAdditionalCapability("app", fullPathToApp);

            //additional capabilities
            desiredCapabilities.AddAdditionalCapability("appium:ensureWebviewsHavePages", true);
            desiredCapabilities.AddAdditionalCapability("appium:nativeWebScreenshot", true);
            desiredCapabilities.AddAdditionalCapability("appium:connectHardwareKeyboard", true);
            desiredCapabilities.AddAdditionalCapability("autoGrantPermissions", "true");

            //create driver and set up implicity wait
            Uri remoteUrl = new Uri(Url);
            AndroidDriver<AndroidElement> driver = new AndroidDriver<AndroidElement>(remoteUrl, desiredCapabilities, defaultComandTimeForDriver);
            driver.Manage()
                .Timeouts()
                .ImplicitWait = defaultImplicityWait;
            return driver;
        }

        public void RemoveAppFromDevice(AndroidDriver<AndroidElement> driver, String nameOfApp)
        {
            driver.RemoveApp(nameOfApp);
        }




    }
}
