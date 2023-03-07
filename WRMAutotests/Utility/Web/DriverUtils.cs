using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WRMAutotests.Utility.Web
{
    public class DriverUtils
    {

        private TimeSpan defaultImplicityWait = TimeSpan.FromMinutes(4);
        private TimeSpan defaultDriverWait = TimeSpan.FromMinutes(5);


        public IWebDriver GenerateDefaultWebDriver(Boolean isHeadlessModeEnabled)
        {
            var options = new ChromeOptions();

            //add default options that need for every mode
            options.AddArgument("no-sandbox");
            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;

            //add addtional settings that need only if we use headless mode
            if(isHeadlessModeEnabled)
            {
                options.AddArgument("headless");
                options.AddAdditionalCapability("resolution", "1920x1080", true);
                options.AddArgument("--window-size=1920,1080");
            }

            IWebDriver driver = CreateChromeDriver(options);
            AddDefaultSettingsForDriver(driver);
            return driver;
        }



        private void AddDefaultSettingsForDriver(IWebDriver driver)
        {
            driver.Manage().Window
                .Maximize();
            driver.Manage()
                .Timeouts()
                .ImplicitWait = defaultImplicityWait;
        }

        private IWebDriver CreateChromeDriver(ChromeOptions options = null)
        {
            if(options == null)
            {
                ChromeOptions clearOptions = new ChromeOptions();
                return new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, defaultDriverWait);
            } else
            {
                return new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, defaultDriverWait);
            }
                
        }

        static public void CloseDriver(IWebDriver driver)
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }


    }
}
