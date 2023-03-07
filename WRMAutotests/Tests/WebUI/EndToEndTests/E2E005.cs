using MimeKit;
using Newtonsoft.Json;
using RetryOnException;
using WRMAutotests.PageObjects.Web.Contractor.pages;
using WRMAutotests.PageObjects.Web.Contractor.panels;
using WRMAutotests.PageObjects.Web.Utility.pages;
using WRMAutotests.Utility;

namespace WRMAutotests.Tests.WebUI.EndToEndTests
{
    public class E2E005 : BaseWebEndToEndTest
    {

        private static int numberOfRowForCurrentTestCase = 7;
        private String operatingCompanyName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForOperatingCompanyForTestCaseSheet, defaultOperatingCompany);
        private static String eventName = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForEventNameForTestCaseSheet, defaultEvent);
        private static String discipline = GetValueFromExcelOrUseDefaultValue(excelReadedUtils, GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForDisciplineNameForTestCaseSheet, defaultDiscipline);

        private String subject = GetRandomValuesUtilities().GetRandomValue();
        private String message = GetRandomValuesUtilities().GetRandomValue();
        private String contractor = JsonConvert.DeserializeObject<ContinueParameterSettings>(excelReadedUtils.GetCellValue(GlobalVariables.numberOfSheetWithTestCaseSettings, numberOfRowForCurrentTestCase, GlobalVariables.numberOfColumnForContinueParameterForTestCaseSheet)).contractorName;
        [SetUp]
        public void createCrewAvailabilityRequest()
        {
            BaseInformation baseInformation = AddNewDriverWithDefaultSettings();
            ManageSecuredWorkforcePage manageSecuredWorkforcePage = GetActionsForEndToEndTests().LoginToUtilityWithSettingsFromExcel(excelReadedUtils, utilityUser, 7);
            GetActionsForEndToEndTests().CreateCrewAvailabilityRequest(manageSecuredWorkforcePage, operatingCompanyName, contractor, subject, message, discipline);
            RemoveDriver(baseInformation);
        }


        [RetryOnException(ListOfExceptions = new[] { typeof(Exception) })]
        [Retry(numberOfTryFroWebTests)]
        [Test]
        public void E2E005_Test()
        {
            MainPage mainPage = GetActionsForEndToEndTests().LoginUntoDefaultContractor(contractorUser);
            MessagePanel messagePanel = mainPage.GetHeaderPanel()
                .ClickMessagesButoon()
                .GetMessageCenterPanel()
                .GetEmailRowsByPartSubject(subject)[0]
                .ClickSubject();

            messagePanel.GetAssertionUtils().TrueAssertion("Verify that Message contain expected text: " + message, messagePanel.GetMessageText().Contains(message));

            //check emails
            Thread.Sleep(60000);
            MailRepository mailRepository = new MailRepository(contractorUser);
            IList<MimeMessage> foundEmails = mailRepository.GetUnreadEmailsByPartOfSubjectAndReciveEmails(subject, contractorUser.GetEmail());
            messagePanel.GetAssertionUtils().TrueAssertionWithoutNameOfPageObject("Verify that Expected Email present in the Inbox", foundEmails.Count > 0);
            messagePanel.GetAssertionUtils().TrueAssertionWithoutNameOfPageObject("Verify that Expected Email letter contain expected message: " + message, foundEmails[0].HtmlBody.Contains(message));
        }

        private class ContinueParameterSettings
        {
            public string contractorName;
        }

    }
}
