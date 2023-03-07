using OpenQA.Selenium;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Mobile.Android
{
    public class WorkspaceLoginScreen : BaseScreen
    {

        //react
        private By emailFieldLocator = By.XPath("//android.widget.EditText[@class='android.widget.EditText' and @password = 'false']");
        private By passwordFieldLocator = By.XPath("//android.widget.EditText[@class='android.widget.EditText' and @password = 'true']");
        private By loginButton = By.XPath("//*[./*[@text='LOGIN']]");

        public WorkspaceLoginScreen(BaseInformation baseInformation) : base(baseInformation, new ReportUtils(baseInformation, "Workspace login", "page"))
        {
        }

        public void EnterEmail(String email)
        {
            GetReportUtils().EnterValueToField("Email", email);
            GetAndroidElement(emailFieldLocator).SendKeys(email);
        }

        public void EnterPassword(String password)
        {
            GetReportUtils().EnterValueToField("Password", password);
            GetAndroidElement(passwordFieldLocator).SendKeys(password);
        }

        public TurnOnYourCurrentLocationScreen ClickLoginButton()
        {
            GetReportUtils().ClickButton("Login");
            GetBaseInformation().GetAndroidDriver().HideKeyboard();
            GetAndroidElement(loginButton).Click();
            WaitThatLoadinCircleAbsent();
            Thread.Sleep(5000);
            return new TurnOnYourCurrentLocationScreen(GetBaseInformation());
        }





    }
}
