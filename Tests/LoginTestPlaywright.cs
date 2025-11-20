using AventStack.ExtentReports.MarkupUtils;
using C_Sharp_Selenium_NUnit.BaseClass;
using C_Sharp_Selenium_NUnit.PlaywrightPages;
using NUnit.Framework;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.Tests.Playwright
{
    /// <summary>
    /// Playwright-based Login tests
    /// </summary>
    [TestFixture]
    public class LoginTestPlaywright : PlaywrightBaseTest
    {
        [Test]
        [Category("Smoke")]
        public async Task Login()
        {
            test!.Info(MarkupHelper.CreateLabel("Verify Login successfully", ExtentColor.Blue));
            
            var loginPage = new LoginPage(Page);
            await loginPage.OpenLoginPageAsync(baseUrl);

            // Test valid credentials
            await loginPage.LoginAsync(userName, password);
            
            // Verify URL changed (optional additional verification)
            Assert.That(Page.Url, Does.Contain("dashboard"), "Dashboard URL should contain 'dashboard'");
        }
    }
}
