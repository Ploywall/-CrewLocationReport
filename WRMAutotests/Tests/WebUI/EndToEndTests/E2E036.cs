using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.PageObjects.Web.Contractor.panels;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.Utility;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E036 : BaseWebEndToEndTest
    {
        //read values from Excel
        private static int numberOfRowForCurrentTestCase = 38;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);

        //read values from Continue parameter column
        private ManageSecuredWorkforcePage.Tabs tabDiscipline = ManageSecuredWorkforcePage.GetTabByNameFoTab(discipline);
        private static ContinueFormSettings settingsContinueForm = JsonConvert.DeserializeObject<ContinueFormSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet));

        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E036_Test()
        {
            String checkCrewSheet = settingsContinueForm.CrewSheet;
            String checkCurrentLocation= settingsContinueForm.CurrentLocation;
            String checkCurrentStatus = settingsContinueForm.CurrentStatus;
            String checkClockInTime= settingsContinueForm.ClockInTime;
            String checkTimeOnTheClock = settingsContinueForm.TimeOnTheClock;
            String checkClockOutTime = settingsContinueForm.ClockOutTime;
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 38);
            /*manageSecuredWorkforcePage.GetHeaderPanel()
                .ClickHomeButton();
            manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickManageSecuredWorkforce();
            manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenOperatingCompanyDropdownMenu()
                .ClickOperatingCompanyByName(operatingCompanyName);
            manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenEventDropdownMenu()
                .ClickEventByName(eventName);*/
            manageSecuredWorkforcePage.ClickTab(tabDiscipline);
            int foundNumberPfRows = manageSecuredWorkforcePage.GetSecuredWorkforcesPanel().GetNumberOFRowsByCrewSheetFromAllPages(checkCrewSheet,checkCurrentLocation,checkCurrentStatus,checkClockInTime,checkTimeOnTheClock,checkClockOutTime);
            manageSecuredWorkforcePage.GetSecuredWorkforcesPanel()
                .GetAssertionUtils()
                .TrueAssertion("Verify that number of rows with Data: " + checkCrewSheet +" / "+ checkCurrentLocation +" / "+ checkCurrentStatus +" / "+ checkClockInTime +" / "+ checkTimeOnTheClock +" / "+ checkClockOutTime + " more than 0", foundNumberPfRows > 0);
            manageSecuredWorkforcePage.GetSecuredWorkforcesPanel()
                .GetAssertionUtils()
                .TrueAssertion("Verify that number of rows with Data: " + checkCrewSheet +" / "+ checkCurrentLocation +" / "+ checkCurrentStatus +" / "+ checkClockInTime +" / "+ checkTimeOnTheClock +" / "+ checkClockOutTime + " equal to 1", foundNumberPfRows == 1); 
        }

        private class ContinueFormSettings
        {
            public string CrewSheet;
            public string CurrentLocation;
            public string CurrentStatus;
            public string ClockInTime;
            public string TimeOnTheClock;
            public string ClockOutTime;

        }

    }

}

            