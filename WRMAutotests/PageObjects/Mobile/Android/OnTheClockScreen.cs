using OpenQA.Selenium;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Mobile.Android
{
    public class OnTheClockScreen : BaseScreen
    {

        private By clockInButtonLocator = By.XPath("//android.widget.Button[./android.widget.TextView[@text='CLOCK-IN']]");
        public OnTheClockScreen(BaseInformation baseInformation) : base(baseInformation, new ReportUtils(baseInformation, "On The Clock", "Screen"))
        {
        }

        public void ClickClockInButton()
        {
            GetReportUtils().ClickButton("Clock-In");
            GetAndroidElement(clockInButtonLocator).Click();
            WaitThatLoadinCircleAbsent();
            Thread.Sleep(2000);
        }

        public void SelectWorkStatus(String workStatus)
        {
            GetReportUtils().AllureStepWithPageObject("Select Work Status");
            ScrollAndClickOnElementByText(workStatus);
            WaitThatLoadinCircleAbsent();
        }


    }
    

}
