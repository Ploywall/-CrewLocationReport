using OpenQA.Selenium;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Mobile.Android
{
    public class SelectWorkspaceScreen : BaseScreen
    {
        private By selectWorkspaceLocator = By.XPath("//*[@class='android.widget.EditText']");
        private By workspaceElementsOnMenu = By.XPath("//*[@resource-id='com.wrmsoftware.stormmanager.reactdev:id/select_dialog_listview']//*");
        private By nextButton = By.ClassName("android.widget.Button");

        public SelectWorkspaceScreen(BaseInformation baseInformation) : base(baseInformation, new ReportUtils(baseInformation, "Select Workspace", "Screen"))
        {
        }

        public void SelectWorkspace(String workspaceName)
        {
            Thread.Sleep(10000);//need for loading first screen
            GetReportUtils().AllureStepWithPageObject("Select Workspace");
            //open menu
            TapSelectWorkspace();

            //find target element
            Thread.Sleep(5000);
            ScrollAndClickOnElementByText(workspaceName);
            WaitThatLoadinCircleAbsent();
        }

        private void TapSelectWorkspace()
        {
            //no need Report here
            GetAndroidElement(selectWorkspaceLocator).Click();
        }

        public WorkspaceLoginScreen ClickNextButton()
        {
            GetReportUtils().ClickButton("Next");
            GetAndroidElement(nextButton).Click();
            WaitThatLoadinCircleAbsent();
            return new WorkspaceLoginScreen(GetBaseInformation());
        }



    }
}
