using OpenQA.Selenium;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Mobile.Android
{
    public class TurnOnYourCurrentLocationScreen : BaseScreen
    {

        private By okButtonLocator = By.XPath("//android.widget.Button[./*[@text='OK']]");
        private By confirmButton = By.XPath("//*[@resource-id='android:id/button1']");

        public TurnOnYourCurrentLocationScreen(BaseInformation baseInformation) : base(baseInformation, new ReportUtils(baseInformation, "Turn On Current location", "screen"))
        {


        }

        public OnTheClockScreen ClickOk()
        {
            GetReportUtils().ClickButton("Ok");
            GetAndroidElement(okButtonLocator).Click();
            WaitThatLoadinCircleAbsent();
            return new OnTheClockScreen(GetBaseInformation());
        }

        public void ClickConfirmButton()
        {
            GetReportUtils().ClickButton("Confirm");
            GetAndroidElement(confirmButton).Click();
        }



    }
}
