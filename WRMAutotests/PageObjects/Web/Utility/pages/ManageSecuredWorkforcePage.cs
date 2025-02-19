﻿using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using WRMAutotests.PageObjects.Web.Base;
using WRMAutotests.PageObjects.Web.BaseElements;
using WRMAutotests.PageObjects.Web.Utility.panel;
using WRMAutotests.PageObjects.Web.Utility.windows;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Web.Utility.pages
{
    public class ManageSecuredWorkforcePage : WRMAutotests.PageObjects.Web.Utility.pages.BaseLoggedPage
    {

        [FindsBy(How = How.CssSelector, Using = "div[title='Assign event to selected resource pool/ crew sheet']")]
        private IWebElement eventButton;

        [FindsBy(How = How.CssSelector, Using = "div[title='Assign Supervisor to selected resource pool/ crew sheet']")]
        private IWebElement supervisorButton;

        [FindsBy(How = How.CssSelector, Using = "div[title='Assign destination to selected resource pool/ crew sheet']")]
        private IWebElement assignedLocationButton;

        [FindsBy(How = How.CssSelector, Using = "ul.dxtc-strip > li.dxtc-tab")]
        private IList<IWebElement> tabs;

        [FindsBy(How = How.CssSelector, Using = "ul.dxtc-strip > li.dxtc-activeTab")]
        private IList<IWebElement> activeTabs;

        public ManageSecuredWorkforcePage(BaseInformation baseInformation) : base(baseInformation, new WRMAutotests.Utility.ReportUtils(baseInformation, "Utility Manage Secured Workforce", "page"))
        {
        }

        public SecuredWorkforcesPanel GetSecuredWorkforcesPanel()
        {
            GetWaitUtils().WaitForLoadingPanelAbsent();
            return new SecuredWorkforcesPanel(GetBaseInformation());
        }

        public CrewAvailabilitiesPanel GetCrewAvailabilitiesPanel()
        {
            return new CrewAvailabilitiesPanel(GetBaseInformation());
        }


        public EventMenu ClickEventButton()
        {
            GetReportUtils().ClickButton("Event");
            GetWebElementUtils().clickWebElement(eventButton);
            GetWaitUtils().WaitForLoadingPanelAbsent();
            Thread.Sleep(5000);
            return new EventMenu(GetBaseInformation());
        }

        public SupervisorMenu ClickSupervisorButton()
        {
            GetReportUtils().ClickButton("Supervisor");
            GetWebElementUtils().clickWebElement(supervisorButton);
            Thread.Sleep(30000);
            GetWaitUtils().WaitForLoadingPanelAbsent();
            return new SupervisorMenu(GetBaseInformation());
        }

        public AssignedLocation ClickAssignedLocationButton()
        {
            GetReportUtils().ClickButton("Assigned Location");
            GetWebElementUtils().clickWebElement(assignedLocationButton);
            return new AssignedLocation(GetBaseInformation());
        }

        public class AssignedLocation : BasePageObject
        {

            private static By rootLocator = By.CssSelector("#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignDestination_PW-1");

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignDestination_UtilityLocations_rdnUSCUtilityLocation td > table.dxeBase_DevEx.dxeTAR")]
            private IList<IWebElement> typeOlLocation;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignDestination_UtilityLocations_cpnUSCUtilityLocation_cbUSCUtilityLocationDestination_ET")]
            private IWebElement locationSelectorRootElement;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignDestination_dtETAAssignDestination_B-1")]
            private IWebElement dateTimeDropdownMenu;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignDestination_dtETAAssignDestination_DDD_PW-1")]
            private IWebElement dateTimePanelRoot;

            [FindsBy(How = How.CssSelector, Using = "textarea#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignDestination_eta_comment_ASPxTextBox_I")]
            private IWebElement textAreaAdditionalInfo;

            [FindsBy(How = How.CssSelector, Using = "div[id*='BtnAssignDestinationSave']")]
            private IWebElement assignButton;

            public AssignedLocation(BaseInformation baseInformation) : base(baseInformation, rootLocator, new ReportUtils(baseInformation, "Assigned location menu", "panel"))
            {
            }

            private void ClickTypeOfLocationByNumber(int number)
            {
                typeOlLocation[number].Click();
                GetWaitUtils().WaitForLoadingPanelAbsent(180);
            }

            public void ClickRegionTypeOfLocation()
            {
                GetReportUtils().ClickButton("Region");
                ClickTypeOfLocationByNumber(3);
            }

            public void SelectLocation(String locationName)
            {
                new StandardDropdownMenu(GetBaseInformation(), locationSelectorRootElement, new ReportUtils(GetBaseInformation(), "Location", "dropdown Menu")).SelectMenuElement(locationName);
            }

            public void SelectDate(DateTime dateTime)
            {
                GetReportUtils().AllureStepWithPageObject("Select Date and time");
                dateTimeDropdownMenu.Click();
                Thread.Sleep(5000);
                new DateTimeSelectorWindow(GetBaseInformation(), dateTimePanelRoot).SelectDate(dateTime);
            }

            public void EnterAdditionalRequiments(String text)
            {
                GetReportUtils().EnterValueToField("Additional info", text);
                GetWebElementUtils().enterValueToFieldWithWaitEntering(textAreaAdditionalInfo, text);
            }

            public void ClickAssignButton()
            {
                GetReportUtils().ClickButton("Assign");
                GetWebElementUtils().clickWebElement(assignButton);
                GetWaitUtils().WaitForLoadingPanelAbsent();
            }

        }

        public class SupervisorMenu : BasePageObject
        {

            private static By rootLocator = By.CssSelector("#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignSupervisor_PW-1");

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignSupervisor_cpnRefreshBirdDog")]
            private IWebElement superuserRoot;

            [FindsBy(How = How.CssSelector, Using = "div[title='Assign Supervisor']")]
            private IWebElement assignButton;

            public SupervisorMenu(BaseInformation baseInformation) : base(baseInformation, rootLocator, new ReportUtils(baseInformation, "Supervisor menu", "panel"))
            {
            }

            public void SelectSupervisor(String supervisorName)
            {
                GetReportUtils().AllureStepWithPageObject("Select Assigned Supervisor: " + supervisorName);
                IWebElement field = superuserRoot.FindElement(By.CssSelector("#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignSupervisor_cpnRefreshBirdDog_tkbAssignSupervisor_I"));
                GetWebElementUtils().enterValueToFieldWithWaitEntering(field, supervisorName);
                Thread.Sleep(5000);
                IList<IWebElement> possibleUsers = superuserRoot.FindElements(By.CssSelector("#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignSupervisor_cpnRefreshBirdDog_tkbAssignSupervisor_DDD_L_LBT tr.dxeListBoxItemRow_DevEx"))
                    .Where(r => r.FindElement(By.CssSelector(".dxeListBoxItem_DevEx")).Text.Equals(supervisorName))
                    .ToList();
                GetWebElementUtils().clickWebElement(possibleUsers[0]);
                Thread.Sleep(3000);
                field.SendKeys("d");
                Thread.Sleep(1000);
                field.SendKeys(Keys.Backspace);
                Thread.Sleep(2000);
            }

            public void ClickAssignButton()
            {
                GetReportUtils().ClickButton("Assign");
                try
                {
                    assignButton.Click();
                }
                catch (ElementClickInterceptedException ex)
                {
                    Thread.Sleep(2000);
                    GetWebElementUtils().clickWebElement(assignButton);
                }
                GetWaitUtils().WaitForLoadingPanelAbsent();
            }

            public void SelectSupervisorAndClickAssignButton(String supervisorName)
            {
                SelectSupervisor(supervisorName);
                ClickAssignButton();
            }




        }

        public class EventMenu : BasePageObject
        {

            private static By rootLocator = By.CssSelector("#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignEvent_PWC-1");

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignEvent_assign_event_mscwf_ASPxComboBox_CallbackPanel")]
            private IWebElement eventSelectorRootElement;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignEvent_dtEventDate_DDD_PWC-1")]
            private IWebElement dateTimePanelRoot;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignEvent_dtEventDate_B-1")]
            private IWebElement dateTimeDropdownMenu;

            [FindsBy(How = How.CssSelector, Using = "div#ASPxPanel2_ContentPlaceHolder1_cpnTokenDeleted_cpnMain_pucBtnAssignEvent_cpnBtnAssignEventSave_assign_event_mscwf_save_ASPxButton_CD")]
            private IWebElement applyEventButton;

            public EventMenu(BaseInformation baseInformation) : base(baseInformation, rootLocator, new ReportUtils(baseInformation, "Event menu", "panel"))
            {
            }

            public void SelectEvent(String eventName)
            {
                new StandardDropdownMenu(GetBaseInformation(), eventSelectorRootElement, new ReportUtils(GetBaseInformation(), "Event", "dropdown menu")).SelectMenuElement(eventName);
            }

            public void SelectTime(DateTime dateTime)
            {
                GetReportUtils().AllureStepWithPageObject("Select Date and time");
                dateTimeDropdownMenu.Click();
                Thread.Sleep(5000);
                new DateTimeSelectorWindow(GetBaseInformation(), dateTimePanelRoot).SelectDate(dateTime);
            }

            public void ClickAppluButton()
            {
                GetReportUtils().ClickButton("Apply Event");
                GetWebElementUtils().clickWebElement(applyEventButton);
                GetWaitUtils().WaitForLoadingPanelAbsent();
            }

            public void ApplyEvent(String eventName, DateTime dateTime)
            {
                SelectEvent(eventName);
                SelectTime(dateTime);
                ClickAppluButton();
            }



        }

        public ManageSecuredWorkforcePage ClickTab(Tabs tab)
        {
            Thread.Sleep(10000);
            switch (tab)
            {
                case Tabs.Airboats:
                    {
                        ClickTabByName("Airboats");
                        break;
                    }
                case Tabs.Damage_Assessment:
                    {
                        ClickTabByName("Damage Assessment");
                        break;
                    }
                case Tabs.Distribution_Line:
                    {
                        ClickTabByName("Distribution Line");
                        break;
                    }
                case Tabs.Distribution_Veg_Mgmt:
                    {
                        ClickTabByName("Distribution Veg Mgmt");
                        break;
                    }
                case Tabs.Substation:
                    {
                        ClickTabByName("Substation");
                        break;
                    }
                case Tabs.Support:
                    {
                        ClickTabByName("Support");
                        break;
                    }
                case Tabs.Transmission_Line:
                    {
                        ClickTabByName("Transmission Line");
                        break;
                    }
                case Tabs.UAS:
                    {
                        ClickTabByName("UAS");
                        break;
                    }
                case Tabs.UG_Network:
                    {
                        ClickTabByName("UG Network");
                        break;
                    }
            }
            return this;
        }

        private void ClickTabByName(String tabName)
        {
            GetReportUtils().ClickButton("Tab: " + tabName);
            IWebElement tabButton = null;
            try
            {
                tabButton = tabs.Where(r => r.FindElement(By.CssSelector("a > span")).Text.Equals(tabName)).First();
            }
            catch (Exception ex)
            {
                //checked that we already on expected tab or not
                if (activeTabs.Where(r => r.FindElement(By.CssSelector("a > span")).Text.Equals(tabName)).ToList().Count > 0)
                {
                    return;
                }
                else
                {
                    throw new AssertionException("Absent tab with name: " + tabName);
                }

            }

            GetWebElementUtils().clickWebElement(tabButton);
            GetWaitUtils().WaitForLoadingPanelAbsent();
            Thread.Sleep(10000);
            GetWaitUtils().WaitForLoadingPanelAbsent();
        }

        public enum Tabs
        {
            Airboats,
            Damage_Assessment,
            Distribution_Line,
            Distribution_Veg_Mgmt,
            Substation,
            Support,
            Transmission_Line,
            UAS,
            UG_Network
        }

        public static Tabs GetTabByNameFoTab(String nameOfTab)
        {

            switch (nameOfTab)
            {
                case "Airboats":
                    return Tabs.Airboats;
                case "Damage Assessment":
                    return Tabs.Damage_Assessment;
                case "Distribution Line":
                    return Tabs.Distribution_Line;
                case "Distribution Veg Mgmt":
                    return Tabs.Distribution_Veg_Mgmt;
                case "Substation":
                    return Tabs.Substation;
                case "Support":
                    return Tabs.Support;
                case "Transmission Line":
                    return Tabs.Transmission_Line;
                case "UAS":
                    return Tabs.UAS;
                case "UG Network":
                    return Tabs.UG_Network;
                default:
                    throw new AssertionException("Abasent Tab for: " + nameOfTab);
            }

        }




    }
}
