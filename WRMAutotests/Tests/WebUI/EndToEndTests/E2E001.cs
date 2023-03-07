using RetryOnException;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E001 : BaseWebEndToEndTest
    {
        private static int numberOfRowForCurrentTestCase = 3;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);


        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E001_Test()
        {
            GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 3)
                .GetAssertionUtils()
                .TrueAssertion("Verify that Header panel present", WRMAutotests.PageObjects.Web.Utility.panel.HeaderPanel.IsHeaderPanelPresent(GetDefaultBaseInformation())); ;
        }

    }
}
