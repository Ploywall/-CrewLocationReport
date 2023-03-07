using MimeKit;
using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.PageObjects.Web.Contractor.windows.crewavailabilitywindow;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.Utility;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E007 : BaseWebEndToEndTest
    {

        private static int numberOfRowForCurrentTestCase = 9;
        private static int numberOfRowForE2E008TestCase = 10;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);

        private String resourcePoolName;
        private InternalWorkforcePage.Tabs tabDiscipline = InternalWorkforcePage.GetTabByNameFoTab(discipline);
        private Boolean IsClearDataAfterTest = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForClearDataForTestCaseSheet);

        //read parameters from continue column
        private static ContinueFormSettingsForE2E007 settingsContinueFormForE2E007 = JsonConvert.DeserializeObject<ContinueFormSettingsForE2E007>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet));
        private static ContinueFormSettingsForE2E008 settingsContinueFormForE2E008 = JsonConvert.DeserializeObject<ContinueFormSettingsForE2E008>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForE2E008TestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet));

        [SetUp]
        public void CreateResourcePool()
        {
            String sourceLocation = "Aaronsburg, PA";
            int estimatedResources = 5;
            int estimatedCrews = 3;
            int crewSize = 4;
            int estimatedBuckets = 6;
            int estimatedDiggers = 6;

            //Create resource pool if we dont have resource pool from continue parameter
            if (settingsContinueFormForE2E007.ResourcePoolName != null && !settingsContinueFormForE2E007.ResourcePoolName.Equals(""))
            {
                resourcePoolName = settingsContinueFormForE2E007.ResourcePoolName;
            }
            else
            {
                resourcePoolName = GetRandomValuesUtilities().GetRandomValue();
                BaseInformation baseInformation = AddNewDriverWithDefaultSettings();
                MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(baseInformation, contractorUser);
                InternalWorkforcePage internalWorkforcePage = GetActionsForEndToEndTests().CreateResourcePool(mainPage, tabDiscipline, resourcePoolName, sourceLocation, estimatedResources, estimatedCrews, crewSize, estimatedBuckets, estimatedDiggers, checkIouCheckbox, operatingCompanyName);
                RemoveDriver(baseInformation);
            }
        }

        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E007_Test()
        {
            //Share Crew Availability Form
            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(contractorUser);
            InternalWorkforcePage internalWorkforcePage = mainPage.GetHeaderPanel()
                .ClickHomeButton()
                .GetHeaderPanel()
                .OpenWorkforceMenuPanel()
                .ClickInternalWorkforceButton()
                .ClickTab(tabDiscipline);
            String resourcePool = internalWorkforcePage.GetResourcesPoolPanel()
                .GetResourcePoolRowByResourcePoolNameFromAnyPage(resourcePoolName)
                .GetResourcePool();
            CrewAvailabilityFormWindow crewAvailabilityFormWindow = internalWorkforcePage.ClickCrewAvailabilityFormButton();
            crewAvailabilityFormWindow.GetResourcePoolTablePanel()
                .GetRowByResourcePoolFromAnyPage(resourcePool)
                .ClickCheckbox();
            crewAvailabilityFormWindow.GetUtilitiesTablePanel()
                .GetRowsByUtilityName(settingsContinueFormForE2E007.UtilityName)[0]
                .ClickCheckbox();
            DateTime timeOfAction = DateTime.Now;
            crewAvailabilityFormWindow.ClickShareCrewAvailability();
            internalWorkforcePage.GetHeaderPanel()
                .OpenAccountDropdownMenu()
                .ClickLogoutButton();

            //Check from utility user
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 9);
            NonIouMarketplacePage nonIouMarketplacePage = manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickNonIouMarketplaceButton();
            nonIouMarketplacePage.GetCrewAvailabilitiesPanel()
                .GetAssertionUtils()
                .TrueAssertion("Verify that Row with resource pool: " + resourcePool + " present on any page", nonIouMarketplacePage.GetCrewAvailabilitiesPanel().IsRowPresentOnAnyPageByResourcePool(resourcePool));

            //Test E2E008
            //check message from Message center panel
            String expectedSubject = "Notification of crew availability information updates from " + settingsContinueFormForE2E008.ContractorName + " to your company";
            WRMAutotests.PageObjects.Web.Utility.panel.MessageCenterPanel messageCenterPanel = nonIouMarketplacePage.GetHeaderPanel()
                .ClickMessagesButoon()
                .GetMessageCenterPanel();
            WRMAutotests.PageObjects.Web.Utility.panel.MessagePanel messagePanel = messageCenterPanel.GetUnreadEmailRowsByPartSubject(expectedSubject)[0]
                .ClickSubject();

            messagePanel.GetAssertionUtils().TrueAssertion("Verify that date of message expected or more: " + timeOfAction, messagePanel.GetDate() >= timeOfAction.AddHours(timeZoneDifferenceHours));

            //check message from Email
            MailRepository mailRepository = new MailRepository(utilityUser);
            IList<MimeMessage> foundEmails = mailRepository.GetUnreadEmailsByPartOfSubjectAndRecivedAndAfterDateTimeEmails("Notification of crew availability information updates from  Best's Line&Company to your company", utilityUser.GetEmail(), timeOfAction);
            messagePanel.GetAssertionUtils().TrueAssertionWithoutNameOfPageObject("Verify that expected Email recived", foundEmails.Count > 0);
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

        private class ContinueFormSettingsForE2E007
        {
            public string ResourcePoolName;
            public string UtilityName;
        }

        private class ContinueFormSettingsForE2E008
        {
            public string ContractorName;
        }

    }
}
