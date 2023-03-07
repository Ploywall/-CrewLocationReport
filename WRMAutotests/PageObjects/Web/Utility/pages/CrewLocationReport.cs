using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WRMAutotests.PageObjects.Web.Base;
using WRMAutotests.PageObjects.Web.Utility.panel;
using WRMAutotests.PageObjects.Web.Utility.windows;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Web.Utility.pages
{
    public class CrewLocationReport : BaseLoggedPage
    {
        [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_tbReportViews_T2T")] 
        private IWebElement splitButton;
        [FindsBy(How = How.CssSelector, Using = "#TopPanel_gluEvents_B-1")]
        private IWebElement pinInMapButton;

        [FindsBy(How = How.CssSelector, Using = "tr.dxgvDataRow_DevEx,tr.dxgvSelectedRow_DevEx")]
        private IList<IWebElement> rows;

        [FindsBy(How = How.CssSelector, Using = "div.dxgvPagerBottomPanel_DevEx")]
        private IWebElement paginationBarRootElement;

        [FindsBy(How = How.CssSelector, Using = "ul.dxtc-strip > li.dxtc-tab")]
        private IList<IWebElement> tabs;

        [FindsBy(How = How.CssSelector, Using = "ul.dxtc-strip > li.dxtc-activeTab")]
        private IList<IWebElement> activeTabs;

        [FindsBy(How = How.CssSelector, Using = "ul.dxtc-strip dxtc-stripContainer > li.dxtc-tab")]
        private IList<IWebElement> tabSplit; 

        [FindsBy(How = How.CssSelector, Using = "ul.dxtc-strip dxtc-stripContainer > li.dxtc-activeTab")]
        private IList<IWebElement> activeTabSplit;

        public CrewLocationReport(BaseInformation baseInformation) : base(baseInformation, new ReportUtils(baseInformation, "Crew Location Report", "page"))
        {

        }

        public CrewLocationReport GetCrewLocationReport()
        {
            Thread.Sleep(5000);
            return new CrewLocationReport(GetBaseInformation());
        }

        public int GetNumberOFRowsByCrewSheetFromAllPages(String crewSheet)
        {
            GetPaginationSubPanel().ClickFirstPage();
            int result = 0;
            result += GetRowsByCrewSheet(crewSheet).Count;
            while (GetPaginationSubPanel().IsNextButtonEnabled())
            {
                GetPaginationSubPanel().ClickNextButton();
                result += GetRowsByCrewSheet(crewSheet).Count;
            }
            return result;

        }

        public PaginationSubPanel GetPaginationSubPanel()
        {
            return new PaginationSubPanel(GetBaseInformation(), paginationBarRootElement);
        }

         public IList<Row> GetRowsByCrewSheet(String crewSheet)
        {
            return GetRows().Where(row => row.GetCrewSheet().Equals(crewSheet)
            ).ToList();
        }

        public IList<Row> GetRows()
        {
            IList<Row> result = new List<Row>();
            foreach (IWebElement element in rows)
            {
                GetWebElementUtils().ScrollToElement(element);
                result.Add(new Row(GetBaseInformation(), element));
            }
            return result;
        }


        public class Row : BasePageObject
        {
            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col0")]
            private IWebElement company;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col1")]
            private IWebElement crewSheet;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col2")]
            private IWebElement resources;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col3")]
            private IWebElement crew;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col4")]
            private IWebElement supervisor;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col5")]
            private IWebElement currentStatus;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col6")]
            private IWebElement currentLocation;

            [FindsBy(How = How.CssSelector, Using = "#ASPxPanel2_ContentPlaceHolder1_cpnAction_gvDetail_Split_col7")]
            private IWebElement pinInMap;


            public Row(BaseInformation baseInformation, IWebElement rootElement) : base(baseInformation, rootElement, new ReportUtils(baseInformation, "Secured Workforce", "row"))
            {
            }


            public String GetCompany()
            {
                GetWebElementUtils().ScrollToElement(company);
                return company.GetAttribute("innerHTML").Trim();
            }

            public String GetCrewSheet()
            {
                return crewSheet.Text;
            }
            public String GetResources()
            {
                GetWebElementUtils().ScrollToElement(resources);
                return resources.GetAttribute("innerHTML").Trim();
            }

            public String GetCrew()
            {
                GetWebElementUtils().ScrollToElement(crew);
                return crew.GetAttribute("innerHTML").Trim();
            }

            public String GetSupervisor()
            {
                GetWebElementUtils().ScrollToElement(supervisor);
                return supervisor.GetAttribute("innerHTML").Trim();
            }

            public String GetCurrentStatus()
            {
                GetWebElementUtils().ScrollToElement(currentStatus);
                return currentStatus.GetAttribute("innerHTML").Trim();
            }

            public String GetCurrentLocation()
            {
                GetWebElementUtils().ScrollToElement(currentLocation);
                return currentLocation.GetAttribute("innerHTML").Trim();
            }

            public void ClickPinInMap()
            {
                GetReportUtils().ClickButton("Pin In Map");
                Thread.Sleep(1000);
                pinInMap.Click();
                Thread.Sleep(1000);
            }
        }

        public CrewLocationReport ClickTab(Tabs tab)
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

//----------------------------------------------------------
        public CrewLocationReport ClickTabSplit(TabSplit tabSplit)
        {
            Thread.Sleep(1000);
            switch (tabSplit)
            {
                case TabSplit.Map:
                    {
                        ClickTabSplitByName("Map");
                        break;
                    }
                case TabSplit.Table:
                    {
                        ClickTabSplitByName("Table");
                        break;
                    }
                case TabSplit.Split:
                    {
                        ClickTabSplitByName("Split");
                        break;
                    }
            }
            return this;
        }

        public enum TabSplit
        {
            Map,
            Table,
            Split
        }

         public static TabSplit GetTabByNameFoTabSplit(String nameOfTab)
        {

            switch (nameOfTab)
            {
                case "Map":
                    return TabSplit.Map;
                case "Table":
                    return TabSplit.Table;
                case "Split":
                    return TabSplit.Split;
                default:
                    throw new AssertionException("Abasent Tab for: " + nameOfTab);
            }

        }

         private void ClickTabSplitByName(String tabName)
        {
            GetReportUtils().ClickButton("Tab: " + tabName);
            IWebElement tabButton = null;
            try
            {
                tabButton = tabSplit.Where(r => r.FindElement(By.CssSelector("a > span")).Text.Equals(tabName)).First();
            }
            catch (Exception ex)
            {
                //checked that we already on expected tab or not
                if (activeTabSplit.Where(r => r.FindElement(By.CssSelector("a > span")).Text.Equals(tabName)).ToList().Count > 0)
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
    }
}
