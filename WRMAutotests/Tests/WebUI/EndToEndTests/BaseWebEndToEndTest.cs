using WRMAutotests.Tests.WebUI.Actions.Web;
using WRMAutotests.Tests.WebUI.Base;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class BaseWebEndToEndTest : BaseWebTest
    {
        public static String defaultDiscipline = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 12, 1);
        public static String defaultOperatingCompany = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 14, 1);
        public static String defaultEvent = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 15, 1);
        public static String defaultSourceLocation = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 17, 1);
        public static Boolean checkIouCheckbox = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithConfig, 11, 1);

        public ActionsForEndToEndTests GetActionsForEndToEndTests()
        {
            return new ActionsForEndToEndTests(GetDefaultBaseInformation(), defaultContractorUrl, defaultUtilityUrl);
        }

    }
}
