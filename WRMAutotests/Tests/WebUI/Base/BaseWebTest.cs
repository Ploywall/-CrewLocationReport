using NUnit.Allure.Core;
using WRMAutotests.Tests.StabilityScript;
using WRMAutotests.Utility;
using WRMAutotests.Utility.Web;

[assembly: LevelOfParallelism(4)]

namespace WRMAutotests.Tests.WebUI.Base
{
    [Parallelizable(scope: ParallelScope.All)]
    [AllureNUnit]
    [TestFixture]
    public class BaseWebTest : BaseTest
    {

        [SetUp]
        public void SetUpDriver()
        {
            //create 1 default driver because in any case for any web test we should have at least 1 driver
            SetBaseInformation(new List<BaseInformation>());
            AddNewDriverWithDefaultSettings();
        }

        [TearDown]
        public void CloseDrive()
        {
            foreach (BaseInformation baseInformation in GetBaseInformation())
            {
                //add screeshoot after finishing test
                try
                {
                    ReportUtils.MakeScreenshoot(baseInformation);
                }
                catch (Exception ex)
                {
                    //need just for ingnorning Exceptions when we try to make screenshoot
                }

                //close driver
                DriverUtils.CloseDriver(baseInformation.GetDriver());
            }

        }

    }
}

[SetUpFixture]
public class RootFixtureSetup
{
    private static ExcelReadedUtils excelReadedUtils = new ExcelReadedUtils("configDemo1.4.xlsx");
    private Boolean isHeadlessModeEnabled = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithConfig, 18, 1);
    private Boolean runWarmUpScripts = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithConfig, 7, 1);
    private String defaultContractorUrl = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 1);
    private String defaultUtilityUrl = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 1);
    private User contractorUser = new User(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 2), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 3), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 4));
    private User utilityUser = new User(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 2), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 3), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 4));

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {

        DriverUtils driverUtils = new DriverUtils();

        if (runWarmUpScripts)
        {
            {
                BaseInformation baseInformation = new BaseInformation(driverUtils.GenerateDefaultWebDriver(isHeadlessModeEnabled));
                try
                {
                    StabilityScripts.DEV_1_MT_ContractorAllMenu(baseInformation, defaultContractorUrl, contractorUser);
                }
                finally
                {
                    if (baseInformation.GetDriver() != null)
                    {
                        baseInformation.GetDriver().Close();
                    }
                }
            }
            {
                BaseInformation baseInformation = new BaseInformation(driverUtils.GenerateDefaultWebDriver(isHeadlessModeEnabled));
                try
                {
                    StabilityScripts.DEV_2_NG_SCS_ALL_MENU(baseInformation, defaultUtilityUrl, utilityUser);
                }
                finally
                {
                    if (baseInformation.GetDriver() != null)
                    {
                        baseInformation.GetDriver().Close();
                    }
                }
            }
        }

    }



}


