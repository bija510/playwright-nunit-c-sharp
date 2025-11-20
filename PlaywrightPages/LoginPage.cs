using Microsoft.Playwright;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.PlaywrightPages
{
    /// <summary>
    /// Page Object for the Login page
    /// </summary>
    public class LoginPage
    {
        private readonly IPage _page;

        /// <summary>
        /// Locator for username input field
        /// </summary>
        private ILocator UserNameTxt => _page.Locator("//input[@placeholder='Username']");

        /// <summary>
        /// Locator for password input field
        /// </summary>
        private ILocator PasswordTxt => _page.Locator("//input[@placeholder='Password']");

        /// <summary>
        /// Locator for login button
        /// </summary>
        private ILocator LoginBtn => _page.Locator("//button[@type='submit']");

        /// <summary>
        /// Locator for dashboard label
        /// </summary>
        private ILocator DashboardLbl => _page.Locator("//h6[@class='oxd-text oxd-text--h6 oxd-topbar-header-breadcrumb-module']");

        public LoginPage(IPage page)
        {
            _page = page;
        }

        /// <summary>
        /// Navigate to the login page
        /// </summary>
        public async Task OpenLoginPageAsync(string url)
        {
            await _page.GotoAsync(url);
        }

        /// <summary>
        /// Perform login with provided credentials
        /// </summary>
        public async Task LoginAsync(string userName, string password)
        {
            await UserNameTxt.FillAsync(userName);
            await PasswordTxt.FillAsync(password);
            await LoginBtn.ClickAsync();
            
            // Wait for dashboard label to be visible
            await DashboardLbl.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 30000 });
            
            // Verify dashboard text is present
            var dashboardText = await DashboardLbl.TextContentAsync();
            if (!dashboardText?.Contains("Dashboard") ?? false)
            {
                throw new Exception("Dashboard label not found after login");
            }
        }
    }
}
