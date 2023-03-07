using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using WRMAutotests.PageObjects.Web.Base;
using WRMAutotests.PageObjects.Web.Utility.panel;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Web.Contractor.windows.crewavailabilitywindow
{
    public class ResourcePoolTablePanel : BasePageObject
    {

        [FindsBy(How = How.CssSelector, Using = "tr.dxgvDataRow_DevEx")]
        private IList<IWebElement> rows;

        [FindsBy(How = How.CssSelector, Using = "div.dxgvPagerBottomPanel_DevEx")]
        private IWebElement paginationBarRootElement;

        private static String nameOfPageObject = "Resource Pool table on Crew availability form window";
        private static String typeOfPageObject = "panel";

        public ResourcePoolTablePanel(BaseInformation baseInformation, IWebElement rootElement) : base(baseInformation, rootElement, new ReportUtils(baseInformation, nameOfPageObject, typeOfPageObject))
        {
        }

        public PaginationSubPanel GetPaginationSubPanel()
        {
            return new PaginationSubPanel(GetBaseInformation(), paginationBarRootElement);
        }

        public IList<Row> GetRows()
        {
            IList<Row> result = new List<Row>();
            foreach (IWebElement row in rows)
            {
                result.Add(new Row(GetBaseInformation(), row));
            }
            return result;
        }

        public Row GetRowByResourcePoolFromAnyPage(String resourcePool)
        {
            GetPaginationSubPanel().ClickFirstPage();
            if(GetRowsByResourcePool(resourcePool).Count == 0)
            {
                throw new AssertionException(nameOfPageObject + " " + typeOfPageObject + " is empty");
            }
            while (GetPaginationSubPanel().IsNextButtonEnabled())
            {
                IList<Row> foundRows = GetRowsByResourcePool(resourcePool);
                if (foundRows.Count > 0)
                    return foundRows[0];
                GetPaginationSubPanel().ClickNextButton();
            }
            {
                IList<Row> foundRows = GetRowsByResourcePool(resourcePool);
                if (foundRows.Count > 0)
                    return foundRows[0];
            }
            throw new AssertionException("Row with resource pool: " + resourcePool + " absent");

        }

        public IList<Row> GetRowsByResourcePool(String resourcePool)
        {
            return GetRows().Where(r => r.GetResourcePool().Equals(resourcePool)).ToList();
        }

        public class Row : BasePageObject
        {

            [FindsBy(How = How.CssSelector, Using = "span.dxICheckBox_DevEx")]
            private IWebElement selectorCheckbox;

            [FindsBy(How = How.CssSelector, Using = "a[title='View the Information']")]
            private IWebElement resorcePool;

            public Row(BaseInformation baseInformation, IWebElement rootElement) : base(baseInformation, rootElement, new ReportUtils(baseInformation, "Resource Pool on Crew Availability Form", "row"))
            {
            }

            public void ClickCheckbox()
            {
                GetReportUtils().ClickButton("Checkbox");
                GetWebElementUtils().clickWebElement(selectorCheckbox);
                Thread.Sleep(3000);
            }

            public String GetResourcePool()
            {
                return resorcePool.Text;
            }

        }


    }
}
