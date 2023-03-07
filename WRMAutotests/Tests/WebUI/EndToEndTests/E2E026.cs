using RetryOnException;
using WRMAutotests.PageObjects.Mobile.Android;
using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.Utility;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E026 : BaseMobileEndToEndTest
    {

        private String resourcePoolName = GetRandomValuesUtilities().GetRandomValue();
        private String resourcePool = null;

        private static int numberOfRowForCurrentTestCase = 28;
        private static String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);

        private InternalWorkforcePage.Tabs tabDiscipline = InternalWorkforcePage.GetTabByNameFoTab(discipline);
        private String fullResourceName = "Automation User0001";
        private String supervisor = "Adam Turner";


        //[SetUp]
        public void PrepareEnv()
        {
            int estimatedResources = 5;
            int estimatedCrews = 3;
            int crewSize = 4;
            int estimatedBuckets = 6;
            int estimatedDiggers = 6;

            String timeSheetLastSubmitter = "Manual Best";
            String expenceLastSubmitter = "ALead01 Test";

            BaseInformation baseInformationForContractor = AddNewDriverWithDefaultSettings();
            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(baseInformationForContractor, contractorUser);
            InternalWorkforcePage internalWorkforcePage = GetActionsForEndToEndTests().CreateResourcePool(mainPage, tabDiscipline, resourcePoolName, defaultSourceLocation, estimatedResources, estimatedCrews, crewSize, estimatedBuckets, estimatedDiggers, checkIouCheckbox, operatingCompanyName);
            internalWorkforcePage = GetActionsForEndToEndTests().CreateCrewAvailabilityForm(internalWorkforcePage, resourcePoolName, operatingCompanyName);

            BaseInformation baseInformationForUtility = AddNewDriverWithDefaultSettings();
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 22, baseInformationForUtility);
            resourcePool = GetActionsForEndToEndTests().CreateProcureCrew(manageSecuredWorkforcePage, defaultSourceLocation, resourcePoolName, supervisor, GetRandomValuesUtilities().GetRandomValue());

            ProcurementRequestPage procurementRequestPage = internalWorkforcePage.GetHeaderPanel()
                .OpenWorkforceMenuPanel()
                .ClickProcurementRequest();
            GetActionsForEndToEndTests().AcceptProcurenmentRequest(procurementRequestPage, resourcePool);

            //edit organization
            internalWorkforcePage = procurementRequestPage.GetHeaderPanel()
                .OpenWorkforceMenuPanel()
                .ClickInternalWorkforceButton();
            ResourcePoolEditOrganizationPage resourcePoolEditOrganizationPage = internalWorkforcePage.GetResourcesPoolPanel()
                .GetResourcePoolRowsByResourcePoolName(resourcePoolName)[0]
                .ClickResourcePool()
                .ClickEditOrganizationButton();

            String type = "Bucket";
            String subType = "TestBucket";
            String licensePlate = GetRandomValuesUtilities().GetRandomValue();
            String LicenseState = "AL";
            String equipmentId = GetRandomValuesUtilities().GetRandomValue();
            resourcePoolEditOrganizationPage = GetActionsForEndToEndTests().AddEquipmentToOrganization(resourcePoolEditOrganizationPage, type, subType, licensePlate, LicenseState, equipmentId);
            resourcePoolEditOrganizationPage = GetActionsForEndToEndTests().DragAndDropResourceToOrganization(resourcePoolEditOrganizationPage, fullResourceName, 4);
            resourcePoolEditOrganizationPage = GetActionsForEndToEndTests().DragAndDropEqupment(resourcePoolEditOrganizationPage, licensePlate, 4);
            resourcePoolEditOrganizationPage.GetResourcePoolEditOrganizationPanel().ClickSaveButton();
            Thread.Sleep(10000);

            //need steps for contractor
            internalWorkforcePage = resourcePoolEditOrganizationPage.GetHeaderPanel().OpenWorkforceMenuPanel()
                .ClickInternalWorkforceButton();
            ResourcePoolOverviewPage resourcePoolOverviewPage = internalWorkforcePage.GetResourcesPoolPanel()
                .GetResourcePoolRowsByResourcePoolName(resourcePoolName)[0]
                .ClickResourcePool();
            GetActionsForEndToEndTests().CreateCrewSheetAddResources(resourcePoolOverviewPage, resourcePoolName, operatingCompanyName);
            ResourcePoolEditCrewSheetPage resourcePoolEditCrewSheetPage = resourcePoolOverviewPage.GetCrewSheetsPanel()
                .GetRowByCrewSheetName(resourcePoolName)
                .ClickCrewSheetIdLink();
            GetActionsForEndToEndTests().SelectTimeSheetLastSubmitterAndExpenceLastSubmitter(resourcePoolEditCrewSheetPage, timeSheetLastSubmitter, expenceLastSubmitter);

            //need steps for utility
            manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenEventDropdownMenu()
                .ClickEventByName("— All Include No Event —");
            manageSecuredWorkforcePage = manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickManageSecuredWorkforce();
            manageSecuredWorkforcePage.GetSecuredWorkforcesPanel()
                .GetRowByResourcePoolFromAmyPage(resourcePool)
                .ClickChecbox();
            GetActionsForEndToEndTests().ApplyEvent(manageSecuredWorkforcePage, eventName, DateTime.Now.AddDays(-1));
            GetActionsForEndToEndTests().AddSupervisor(manageSecuredWorkforcePage, supervisor);
            GetActionsForEndToEndTests().AssignLocationRegion(manageSecuredWorkforcePage, "[North - Central]", DateTime.Now.AddDays(-1), GetRandomValuesUtilities().GetRandomValue());
            RemoveDriver(baseInformationForContractor);
            RemoveDriver(baseInformationForUtility);
        }


        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        //[Test]
        public void E2E026_Test()
        {
            SelectWorkspaceScreen selectWorkspaceScreen = OpenAndroidApplication();

            //E2E026
            String contractorWorkspace = "MT Dev (New) Contractor";
            String contractorEmail = "hiyaval829@fsouda.com";
            String contractorPassword = "UseR5^7%%9";
            selectWorkspaceScreen.SelectWorkspace(contractorWorkspace);
            WorkspaceLoginScreen workspaceLoginScreen = selectWorkspaceScreen.ClickNextButton();
            workspaceLoginScreen.EnterEmail(contractorEmail);
            workspaceLoginScreen.EnterPassword(contractorPassword);
            TurnOnYourCurrentLocationScreen turnOnYourCurrentLocationScreen = workspaceLoginScreen.ClickLoginButton();
            OnTheClockScreen onTheClockScreen = turnOnYourCurrentLocationScreen.ClickOk();

            //E2E027
            onTheClockScreen.ClickClockInButton();
            onTheClockScreen.SelectWorkStatus("Work");


        }


        [TearDown]
        public void RemoveInstalledApplication()
        {
            RemoveInstalledAndroidApplication();
        }


    }
}
