using NUnit.Allure.Core;
using WRMAutotests.PageObjects.Mobile.Android;
using WRMAutotests.Utility;
using WRMAutotests.Utility.Mobile;

namespace WRMAutotests.Tests.WebUI.Base
{
    [AllureNUnit]
    [TestFixture]
    public class BaseMobileTest : BaseTest
    {

        private MobileDriverUtils mobileDriverUtils = new MobileDriverUtils();
        public BaseInformation baseInformation = null;

        private static String defaultUrl = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithMobileConfigSettings, 0, 1);
        private static String defaultDeviceName = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithMobileConfigSettings, 1, 1);
        private static String defaultNameOfAppFile = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithMobileConfigSettings, 2, 1);
        private static String defaultNameOfApp = excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithMobileConfigSettings, 3, 1);
        private static String defaultFullPathToAppFile = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, @"Resources\", defaultNameOfAppFile);

        public BaseInformation GetAndroidBaseInfromation(String Url, String deviceName, String fullPathToApp)
        {
            return new BaseInformation(mobileDriverUtils.GetAndroidDriver(Url, deviceName, fullPathToApp));
        }

        public SelectWorkspaceScreen OpenAndroidApplication(BaseInformation baseInformation)
        {
            return new SelectWorkspaceScreen(baseInformation);
        }

        public SelectWorkspaceScreen OpenAndroidApplication()
        {
            BaseInformation baseInformation = GetAndroidBaseInfromation(defaultUrl, defaultDeviceName, defaultFullPathToAppFile);
            this.baseInformation = baseInformation;
            return OpenAndroidApplication(baseInformation);
        }

        public void RemoveInstalledAndroidApplication()
        {
            mobileDriverUtils.RemoveAppFromDevice(baseInformation.GetAndroidDriver(), defaultNameOfApp);
        }

    }
}
