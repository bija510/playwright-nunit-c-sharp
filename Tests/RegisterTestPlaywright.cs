using AventStack.ExtentReports.MarkupUtils;
using C_Sharp_Selenium_NUnit.BaseClass;
using C_Sharp_Selenium_NUnit.PlaywrightPages;
using NUnit.Framework;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.Tests.Playwright
{
    /// <summary>
    /// Playwright-based Register tests
    /// </summary>
    [TestFixture]
    public class RegisterTestPlaywright : PlaywrightBaseTest
    {
        private RegisterPage? _registerPage;

        [SetUp]
        public async Task TestSetup()
        {
            // Page is already initialized by PlaywrightBaseTest
            _registerPage = new RegisterPage(Page);
        }

        [Test]
        [Category("Sanity")]
        public async Task TestFillNameAndNumber()
        {
            test!.Info(MarkupHelper.CreateLabel("Test fill name and number", ExtentColor.Blue));
            
            await _registerPage!.OpenAsync();
            await _registerPage.EnterFirstNameAsync("John");
            await _registerPage.EnterLastNameAsync("Doe");
            await _registerPage.EnterAddressAsync("123 Main St");
            await _registerPage.EnterEmailAsync("john.doe@example.com");
            await _registerPage.EnterPhoneAsync("1234567890");
            
            Assert.Pass("Form filled successfully");
        }

        [Test]
        [Category("Regression")]
        public async Task TestFillAgeAndHobbies()
        {
            test!.Info(MarkupHelper.CreateLabel("Test fill age and hobbies", ExtentColor.Blue));
            
            await _registerPage!.OpenAsync();
            await _registerPage.SelectGenderAsync("Male");
            await _registerPage.CheckHobbiesCheckboxAsync();
            await _registerPage.SelectDropDownLabelAsync("CSS");
            
            Assert.Pass("Form filled successfully");
        }

        [Test]
        [Category("Sanity")]
        public async Task TestFillPasswordAndSubmit()
        {
            test!.Info(MarkupHelper.CreateLabel("Test fill password and submit", ExtentColor.Blue));
            
            await _registerPage!.OpenAsync();
            await _registerPage.EnterPasswordAsync("Password123!");
            await _registerPage.EnterConfirmPasswordAsync("Password123!");
            await _registerPage.SubmitAsync();

            Assert.Pass("Form submitted successfully");
        }
    }
}
