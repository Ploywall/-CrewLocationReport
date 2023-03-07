using WRMAutotests.PageObjects.Web.Contractor.panels;
using WRMAutotests.Utility;

namespace WRMAutotests.PageObjects.Web.Contractor.pages
{
    public class ResourcePoolEditOrganizationPage : BaseLoggedPage
    {


        public ResourcePoolEditOrganizationPage(BaseInformation baseInformation) : base(baseInformation, new ReportUtils(baseInformation, "Resource Pool - Edit Organization", "page"))
        {
        }

        public ResourcePoolEditOrganizationPanel GetResourcePoolEditOrganizationPanel()
        {
            return new ResourcePoolEditOrganizationPanel(GetBaseInformation());
        }


    }
}
