using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.PageObjects.Web.Contractor.panels;
using WRMAutotests.PageObjects.Web.Contractor.windows;
using WRMAutotests.PageObjects.Web.Contractor.windows.crewavailabilitywindow;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.PageObjects.Web.Utility.windows;
using WRMAutotests.Utility;
using WRMAutotests.Utility.Web;
using static WRMAutotests.PageObjects.Web.Utility.pages.ManageSecuredWorkforcePage;

namespace WRMAutotests.Tests.WebUI.Actions.Web
{
    public class ActionsForEndToEndTests
    {
        private BaseInformation baseInformation;
        private String defaultContractorUrl;
        private String defaultUtilityUrl;

        public ActionsForEndToEndTests(BaseInformation baseInformation, String defaultContractorUrl, String defaultUtilityUrl)
        {
            this.baseInformation = baseInformation;
            this.defaultContractorUrl = defaultContractorUrl;
            this.defaultUtilityUrl = defaultUtilityUrl;
        }

        private BaseInformation GetDefaultBaseInformation()
        {
            return baseInformation;
        }

        //methods that need for login into utility
        public ManageSecuredWorkforcePage LoginToUtilityWithSettingsFromExcel(ExcelReadedUtils excelReadedUtils, User utilityUser, int targetRowOnTestCaseSheet)
        {
            return LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, targetRowOnTestCaseSheet, GetDefaultBaseInformation());
        }
        public ManageSecuredWorkforcePage LoginToUtilityWithSettingsFromExcel(ExcelReadedUtils excelReadedUtils, User utilityUser, int targetRowOnTestCaseSheet, BaseInformation baseInformation)
        {
            //read all need data from excel
            String defaultOperatingCompanyFromExcel = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 14, 1);
            String defaultEventFromExcel = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 15, 1);
            String operatingCompanyFromExcel = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, targetRowOnTestCaseSheet, 2);
            String eventFromExcel = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, targetRowOnTestCaseSheet, 3); ;

            //login and open page for selecting operating company and event
            DefaultPage defaultPage = OpenDefaultUtilityPage(baseInformation);
            WRMAutotests.PageObjects.Web.Utility.pages.LoginPage loginPage = defaultPage.ClickLoginButton();
            loginPage.GetLoginPanel().EnterEmail(utilityUser.GetEmail());
            loginPage.GetLoginPanel().EnterPassword((utilityUser.GetPassword()));
            EventSelectionPage eventSelectionPage = loginPage.GetLoginPanel().ClickLoginButton();

            //Find final value of operating company and event. Value from sheet "TestCase" have first priority. Default values of Event and operating company have second priority
            String finalOperatingCompany = operatingCompanyFromExcel.Equals("") ? defaultOperatingCompanyFromExcel : operatingCompanyFromExcel;
            String finalEventName = eventFromExcel.Equals("") ? defaultEventFromExcel : eventFromExcel;

            //need to chouse our actions accoting to read values from Excel
            if (finalOperatingCompany.Equals(""))
            {
                if (finalEventName.Equals(""))
                {
                    //when operating company and event are empty we should click "Access without event selection"
                    Thread.Sleep(5000);
                    return eventSelectionPage.GetWelcomeToStormManagerPanel().ClickAccessWithoutEventSelection();
                }
                else
                {
                    //when operating company is empty but event filled we shoud chouse only event
                    Thread.Sleep(5000);
                    return eventSelectionPage.GetWelcomeToStormManagerPanel().SelectEvent(finalEventName);
                }
            }
            else
            {
                if (finalEventName.Equals(""))
                {
                    //when operating company filled but event is empty we should chouse all active events
                    eventSelectionPage.GetWelcomeToStormManagerPanel().SelectOperatingCompany(finalOperatingCompany);
                    Thread.Sleep(5000);
                    return eventSelectionPage.GetWelcomeToStormManagerPanel().SelectEvent("All Active Events");
                }
                else
                {
                    //when we have operation company and event - we should chouse them
                    eventSelectionPage.GetWelcomeToStormManagerPanel().SelectOperatingCompany(finalOperatingCompany);
                    Thread.Sleep(5000);
                    return eventSelectionPage.GetWelcomeToStormManagerPanel().SelectEvent(finalEventName);
                }

            }

        }



        public void OpenUrl(BaseInformation baseInformation, String url)
        {
            baseInformation.GetDriver().Navigate().GoToUrl(url);
        }

        public void OpenUrl(String url)
        {
            OpenUrl(GetDefaultBaseInformation(), url);
        }

        public WRMAutotests.PageObjects.Web.Contractor.pages.LoginPage OpenLoginPageForContractor()
        {
            return OpenLoginPageForContractor(GetDefaultBaseInformation());
        }
        public WRMAutotests.PageObjects.Web.Contractor.pages.LoginPage OpenLoginPageForContractor(BaseInformation baseInformation)
        {
            OpenUrl(baseInformation, defaultContractorUrl);
            return new WRMAutotests.PageObjects.Web.Contractor.pages.LoginPage(baseInformation);
        }

        public DefaultPage OpenDefaultUtilityPage(BaseInformation baseInformation)
        {
            OpenUrl(baseInformation, defaultUtilityUrl);
            return new DefaultPage(baseInformation);
        }

        public DefaultPage OpenDefaultUtilityPage()
        {
            return OpenDefaultUtilityPage(GetDefaultBaseInformation());
        }

        public ManageSecuredWorkforcePage LoginIntoDefaultUtility(BaseInformation baseInformation, User user)
        {
            DefaultPage defaultPage = OpenDefaultUtilityPage(baseInformation);
            WRMAutotests.PageObjects.Web.Utility.pages.LoginPage loginPage = defaultPage.ClickLoginButton();
            loginPage.GetLoginPanel().EnterEmail(user.GetEmail());
            loginPage.GetLoginPanel().EnterPassword(user.GetPassword());
            EventSelectionPage eventSelectionPage = loginPage.GetLoginPanel().ClickLoginButton();
            return eventSelectionPage.GetWelcomeToStormManagerPanel()
                .ClickAccessWithoutEventSelection();

        }

        public ManageSecuredWorkforcePage LoginIntoDefaultUtility(User user)
        {
            return LoginIntoDefaultUtility(GetDefaultBaseInformation(), user);
        }

        public ManageSecuredWorkforcePage LoginIntoDefaultUtility(BaseInformation baseInformation, User user, String operationCompany, String eventName)
        {
            DefaultPage defaultPage = OpenDefaultUtilityPage(baseInformation);
            WRMAutotests.PageObjects.Web.Utility.pages.LoginPage loginPage = defaultPage.ClickLoginButton();
            loginPage.GetLoginPanel().EnterEmail(user.GetEmail());
            loginPage.GetLoginPanel().EnterPassword(user.GetPassword());
            EventSelectionPage eventSelectionPage = loginPage.GetLoginPanel().ClickLoginButton();
            eventSelectionPage.GetWelcomeToStormManagerPanel().SelectOperatingCompany(operationCompany);
            return eventSelectionPage.GetWelcomeToStormManagerPanel().SelectEvent(eventName);

        }

        public ManageSecuredWorkforcePage LoginIntoDefaultUtility(User user, String operationCompany, String eventName)
        {
            return LoginIntoDefaultUtility(GetDefaultBaseInformation(), user, operationCompany, eventName);
        }

        public MainPage LoginUntoDefaultContractor(BaseInformation baseInfortamtion, User user)
        {
            WRMAutotests.PageObjects.Web.Contractor.pages.LoginPage loginPage = OpenLoginPageForContractor(baseInfortamtion);
            loginPage.EnterEmail(user.GetEmail());
            loginPage.EnterPassword(user.GetPassword());
            return loginPage.ClickLoginButton();
        }

        public MainPage LoginUntoDefaultContractor(User user)
        {
            return LoginUntoDefaultContractor(GetDefaultBaseInformation(), user);
        }

        public void CreateCrewAvailabilityRequest(ManageSecuredWorkforcePage manageSecuredWorkforcePage, String operatingCompany, String contractor, String subject, String message, String discipline)
        {
            manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenOperatingCompanyDropdownMenu()
                .ClickOperatingCompanyByName(operatingCompany);
            CreawAvailabilityRequestPage creawAvailabilityRequestPage = manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickCrewAvailabilityRequestButton()
                .ClickTabByName(discipline);
            creawAvailabilityRequestPage.GetCrewAvailabilityRequestContractorsTablePanel()
                .GetRowByContractorFromAnyPage(contractor)
                .ClickCheckbox();
            CrewAvailabilityRequestWindow crewAvailabilityRequestWindow = creawAvailabilityRequestPage.ClickRequestCrewAvailabilityInfo();
            crewAvailabilityRequestWindow.EnterSubject(subject);
            crewAvailabilityRequestWindow.EnterMessage(message);
            crewAvailabilityRequestWindow.ClickRequestCrewAvailabilityInfo();
        }

        public InternalWorkforcePage CreateResourcePool(MainPage mainPage, InternalWorkforcePage.Tabs tab, String resourcePoolName, String sourceLocation, int estimatedResources, int estimatedCrews, int crewSize, int estimatedBuckets, int estimatedDiggers, Boolean checkOnIouCheckbox, String utilityName)
        {
            AddResourcePoolPage addResourcePoolPage = mainPage.GetHeaderPanel()
                .ClickHomeButton()
                .GetHeaderPanel()
                .OpenWorkforceMenuPanel()
                .ClickInternalWorkforceButton()
                .ClickTab(tab)
                .ClickAddButton();
            addResourcePoolPage.EnterResourcePoolName(resourcePoolName);
            addResourcePoolPage.GetSourceLocationDropDownMenu().SelectMenuElement(sourceLocation);
            if (checkOnIouCheckbox)
            {
                addResourcePoolPage.CheckOnIouCheckbox();
                addResourcePoolPage.GetUtilityDropDownMenu().SelectMenuElement(utilityName);
            }
            else
            {
                addResourcePoolPage.UncheckOnIouCheckbox();
            }

            addResourcePoolPage.EnterEstimatedResources(estimatedResources);
            addResourcePoolPage.EnterExtimatedCrews(estimatedCrews);
            addResourcePoolPage.EnterCrewSize(crewSize);
            addResourcePoolPage.EnterEstimatedBuckets(estimatedBuckets);
            addResourcePoolPage.EnterEstimatedDiggers(estimatedDiggers);
            InternalWorkforcePage internalWorkforcePage = addResourcePoolPage.ClickAddButton();
            return internalWorkforcePage;
        }

        public InternalWorkforcePage CreateCrewAvailabilityForm(InternalWorkforcePage internalWorkforcePage, String resourcePoolName, String operatingCompany)
        {
            String resourcePool = internalWorkforcePage.GetResourcesPoolPanel()
                .GetResourcePoolRowByResourcePoolNameFromAnyPage(resourcePoolName)
                .GetResourcePool();
            CrewAvailabilityFormWindow crewAvailabilityFormWindow = internalWorkforcePage.ClickCrewAvailabilityFormButton();
            crewAvailabilityFormWindow.GetResourcePoolTablePanel()
                .GetRowByResourcePoolFromAnyPage(resourcePool)
                .ClickCheckbox();
            crewAvailabilityFormWindow.GetUtilitiesTablePanel()
                .GetRowsByUtilityName(operatingCompany)[0]
                .ClickCheckbox();
            crewAvailabilityFormWindow.ClickShareCrewAvailability();
            return internalWorkforcePage;
        }

        public String CreateProcureCrew(ManageSecuredWorkforcePage manageSecuredWorkforcePage, String sourceLocation, String resourcePoolName, String assignedSupervisor, String additionalRequirement)
        {
            NonIouMarketplacePage nonIouMarketplacePage = manageSecuredWorkforcePage.GetHeaderPanel()
                .OpenWorkforceEventsManuPanel()
                .ClickNonIouMarketplaceButton();
            nonIouMarketplacePage.GetLocationDropDownMenu()
                .SelectMenuElement(sourceLocation);
            nonIouMarketplacePage.ClickUpdateButton();
            WRMAutotests.PageObjects.Web.Utility.panel.CrewAvailabilitiesPanel.Row targetrow = nonIouMarketplacePage.GetCrewAvailabilitiesPanel()
                .GetRowFromAnyPageByResourcePoolName(resourcePoolName);
            String resourcePool = targetrow.GetResourcePool();
            targetrow.ClickCheckbox();
            ProcureContractorCrewWindow procureContractorCrewWindow = nonIouMarketplacePage.ClickProcureCrew();
            procureContractorCrewWindow.ClickAllResourcesCheckbox();
            procureContractorCrewWindow.ClickAllBucketCheckbox();
            procureContractorCrewWindow.ClickAllDiggersCheckbox();
            procureContractorCrewWindow.SelectDestination(sourceLocation);
            procureContractorCrewWindow.SelectWorkStatus(ProcureContractorCrewWindow.WorkStatus.Immediatly_Mobilize);
            procureContractorCrewWindow.AddAssignedSupervisor(assignedSupervisor);
            procureContractorCrewWindow.EnterAdditionalRequirement(additionalRequirement);
            procureContractorCrewWindow.ClickProcureCrewsButton();
            Thread.Sleep(10000);
            return resourcePool;
        }

        public void AcceptProcurenmentRequest(ProcurementRequestPage procurementRequestPage, String resourcePool)
        {
            ProcurementRequestsPanel.Row row = procurementRequestPage.GetProcurementRequestsPanel()
               .GetRowsFromCurrentPageByResourcePool(resourcePool)[0];
            row.ClickEditButton()
                .ClickAcceptButton()
                .ClickConfirmButton();
            Thread.Sleep(10000);
        }

        public ResourcePoolEditOrganizationPage AddResourceToOrganization(ResourcePoolEditOrganizationPage resourcePoolEditOrganizationPage, String resourceFirstName, String resourceLastName, String classification)
        {
            String fullResourceName = resourceFirstName + " " + resourceLastName;
            resourcePoolEditOrganizationPage.GetBaseInformation().GetDriver().Navigate().Refresh();
            AddResourceWindow addresourceWindow = resourcePoolEditOrganizationPage.GetResourcePoolEditOrganizationPanel().ClickAddResourceButton();
            addresourceWindow.EnterFirstName(resourceFirstName);
            addresourceWindow.EnterLastName(resourceLastName);
            addresourceWindow.SelectClassification(classification);
            addresourceWindow.ClickAddResource();
            return resourcePoolEditOrganizationPage;
        }

        public ResourcePoolEditOrganizationPage AddEquipmentToOrganization(ResourcePoolEditOrganizationPage resourcePoolEditOrganizationPage, String type, String subType, String licensePlate, String LicenseState, String equipmentId)
        {
            resourcePoolEditOrganizationPage.GetResourcePoolEditOrganizationPanel()
                .ClickEquimentTab();
            AddEquipmentWindow addEquipmentWindow = resourcePoolEditOrganizationPage.GetResourcePoolEditOrganizationPanel().ClickAddEquipmentButton();
            addEquipmentWindow.SelectType(type);
            addEquipmentWindow.SelectSubType(subType);
            addEquipmentWindow.EnterLicensePlate(licensePlate);
            addEquipmentWindow.SelectLicenseState(LicenseState);
            addEquipmentWindow.EnterEquipmentId(equipmentId);
            addEquipmentWindow.ClickAddEquipment();
            return resourcePoolEditOrganizationPage;
        }

        public ResourcePoolEditOrganizationPage DragAndDropResourceToOrganization(ResourcePoolEditOrganizationPage resourcePoolEditOrganizationPage, String fullResourceName, int numberOfORganizationLevel)
        {
            ResourcePoolEditOrganizationPanel resourcePoolEditOrganizationPanel = resourcePoolEditOrganizationPage.GetResourcePoolEditOrganizationPanel();
            resourcePoolEditOrganizationPanel.ClickResourceTab();
            resourcePoolEditOrganizationPanel.SearchResource(fullResourceName);
            ResourcePoolEditOrganizationPanel.ResourceRow resourceRow = resourcePoolEditOrganizationPanel.GetResourceRowsByResourceName(fullResourceName)[0];
            ResourcePoolEditOrganizationPanel.OrganizationRow organizationRow = resourcePoolEditOrganizationPanel.GetOrganizationRows()[numberOfORganizationLevel];
            resourcePoolEditOrganizationPanel.DragResourceIntoOrganization(resourceRow, organizationRow);
            return resourcePoolEditOrganizationPage;
        }

        public ResourcePoolEditOrganizationPage DragAndDropEqupment(ResourcePoolEditOrganizationPage resourcePoolEditOrganizationPage, String licensePlate, int numberOfORganizationLevel)
        {
            ResourcePoolEditOrganizationPanel resourcePoolEditOrganizationPanel = resourcePoolEditOrganizationPage.GetResourcePoolEditOrganizationPanel();
            resourcePoolEditOrganizationPanel.ClickEquimentTab();
            resourcePoolEditOrganizationPanel.SearchEqupment(licensePlate);
            ResourcePoolEditOrganizationPanel.EquipmentRow equipmentRow = resourcePoolEditOrganizationPanel.GetEquipmentRowByLicensePlateEquipmentId(licensePlate)[0];
            ResourcePoolEditOrganizationPanel.OrganizationRow organizationRow = resourcePoolEditOrganizationPanel.GetOrganizationRows()[numberOfORganizationLevel];
            resourcePoolEditOrganizationPanel.DragEqupmentIntoOrganization(equipmentRow, organizationRow);
            return resourcePoolEditOrganizationPage;
        }

        public String CreateCrewSheetAddResources(ResourcePoolOverviewPage resourcePoolOverviewPage, String resourcePoolName, String operatingCompanyName)
        {
            resourcePoolOverviewPage.ClickNewCrewSheetButton();
            String crewSheet = resourcePoolOverviewPage.GetCrewSheetsPanel()
                .GetRowByCrewSheetName(resourcePoolName)
                .GetCrewSheet();
            resourcePoolOverviewPage.GetResourcePoolResourcesPanel()
                .GetRows()[0]
                .ClickCheckbox();
            resourcePoolOverviewPage.GetResourcePoolResourcesPanel()
                .ClickAssignButton()
                .AssignToCrewSheet(crewSheet);
            resourcePoolOverviewPage.GetCrewSheetsPanel()
                .GetRows()[0]
                .ClickCheckbox();
            resourcePoolOverviewPage.ClickSubmitButton()
                .SelectUtility(operatingCompanyName)
                .ClickConfirmButton();
            return crewSheet;
        }

        public void SelectTimeSheetLastSubmitterAndExpenceLastSubmitter(ResourcePoolEditCrewSheetPage resourcePoolEditCrewSheetPage, String timesheetLastSubmitter, String expenseLastSubmitter)
        {
            resourcePoolEditCrewSheetPage.SelectTimeSheetLastSubmitter(timesheetLastSubmitter);
            resourcePoolEditCrewSheetPage.SelectExpenceLastSubmitter(expenseLastSubmitter);
        }

        public void ApplyEvent(ManageSecuredWorkforcePage manageSecuredWorkforcePage, String eventName, DateTime dateTime)
        {
            manageSecuredWorkforcePage.ClickEventButton()
                .ApplyEvent(eventName, dateTime);
        }

        public void AddSupervisor(ManageSecuredWorkforcePage manageSecuredWorkforcePage, String supervisor)
        {
            manageSecuredWorkforcePage.ClickSupervisorButton()
                .SelectSupervisorAndClickAssignButton(supervisor);
        }

        public void AssignLocationRegion(ManageSecuredWorkforcePage manageSecuredWorkforcePage, String location, DateTime date, String etaComment)
        {
            AssignedLocation assignedLocation = manageSecuredWorkforcePage.ClickAssignedLocationButton();
            assignedLocation.ClickRegionTypeOfLocation();
            assignedLocation.SelectLocation(location);
            assignedLocation.SelectDate(date);
            assignedLocation.EnterAdditionalRequiments(etaComment);
            assignedLocation.ClickAssignButton();
        }

    }
}
