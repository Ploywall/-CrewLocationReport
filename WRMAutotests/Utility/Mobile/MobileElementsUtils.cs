using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRMAutotests.Utility.Mobile
{
    public class MobileElementsWaitUtils
    {
        private AndroidDriver<AndroidElement> driver;
        private int defaultImplicityWaitTime;
        public MobileElementsWaitUtils(AndroidDriver<AndroidElement> driver, int defaultImplicityWaitTime)
        {
            this.driver = driver;
            this.defaultImplicityWaitTime = defaultImplicityWaitTime;
        }

        public bool WaitForElementDisapper(By element)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            try
            {
                for (int i = 0; i < 30; i++)
                {
                    try
                    {
                        if (driver.FindElement(element).Displayed)
                            Thread.Sleep(2000);
                    }
                    catch (NoSuchElementException)
                    {
                        break;
                    }
                }
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(defaultImplicityWaitTime);
                return true;
            }
            catch (Exception e)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(defaultImplicityWaitTime);
                return false;
            }
            finally{
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(defaultImplicityWaitTime);
            }
        }

    }
}
