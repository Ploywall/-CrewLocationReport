using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.PageObjects.Web.Contractor.panels;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.Utility;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E011 : BaseWebEndToEndTest
    {
        //read values from Excel
        private static int numberOfRowForCurrentTestCase = 13;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);
        private Boolean IsClearDataAfterTest = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForClearDataForTestCaseSheet);
        private InternalWorkforcePage.Tabs tabDiscipline = InternalWorkforcePage.GetTabByNameFoTab(discipline);

        //read values from Continue parameter column
        private static ContinueFormSettings settingsContinueForm = JsonConvert.DeserializeObject<ContinueFormSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet));
        private String resourcePoolName;
        private String resourcePool;

        [SetUp]
        public void CreateNeedEnteties()
        {
            int estimatedResources = 5;
            int estimatedCrews = 3;
            int crewSize = 4;
            int estimatedBuckets = 6;
            int estimatedDiggers = 6;

            BaseInformation baseInformation = AddNewDriverWithDefaultSettings();
            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(baseInformation, contractorUser);

            //Create resource pool if we dont have resource pool from continue parameter
            InternalWorkforcePage internalWorkforcePage = null;
            if (settingsContinueForm.ResourcePoolName != null && !settingsContinueForm.ResourcePoolName.Equals(""))
            {
                resourcePoolName = settingsContinueForm.ResourcePoolName;
                internalWorkforcePage = mainPage.GetHeaderPanel()
                    .ClickHomeButton()
                    .GetHeaderPanel()
                    .OpenWorkforceMenuPanel()
                    .ClickInternalWorkforceButton()
                    .ClickTab(tabDiscipline);
            }
            else
            {
                resourcePoolName = GetRandomValuesUtilities().GetRandomValue();
                internalWorkforcePage = GetActionsForEndToEndTests().CreateResourcePool(mainPage, tabDiscipline, resourcePoolName, defaultSourceLocation, estimatedResources, estimatedCrews, crewSize, estimatedBuckets, estimatedDiggers, checkIouCheckbox, operatingCompanyName);
            }

            internalWorkforcePage = GetActionsForEndToEndTests().CreateCrewAvailabilityForm(internalWorkforcePage, resourcePoolName, operatingCompanyName);
            RemoveDriver(baseInformation);
            baseInformation = AddNewDriverWithDefaultSettings();
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 13, baseInformation);
            resourcePool = GetActionsForEndToEndTests().CreateProcureCrew(manageSecuredWorkforcePage, defaultSourceLocation, resourcePoolName, "Adam Turner", GetRandomValuesUtilities().GetRandomValue());
            RemoveDriver(baseInformation);
        }


        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E011_Test()
        {
            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(contractorUser);
            ProcurementRequestPage procurementRequestPage = mainPage.GetHeaderPanel()
                .OpenWorkforceMenuPanel()
                .ClickProcurementRequest();
            ProcurementRequestsPanel.Row row = procurementRequestPage.GetProcurementRequestsPanel()
                .GetRowsFromCurrentPageByResourcePool(resourcePool)[0];

            row.GetAssertionUtils().EquialAssertion("Verify that for row with resource pool: " + resourcePool + " . Present status : " + "Waiting for Review", "Waiting for Review", row.GetStatus());

            row.ClickEditButton()
                .ClickAcceptButton()
                .ClickConfirmButton();
            procurementRequestPage.GetHeaderPanel()
                .OpenAccountDropdownMenu()
                .ClickLogoutButton();

            //E2E012
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 14);
            NonIouMarketplacePage nonIouMarketplacePage = manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickNonIouMarketplaceButton();
            nonIouMarketplacePage.GetHeaderPanel().OpenOperatingCompanyDropdownMenu()
                .ClickOperatingCompanyByName(operatingCompanyName);

            nonIouMarketplacePage.GetCrewAvailabilitiesPanel()
                .GetAssertionUtils()
                .TrueAssertion("Verify that for row with resource pool: " + resourcePool + " present Accepted label",
                nonIouMarketplacePage.GetCrewAvailabilitiesPanel()
                .GetRowsByResourcePoolFromAnyPage(resourcePool)[0]
                .IsRequestAcceptedLabelPresent());


            //E2E013
            manageSecuredWorkforcePage = nonIouMarketplacePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickManageSecuredWorkforce();
            manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenEventDropdownMenu()
                .ClickEventByName("— All Include No Event —");
            int foundNumberPfRows = manageSecuredWorkforcePage.GetSecuredWorkforcesPanel().GetNumberOFRowsByResourcePoolFromAllPages(resourcePool);
            manageSecuredWorkforcePage.GetSecuredWorkforcesPanel()
                .GetAssertionUtils()
                .TrueAssertion("Verify that number of rows with resource pool: " + resourcePool + " more than 0", foundNumberPfRows > 0);
            manageSecuredWorkforcePage.GetSecuredWorkforcesPanel()
                .GetAssertionUtils()
                .TrueAssertion("Verify that number of rows with resource pool: " + resourcePool + " equal to 1", foundNumberPfRows == 1);
        }


        [TearDown]
        public void DeleteCreatedResourcePool()
        {
            if (!IsClearDataAfterTest)
            {
                return;
            }
            BaseInformation baseInformation = AddNewDriverWithDefaultSettings();
            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(baseInformation, contractorUser);
            mainPage.GetHeaderPanel()
                .ClickHomeButton()
                .GetHeaderPanel()
                .OpenWorkforceMenuPanel()
                .ClickInternalWorkforceButton()
                .ClickTab(tabDiscipline)
                .GetResourcesPoolPanel()
                .GetResourcePoolRowsByResourcePoolName(resourcePoolName)[0]
                .ClickOperationButton()
                .ClickDeleteButton()
                .ClickConfirmButton();
            Thread.Sleep(15000);
        }

        private class ContinueFormSettings
        {
            public string ResourcePoolName;
        }

    }

}
