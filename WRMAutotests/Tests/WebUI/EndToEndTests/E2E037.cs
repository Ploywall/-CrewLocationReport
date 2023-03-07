using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Utility.pages;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E037 : BaseWebEndToEndTest
    {

        private static int numberOfRowForCurrentTestCase = 39;
        private static String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);
        //read values from Continue parameter column
        private CrewLocationReport.TabSplit tabDiscipline = CrewLocationReport.GetTabByNameFoTabSplit(discipline);
        private static ContinueFormSettings settingsContinueForm = JsonConvert.DeserializeObject<ContinueFormSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet));
    
        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E037_Test()
        {
            String checkCrewSheet = settingsContinueForm.crewSheet;
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 39);
            CrewLocationReport crewLocationReport = manageSecuredWorkforcePage.GetHeaderPanel()
            .OpenReportMenuPanal()
            .ClickCrewLocationReportButton();
            crewLocationReport.ClickTabSplit(tabDiscipline);
            int foundNumberPfRows = crewLocationReport.GetCrewLocationReport().GetNumberOFRowsByCrewSheetFromAllPages(checkCrewSheet);
            crewLocationReport.GetCrewLocationReport()
                .GetAssertionUtils()
                .TrueAssertion("Verify that number of rows with crew sheet: " + checkCrewSheet + " more than 0", foundNumberPfRows > 0);
            crewLocationReport.GetCrewLocationReport()
                .GetAssertionUtils()
                .TrueAssertion("Verify that number of rows with crew sheet: " + checkCrewSheet + " equal to 1", foundNumberPfRows == 1);
            crewLocationReport.GetCrewLocationReport()
                .GetRowsByCrewSheet(settingsContinueForm.crewSheet)[0]
                .ClickPinInMap();
        }

        private class ContinueFormSettings
        {
            public string crewSheet;
            
        }

    }
}

