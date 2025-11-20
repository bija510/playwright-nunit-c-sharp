# Playwright NUnit C# Automation Framework

A modern test automation framework using Microsoft Playwright with NUnit, built with C# .NET 8.0 for cross-browser testing and reliable automation of web applications.

## 📋 Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Page Objects](#page-objects)
- [Test Examples](#test-examples)
- [Reporting](#reporting)
- [Troubleshooting](#troubleshooting)

## Overview

This framework provides:
- **Cross-browser testing** with Chromium, Firefox, and WebKit
- **NUnit integration** with Microsoft.Playwright.NUnit
- **Page Object Model (POM)** pattern for maintainable tests
- **Async/await patterns** for non-blocking test execution
- **ExtentReports** for detailed HTML test reports
- **Configuration management** for environment-specific settings
- **Test data management** with JSON-based data readers

## Prerequisites

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** (recommended) or Visual Studio Code
- **PowerShell** (for Playwright browser installation)
- **Administrator access** (for initial browser installation)

## Project Structure

```
playwright-nunit-c-sharp/
├── BaseClass/
│   └── PlaywrightBaseTest.cs          # Base test class with setup/teardown
├── Config/
│   ├── ConfigReader.cs                # Configuration reader utility
│   └── TestConfig.json                # Environment and browser configuration
├── Data/
│   ├── TestData.json                  # Test data repository
│   ├── TestDataReader.cs              # Data reader utility
│   └── TestReport.html                # Sample test report
├── PlaywrightPages/
│   ├── LoginPage.cs                   # Login page object
│   ├── RegisterPage.cs                # Register page object
│   ├── PersonalDetailsPage.cs         # Personal details page object
│   └── OtherDetailsPage.cs            # Other details page object
├── Tests/
│   ├── LoginTestPlaywright.cs         # Login test cases
│   ├── RegisterTestPlaywright.cs      # Registration test cases
│   ├── PersonalDetailsTestPlaywright.cs
│   └── OtherDetailsTestPlaywright.cs
├── Utils/
│   └── [Utility classes]              # Helper and utility methods
├── Reports/
│   └── [Test reports]                 # HTML test reports (generated)
├── playwright-nunit-c-sharp.csproj    # Project file
├── playwright-nunit-c-sharp.sln       # Solution file
├── nuget.config                       # NuGet configuration
└── README.md                          # This file
```

## Installation

### 1. Clone the Repository

```bash
git clone <repository-url>
cd playwright-nunit-c-sharp
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Install Playwright Browsers

```bash
# For Windows PowerShell
pwsh bin/Debug/net8.0/playwright.ps1 install

# Or use the dotnet CLI (after first build)
dotnet build
dotnet playwright install
```

### 4. Build the Project

```bash
dotnet build
```

## Configuration

### TestConfig.json

Update the configuration file for your environment:

```json
{
  "BaseURL": "https://your-application-url.com",
  "BrowserType": "chromium",
  "Headless": true,
  "Timeout": 30000,
  "SlowMo": 0,
  "ScreenshotOnFailure": true,
  "VideoOnFailure": false
}
```

**Configuration Options:**
- `BaseURL`: Target application URL
- `BrowserType`: Browser to use (chromium, firefox, webkit)
- `Headless`: Run browser in headless mode (true/false)
- `Timeout`: Default timeout in milliseconds
- `SlowMo`: Slow down actions by N milliseconds (for debugging)
- `ScreenshotOnFailure`: Capture screenshots on test failure
- `VideoOnFailure`: Record video on test failure

### TestData.json

Manage test data centrally:

```json
{
  "Users": [
    {
      "Username": "testuser@example.com",
      "Password": "SecurePassword123",
      "FirstName": "John",
      "LastName": "Doe"
    }
  ]
}
```

## Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Class

```bash
dotnet test --filter "LoginTestPlaywright"
```

### Run Tests by Category

```bash
# Run only sanity tests
dotnet test --filter "Category=Sanity"

# Run regression tests
dotnet test --filter "Category=Regression"
```

### Run with Specific Browser

```bash
# Set environment variable before running tests
$env:BROWSER_TYPE="firefox"
dotnet test
```

### Run with Verbose Output

```bash
dotnet test --logger "console;verbosity=detailed"
```

## Page Objects

### Base Page Object Pattern

All page objects inherit from a base structure:

```csharp
public class LoginPage
{
    private readonly IPage _page;

    // Locators as properties
    private ILocator UsernameField => _page.GetByLabel("Username");
    private ILocator PasswordField => _page.GetByLabel("Password");
    private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login" });

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task LoginAsync(string username, string password)
    {
        await UsernameField.FillAsync(username);
        await PasswordField.FillAsync(password);
        await LoginButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
```

### Available Page Objects

- **[`LoginPage`](PlaywrightPages/LoginPage.cs)** - User login functionality
- **[`RegisterPage`](PlaywrightPages/RegisterPage.cs)** - User registration
- **[`PersonalDetailsPage`](PlaywrightPages/PersonalDetailsPage.cs)** - Personal information management
- **[`OtherDetailsPage`](PlaywrightPages/OtherDetailsPage.cs)** - Additional user details

## Test Examples

### Login Test

```csharp
[TestFixture]
public class LoginTestPlaywright : PlaywrightBaseTest
{
    private LoginPage _loginPage;

    [SetUp]
    public async Task TestSetup()
    {
        _loginPage = new LoginPage(Page);
    }

    [Test]
    [Category("Sanity")]
    public async Task Login_ValidCredentials_Success()
    {
        await Page.GotoAsync(BaseURL);
        await _loginPage.LoginAsync("user@example.com", "password123");
        
        // Verify login success
        var dashboardLabel = Page.Locator("text=Dashboard");
        await Expect(dashboardLabel).ToBeVisibleAsync();
    }
}
```

### Registration Test

```csharp
[Test]
[Category("Sanity")]
public async Task TestFillNameAndNumber()
{
    await _registerPage.OpenAsync();
    await _registerPage.EnterFirstNameAsync("John");
    await _registerPage.EnterLastNameAsync("Doe");
    await _registerPage.EnterEmailAsync("john.doe@example.com");
    await _registerPage.EnterPhoneAsync("1234567890");
    
    Assert.Pass("Form filled successfully");
}
```

## Reporting

### ExtentReports Integration

Reports are automatically generated during test execution and saved to the `Reports/` directory.

**Report Features:**
- Test execution timeline
- Pass/fail status with details
- Browser and environment information
- Screenshots on failure
- Video recordings (if enabled)
- Custom logs and markers

### View Reports

Open the generated HTML report:

```bash
# Windows
start Reports/Report_*.html

# macOS
open Reports/Report_*.html

# Linux
xdg-open Reports/Report_*.html
```

## Key Features

### ✨ Built-in Waiting Mechanisms
- Automatic waiting for elements
- Network idle detection
- DOM stability checks
- No explicit waits needed

### 🔄 Async/Await Pattern
- Non-blocking test execution
- Better performance and reliability
- Cleaner, more readable code

### 📊 Comprehensive Reporting
- ExtentReports integration
- Screenshots on failure
- Video recording capability
- Detailed execution logs

### 🎯 Semantic Locators
- `GetByRole()` - Find by accessibility role
- `GetByLabel()` - Find by associated label
- `GetByPlaceholder()` - Find by placeholder text
- `GetByText()` - Find by visible text
- `Locator()` - CSS/XPath selectors

### 🌐 Multi-browser Support
- Chromium
- Firefox
- WebKit

## Best Practices

1. **Use Semantic Locators**: Prefer `GetByRole()`, `GetByLabel()` over CSS selectors
2. **Async/Await**: Always use `async/await` for page interactions
3. **Explicit Waits**: Let Playwright handle waiting automatically
4. **Page Object Model**: Keep page logic separate from test logic
5. **Data Management**: Use TestData.json for test data
6. **Assertions**: Use `Expect()` for reliable assertions
7. **Error Handling**: Implement proper try-catch blocks with cleanup

## Troubleshooting

### Issue: Browser Installation Fails

**Solution:**
```bash
# Clear playwright cache
rm -r ~/.cache/ms-playwright

# Reinstall browsers
dotnet playwright install
```

### Issue: Tests Timeout

**Solution:**
- Increase timeout in `TestConfig.json`
- Check network connectivity
- Verify application is running
- Check if selectors are correct

### Issue: Element Not Found

**Solution:**
- Verify selector/locator is correct
- Add appropriate waits: `WaitForSelectorAsync()`
- Use browser DevTools to inspect element
- Check if element is in iframe

### Issue: Tests Pass Locally but Fail in CI/CD

**Solution:**
- Use `headless: true` in CI/CD
- Set appropriate timeouts
- Ensure environment variables are set
- Check browser installation in pipeline

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.Playwright | 1.48.0 | Core Playwright library |
| Microsoft.Playwright.NUnit | 1.48.0 | NUnit integration |
| NUnit | 4.3.2 | Test framework |
| NUnit3TestAdapter | 5.2.0 | NUnit test adapter |
| ExtentReports | 5.0.4 | HTML reporting |
| Newtonsoft.Json | 13.0.3 | JSON parsing |

## Contributing

1. Create a feature branch: `git checkout -b feature/my-feature`
2. Commit changes: `git commit -am 'Add new feature'`
3. Push to branch: `git push origin feature/my-feature`
4. Submit pull request

## License

See [LICENSE](LICENSE) file for details.

## Support

For issues, questions, or contributions, please open an issue in the repository.

## Additional Resources

- [Playwright Documentation](https://playwright.dev)
- [Playwright C# API](https://playwright.dev/dotnet/)
- [NUnit Documentation](https://docs.nunit.org/)
- [ExtentReports](https://www.extentreports.com/)

---

**Last Updated:** November 2024
**Framework Version:** 1.0.0
**Target Framework:** .NET 8.0