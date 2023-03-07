using Allure.Commons;
using WRMAutotests.Utility;
using WRMAutotests.Utility.Web;

namespace WRMAutotests.Tests
{

    abstract public class BaseTest
    {

        //Get Settings from ConfigDemo table
        public static ExcelReadedUtils excelReadedUtils = new ExcelReadedUtils("configDemo1.4.xlsx");
        private Boolean clearPreviousAllureResults = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithConfig, 9, 1);
        public Boolean makeScreensootForEveryReportStep = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithConfig, 16, 1);
        private Boolean isHeadlessModeEnabled = excelReadedUtils.GetBooleanCellValue(GlobalVariables.numberOfSheetWithConfig, 18, 1);
        private static String nameOfTimeZoneOFSite = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 19, 1);

        public String defaultContractorUrl = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 1);
        public String defaultUtilityUrl = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 1);

        public const int numberOfTryFroWebTests = 1;//when 1 - just 1 run, so no make sense
        public static int timeZoneDifferenceHours = (int) (TimeZoneInfo.ConvertTime(DateTime.Now, DateTimeUtils.GetTimezoneInfoByPartOfIdOfTimezone(nameOfTimeZoneOFSite)) - DateTime.Now).TotalHours;

        public User contractorUser = new User(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 2), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 3), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 2, 4));
        public User utilityUser = new User(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 2), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 3), excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 3, 4));

        // ---------------------------------------------------------------------------------------------------------
        // *** All connected with WEB PART ***
        //this need for multy thread test run
        //we use List instead value for situation when we need more than one driver for test
        private ThreadLocal<IList<BaseInformation>> baseInformations = new ThreadLocal<IList<BaseInformation>>();

        private DriverUtils driverUtils = new DriverUtils();

        public BaseInformation GetDefaultBaseInformation()
        {
            return GetBaseInformation()[0];
        }

        public IList<BaseInformation> GetBaseInformation()
        {
            if (baseInformations.Value == null)
                SetBaseInformation(new List<BaseInformation>());
            return baseInformations.Value;
        }

        public void SetBaseInformation(IList<BaseInformation> baseInfromations)
        {
            baseInformations.Value = baseInfromations;
        }

        public BaseInformation AddNewDriverWithDefaultSettings()
        {
            BaseInformation baseInformation = new BaseInformation(driverUtils.GenerateDefaultWebDriver(isHeadlessModeEnabled), makeScreensootForEveryReportStep);
            GetBaseInformation().Add(baseInformation);
            return baseInformation;
        }

        public void RemoveDriver(BaseInformation baseInformation)
        {
            baseInformations.Value.Remove(baseInformation);
            DriverUtils.CloseDriver(baseInformation.GetDriver());
        }
        // ---------------------------------------------------------------------------------------------------------

        //methods that need for read values from Excel data file
        public static String GetValueFromExcelOrUseDefaultValue(ExcelReadedUtils excelReadedUtils, int targetNumberOfSheet, int targetRow, int targetNumberOfColumnColumn, String defaultValue)
        {
            return excelReadedUtils.GetCellValue(targetNumberOfSheet, targetRow, targetNumberOfColumnColumn).Equals("") ? defaultValue : excelReadedUtils.GetCellValue(targetNumberOfSheet, targetRow, targetNumberOfColumnColumn);
        }

        public static RandomValuesUtilities GetRandomValuesUtilities()
        {
            String modeOfNameGeneration = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 10, 1);
            if (modeOfNameGeneration.Equals("Field"))
            {
                String newBaseRandomString = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithConfig, 10, 2)
                    .Trim()
                    .Replace(" ", "");
                return new RandomValuesUtilities(newBaseRandomString);
            }
            else
            {
                return new RandomValuesUtilities();
            }
        }

        [OneTimeSetUp]
        public void ClearResultsDir()
        {
            if (clearPreviousAllureResults)
            {
                AllureLifecycle.Instance.CleanupResultDirectory();
            }

        }


    }
}


public static class GlobalVariables
{
    //Variables for Excel file configDemo1.4.xlsx
    public readonly static int numberOfSheetWithConfig = 0;

    //// For Test case sheet of configDemo1.4.xlsx
    public readonly static int numberOfSheetWithTestCaseSettings = 1;
    public readonly static int numberOfColumnForOperatingCompanyForTestCaseSheet = 2;
    public readonly static int numberOfColumnForEventNameForTestCaseSheet = 3;
    public readonly static int numberOfColumnForDisciplineNameForTestCaseSheet = 4;
    public readonly static int numberOfColumnForInputFormForTestCaseSheet = 5;
    public readonly static int numberOfColumnForContinueParameterForTestCaseSheet = 6;
    public readonly static int numberOfColumnForClearDataForTestCaseSheet = 7;

    // For Mobile Config sheet of configDemo1.4.xlsx
    public readonly static int numberOfSheetWithMobileConfigSettings = 4;
}


