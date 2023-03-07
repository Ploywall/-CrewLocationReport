using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.Utility;
using static WRMAutotests.PageObjects.Web.Contractor.panels.ResourcesPoolPanel;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E006 : BaseWebEndToEndTest
    {

        private static int numberOfRowForCurrentTestCase = 8;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);
        private InternalWorkforcePage.Tabs disciplineTab = InternalWorkforcePage.GetTabByNameFoTab(discipline);

        private static InputFormSettings settingsInputForm = JsonConvert.DeserializeObject<InputFormSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForInputFormForTestCaseSheet));
        private String resourcePoolName = settingsInputForm.ResourcePoolName != null && !settingsInputForm.ResourcePoolName.Equals("") ? settingsInputForm.ResourcePoolName : GetRandomValuesUtilities().GetRandomValue();
        private Boolean IsClearDataAfterTest = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForClearDataForTestCaseSheet);


        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E006_Test()
        {
            String sourceLocation = settingsInputForm.SourceLocation;
            int estimatedResources = int.Parse(settingsInputForm.EstimatedResources);
            int estimatedCrews = int.Parse(settingsInputForm.EstimatedCrews);
            int crewSize = int.Parse(settingsInputForm.CrewSize);
            int estimatedBuckets = int.Parse(settingsInputForm.EstimatedBuckets);
            int estimatedDiggers = int.Parse(settingsInputForm.EstimatedDiggers);
            bool IouCheckboxChecked = settingsInputForm.OnIouCheckbox.Equals("Checked") ? true : false;

            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(contractorUser);
            InternalWorkforcePage internalWorkforcePage = GetActionsForEndToEndTests().CreateResourcePool(mainPage, disciplineTab, resourcePoolName, sourceLocation, estimatedResources, estimatedCrews, crewSize, estimatedBuckets, estimatedDiggers, IouCheckboxChecked, operatingCompanyName);

            ResourcePoolRow resourcePoolRow = internalWorkforcePage.GetResourcesPoolPanel().GetResourcePoolRowsByResourcePoolName(resourcePoolName)[0];

            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Source location expected: " + sourceLocation, resourcePoolRow.GetSourceLocation(), sourceLocation);
            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Estimated Resource location expected: " + estimatedResources.ToString(), resourcePoolRow.GetResource(), estimatedResources.ToString());
            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Estimated Crew expected: " + estimatedCrews.ToString(), resourcePoolRow.GetCrew(), estimatedCrews.ToString());
            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Crew Size expected: " + crewSize.ToString(), resourcePoolRow.GetCrewSize(), crewSize.ToString());
            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Estimated buckets expected: " + estimatedBuckets.ToString(), resourcePoolRow.GetBucket(), estimatedBuckets.ToString());
            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Estimated diggers expected: " + estimatedDiggers.ToString(), resourcePoolRow.GetDigger(), estimatedDiggers.ToString());
            resourcePoolRow.GetAssertionUtils().EquialAssertion("Verify that Modified date expected: " + "just now", resourcePoolRow.GetModified(), "just now");
            if (IouCheckboxChecked)
            {
                resourcePoolRow.GetAssertionUtils().TrueAssertion("Verify that IOU checked", resourcePoolRow.IsOnIouChecked());
            }
            else
            {
                resourcePoolRow.GetAssertionUtils().TrueAssertion("Verify that IOU unchecked", !resourcePoolRow.IsOnIouChecked());
            }


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
                .ClickTab(disciplineTab)
                .GetResourcesPoolPanel()
                .GetResourcePoolRowsByResourcePoolName(resourcePoolName)[0]
                .ClickOperationButton()
                .ClickDeleteButton()
                .ClickConfirmButton();
            Thread.Sleep(30000);
        }

        private class InputFormSettings
        {
            public string ResourcePoolName;
            public string SourceLocation;
            public string OnIouCheckbox;
            public string EstimatedResources;
            public string EstimatedCrews;
            public string CrewSize;
            public string EstimatedBuckets;
            public string EstimatedDiggers;
        }

    }
}
