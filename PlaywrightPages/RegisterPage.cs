using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.PlaywrightPages
{
    /// <summary>
    /// Page Object for the Register page
    /// </summary>
    public class RegisterPage
    {
        private readonly IPage _page;
        private const string Url = "https://demo.automationtesting.in/Register.html";

        /// <summary>
        /// Locator for first name input
        /// </summary>
        private ILocator FirstNameTxt => _page.Locator("input[placeholder='First Name']");

        /// <summary>
        /// Locator for last name input
        /// </summary>
        private ILocator LastNameTxt => _page.Locator("input[placeholder='Last Name']");

        /// <summary>
        /// Locator for address textarea
        /// </summary>
        private ILocator AddressTxt => _page.Locator("textarea");

        /// <summary>
        /// Locator for email input
        /// </summary>
        private ILocator EmailTxt => _page.Locator("input[type='email']");

        /// <summary>
        /// Locator for phone input
        /// </summary>
        private ILocator PhoneTxt => _page.Locator("input[type='tel']");

        /// <summary>
        /// Locator for male gender radio button
        /// </summary>
        private ILocator GenderMaleRdo => _page.Locator("input[value='Male']");

        /// <summary>
        /// Locator for female gender radio button
        /// </summary>
        private ILocator GenderFemaleRdo => _page.Locator("input[value='FeMale']");

        /// <summary>
        /// Locator for hobbies checkboxes
        /// </summary>
        private ILocator HobbiesCheckboxes => _page.Locator("input[type='checkbox']");

        /// <summary>
        /// Locator for skills dropdown
        /// </summary>
        private ILocator SkillsDropdown => _page.Locator("//select[@id='Skill']");

        /// <summary>
        /// Locator for password input
        /// </summary>
        private ILocator PasswordTxt => _page.Locator("#firstpassword");

        /// <summary>
        /// Locator for confirm password input
        /// </summary>
        private ILocator ConfirmPasswordTxt => _page.Locator("#secondpassword");

        /// <summary>
        /// Locator for submit button
        /// </summary>
        private ILocator SubmitBtn => _page.Locator("#submitbtn");

        public RegisterPage(IPage page)
        {
            _page = page;
        }

        /// <summary>
        /// Navigate to the register page
        /// </summary>
        public async Task OpenAsync()
        {
            await _page.GotoAsync(Url);
        }

        /// <summary>
        /// Enter first name
        /// </summary>
        public async Task EnterFirstNameAsync(string firstName)
        {
            await FirstNameTxt.FillAsync(firstName);
        }

        /// <summary>
        /// Enter last name
        /// </summary>
        public async Task EnterLastNameAsync(string lastName)
        {
            await LastNameTxt.FillAsync(lastName);
        }

        /// <summary>
        /// Enter address
        /// </summary>
        public async Task EnterAddressAsync(string address)
        {
            await AddressTxt.FillAsync(address);
        }

        /// <summary>
        /// Enter email
        /// </summary>
        public async Task EnterEmailAsync(string email)
        {
            await EmailTxt.FillAsync(email);
        }

        /// <summary>
        /// Enter phone number
        /// </summary>
        public async Task EnterPhoneAsync(string phone)
        {
            await PhoneTxt.FillAsync(phone);
        }

        /// <summary>
        /// Select gender (Male or Female)
        /// </summary>
        public async Task SelectGenderAsync(string gender)
        {
            if (gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
            {
                await GenderMaleRdo.ClickAsync();
            }
            else if (gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
            {
                await GenderFemaleRdo.ClickAsync();
            }
            else
            {
                throw new ArgumentException($"Invalid gender value: {gender}");
            }
        }

        /// <summary>
        /// Check hobbies checkbox
        /// </summary>
        public async Task CheckHobbiesCheckboxAsync()
        {
            // Check the first checkbox found
            await HobbiesCheckboxes.First.CheckAsync();
        }

        /// <summary>
        /// Select dropdown by text label
        /// </summary>
        public async Task SelectDropDownLabelAsync(string label)
        {
            await SkillsDropdown.SelectOptionAsync(new SelectOptionValue { Label = label });
        }

        /// <summary>
        /// Enter password
        /// </summary>
        public async Task EnterPasswordAsync(string password)
        {
            await PasswordTxt.FillAsync(password);
        }

        /// <summary>
        /// Enter confirm password
        /// </summary>
        public async Task EnterConfirmPasswordAsync(string password)
        {
            await ConfirmPasswordTxt.FillAsync(password);
        }

        /// <summary>
        /// Submit the form
        /// </summary>
        public async Task SubmitAsync()
        {
            await SubmitBtn.ClickAsync();
        }
    }
}
