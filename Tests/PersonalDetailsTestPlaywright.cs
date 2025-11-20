using AventStack.ExtentReports.MarkupUtils;
using C_Sharp_Selenium_NUnit.BaseClass;
using C_Sharp_Selenium_NUnit.Data;
using C_Sharp_Selenium_NUnit.PlaywrightPages;
using NUnit.Framework;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.Tests.Playwright
{
    /// <summary>
    /// Playwright-based Personal Details tests
    /// </summary>
    [TestFixture]
    public class PersonalDetailsTestPlaywright : PlaywrightBaseTest
    {
        private PersonalDetailsPage? _personalDetailsPage;
        private LoginPage? _loginPage;

        [SetUp]
        public async Task TestSetup()
        {
            // Page is already initialized by PlaywrightBaseTest
            _loginPage = new LoginPage(Page);
            _personalDetailsPage = new PersonalDetailsPage(Page);
        }

        [Test]
        [Category("Regression")]
        public async Task EditName_SaveAndVerify()
        {
            // Highlighted step (bold + colored label)
            test!.Info(MarkupHelper.CreateLabel("Verify Login successfully", ExtentColor.Blue));

            await _loginPage!.OpenLoginPageAsync(baseUrl);

            // Login to application
            await _loginPage.LoginAsync(userName, password);

            // Highlighted step (bold + colored label)
            test.Info(MarkupHelper.CreateLabel("Verify personal details page", ExtentColor.Blue));

            // Enter personal details
            await _personalDetailsPage!.ClickMyInfoMenuAsync();
            
            await _personalDetailsPage.EnterFirstNameAsync(pd!.FirstName);
            var firstName = await _personalDetailsPage.GetFirstNameAsync();
            Assert.That(firstName, Does.Contain(pd.FirstName), "First name should be entered correctly");

            await _personalDetailsPage.EnterLastNameAsync(pd.LastName);
            var lastName = await _personalDetailsPage.GetLastNameAsync();
            Assert.That(lastName, Does.Contain(pd.LastName), "Last name should be entered correctly");

            await _personalDetailsPage.EnterOtherIdAsync(pd.OtherId);
            var otherId = await _personalDetailsPage.GetOtherIdAsync();
            Assert.That(otherId, Does.Contain(pd.OtherId), "Other ID should be entered correctly");

            await _personalDetailsPage.EnterDriverLicenseNumAsync(pd.DriverLicenseNum);
            var driverLicense = await _personalDetailsPage.GetDriverLicenseNumAsync();
            Assert.That(driverLicense, Does.Contain(pd.DriverLicenseNum), "Driver license should be entered correctly");

            // Save the changes
            await _personalDetailsPage.ClickSaveAsync();
            
            test.Pass("Personal details saved successfully");
        }
    }
}
