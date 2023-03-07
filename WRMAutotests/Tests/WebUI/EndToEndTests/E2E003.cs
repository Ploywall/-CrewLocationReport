using MimeKit;
using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.Utility;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E003 : BaseWebEndToEndTest
    {

        private static int numberOfRowForCurrentTestCase = 5;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);

        private static InputFormSettings settingsInputForm = JsonConvert.DeserializeObject<InputFormSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForInputFormForTestCaseSheet));
        private static ContinueParameterSettings settingsContinueParameter = JsonConvert.DeserializeObject<ContinueParameterSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet));

        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E003_Test()
        {
            String contractor = settingsContinueParameter.contractorName;
            String subject = settingsInputForm.subject + GetRandomValuesUtilities().GetRandomValue();
            String message = settingsInputForm.massage + GetRandomValuesUtilities().GetRandomValue();
            message = message.Replace("\n", "").Replace("\t", "").Trim();

            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 5);
            GetActionsForEndToEndTests().CreateCrewAvailabilityRequest(manageSecuredWorkforcePage, operatingCompanyName, contractor, subject, message, discipline);

            Thread.Sleep(60000);
            MailRepository mailRepository = new MailRepository(contractorUser);
            IList<MimeMessage> foundEmails = mailRepository.GetUnreadEmailsByPartOfSubjectAndReciveEmails(subject, contractorUser.GetEmail());

            manageSecuredWorkforcePage.GetAssertionUtils().TrueAssertionWithoutNameOfPageObject("Verify that Expected Email present in the Inbox", foundEmails.Count > 0);
            manageSecuredWorkforcePage.GetAssertionUtils().TrueAssertionWithoutNameOfPageObject("Verify that Expected Email letter contain expected message: " + message, foundEmails[0].HtmlBody.Contains(message));

        }


        private class InputFormSettings
        {
            public string subject;
            public string massage;
            public string Utility;
        }

        private class ContinueParameterSettings
        {
            public string contractorName;
        }

    }
}
