using Microsoft.Playwright;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.PlaywrightPages
{
    /// <summary>
    /// Page Object for the Personal Details page
    /// </summary>
    public class PersonalDetailsPage
    {
        private readonly IPage _page;

        /// <summary>
        /// Locator for employee first name field
        /// </summary>
        private ILocator EmployeeFirstNameTxt => _page.Locator("[name='firstName']");

        /// <summary>
        /// Locator for employee last name field
        /// </summary>
        private ILocator EmployeeLastNameTxt => _page.Locator("[name='lastName']");

        /// <summary>
        /// Locator for My Info menu
        /// </summary>
        private ILocator MyInfoMenu => _page.Locator("//div[@class='oxd-sidepanel-body']//li[6]");

        /// <summary>
        /// Locator for page title
        /// </summary>
        private ILocator PageTitle => _page.Locator("//h6[text()='Personal Details']");

        /// <summary>
        /// Locator for employee ID field
        /// </summary>
        private ILocator EmployeeIdField => _page.Locator("//label[normalize-space()='Employee Id']/parent::div/following-sibling::div/input");

        /// <summary>
        /// Locator for other ID field
        /// </summary>
        private ILocator OtherIdTxt => _page.Locator("//label[normalize-space()='Other Id']/parent::div/following-sibling::div/input");

        /// <summary>
        /// Locator for driver license number field
        /// </summary>
        private ILocator DriverLicenseNumberTxt => _page.Locator("//label[contains(text(),'License Number')]/parent::div/following-sibling::div/input");

        /// <summary>
        /// Locator for save button
        /// </summary>
        private ILocator SaveBtn => _page.Locator("//button[@type='submit']");

        public PersonalDetailsPage(IPage page)
        {
            _page = page;
        }

        /// <summary>
        /// Navigate to the personal details page
        /// </summary>
        public async Task NavigateToAsync(string url)
        {
            await _page.GotoAsync(url);
            
            // Wait for page title to be visible
            await PageTitle.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 30000 });
        }

        /// <summary>
        /// Click on My Info menu
        /// </summary>
        public async Task ClickMyInfoMenuAsync()
        {
            await MyInfoMenu.ClickAsync();
            
            // Wait for navigation to complete
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        /// <summary>
        /// Check if page title is displayed
        /// </summary>
        public async Task<bool> IsAtAsync()
        {
            return await PageTitle.IsVisibleAsync();
        }

        /// <summary>
        /// Enter first name
        /// </summary>
        public async Task EnterFirstNameAsync(string? firstName)
        {
            await EmployeeFirstNameTxt.FillAsync(firstName ?? string.Empty);
        }

        /// <summary>
        /// Enter last name
        /// </summary>
        public async Task EnterLastNameAsync(string? lastName)
        {
            await EmployeeLastNameTxt.FillAsync(lastName ?? string.Empty);
        }

        /// <summary>
        /// Enter other ID
        /// </summary>
        public async Task EnterOtherIdAsync(string? otherId)
        {
            await OtherIdTxt.FillAsync(otherId ?? string.Empty);
        }

        /// <summary>
        /// Enter driver license number
        /// </summary>
        public async Task EnterDriverLicenseNumAsync(string? licenseNum)
        {
            await DriverLicenseNumberTxt.FillAsync(licenseNum ?? string.Empty);
        }

        /// <summary>
        /// Get first name value
        /// </summary>
        public async Task<string> GetFirstNameAsync()
        {
            return await EmployeeFirstNameTxt.GetAttributeAsync("value") ?? string.Empty;
        }

        /// <summary>
        /// Get last name value
        /// </summary>
        public async Task<string> GetLastNameAsync()
        {
            return await EmployeeLastNameTxt.GetAttributeAsync("value") ?? string.Empty;
        }

        /// <summary>
        /// Get other ID value
        /// </summary>
        public async Task<string> GetOtherIdAsync()
        {
            return await OtherIdTxt.GetAttributeAsync("value") ?? string.Empty;
        }

        /// <summary>
        /// Get driver license number value
        /// </summary>
        public async Task<string> GetDriverLicenseNumAsync()
        {
            return await DriverLicenseNumberTxt.GetAttributeAsync("value") ?? string.Empty;
        }

        /// <summary>
        /// Click save button
        /// </summary>
        public async Task ClickSaveAsync()
        {
            // Scroll down to make the button visible
            await _page.EvaluateAsync("window.scrollBy(0, 300)");
            
            // Wait for button to be clickable
            await SaveBtn.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
            
            // Click the button
            await SaveBtn.ClickAsync();
            
            // Wait for page to complete loading
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}
