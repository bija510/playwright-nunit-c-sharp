using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using C_Sharp_Selenium_NUnit.Config;
using C_Sharp_Selenium_NUnit.Data;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.IO;

namespace C_Sharp_Selenium_NUnit.BaseClass
{
    /// <summary>
    /// Base class for Playwright-based tests using PageTest from Microsoft.Playwright.NUnit
    /// </summary>
    public class PlaywrightBaseTest : PageTest
    {
        protected static TestData? pd;
        protected ExtentReports? extent;
        protected ExtentTest? test;

        // TestProfile configuration loaded from JSON
        protected static readonly TestProfile Config = ConfigReader.Load();
        private string environment = Config.Environment!;
        private string browser = Config.Browser!.ToUpper();

        protected string baseUrl = Config.BaseUrl!;
        protected string userName = Config.UserName!;
        protected string password = Config.Password!;

        /// <summary>
        /// Override context options to enable video recording
        /// </summary>
        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true,
                RecordVideoDir = Path.Combine(
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.")),
                    "Reports",
                    "Videos"
                ),
            };
        }

        /// <summary>
        /// Global setup runs once before all tests
        /// </summary>
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Get shared instance of ExtentReports
            extent = ExtentReportManager.GetInstance(browser, environment);

            TestContext.WriteLine($"[OneTimeSetUp] Environment: {environment}");
            TestContext.WriteLine($"[OneTimeSetUp] Base URL: {baseUrl}");
            TestContext.WriteLine($"[OneTimeSetUp] Browser: {browser}");

            var testDataReader = TestDataReader.Load();
            pd = testDataReader.pd;
        }

        /// <summary>
        /// Setup runs before each test
        /// </summary>
        [SetUp]
        public async Task Setup()
        {
            // Create a test node in ExtentReports for each test
            // Give full name in report: "C_Sharp_Selenium_NUnit.Tests.LoginTest.Login"
            test = extent?.CreateTest(TestContext.CurrentContext.Test.FullName);

            // Log to both console and ExtentReports
            Log($"Test started: {TestContext.CurrentContext.Test.Name}");
            test?.Info($"Browser {browser} initialized");
            
            // Browser context and page are automatically initialized by PageTest
            // Page is available as the 'Page' property inherited from PageTest
            
            // Optional: Set viewport size
            await Page.SetViewportSizeAsync(1920, 1080);
            Log($"Browser window set to 1920x1080");
        }

        /// <summary>
        /// TearDown runs after each test
        /// </summary>
        [TearDown]
        public async Task TearDown()
        {
            var testMethodName = TestContext.CurrentContext.Test.Name;
            var testOutcome = TestContext.CurrentContext.Result.Outcome.Status;
            var errorMessage = TestContext.CurrentContext.Result.Message;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;

            switch (testOutcome)
            {
                case NUnit.Framework.Interfaces.TestStatus.Passed:
                    test?.Pass(AventStack.ExtentReports.MarkupUtils.MarkupHelper
                        .CreateLabel("PASSED", AventStack.ExtentReports.MarkupUtils.ExtentColor.Green));
                    break;

                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    test?.Fail(errorMessage)
                        .Fail(stackTrace)
                        .Fail(AventStack.ExtentReports.MarkupUtils.MarkupHelper
                            .CreateLabel("FAILED", AventStack.ExtentReports.MarkupUtils.ExtentColor.Red));

                    try
                    {
                        string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                        string screenshotDir = Path.Combine(projectRoot, "Reports", "Screenshots");
                        Directory.CreateDirectory(screenshotDir);

                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string screenshotFile = $"{testMethodName}_{timestamp}.png";
                        string screenshotPath = Path.Combine(screenshotDir, screenshotFile);

                        // Take screenshot with Playwright
                        await Page.ScreenshotAsync(new() { Path = screenshotPath });
                        Log($"Screenshot saved to: {screenshotPath}");

                        // Use relative path for Extent report so it works in GitHub Pages
                        string relativePath = Path.Combine("Screenshots", screenshotFile).Replace("\\", "/");
                        test?.AddScreenCaptureFromPath(relativePath);
                    }
                    catch (Exception ex)
                    {
                        Log($"Failed to take screenshot: {ex.Message}");
                    }
                    break;

                default:
                    test?.Skip(AventStack.ExtentReports.MarkupUtils.MarkupHelper
                        .CreateLabel("SKIPPED / INCONCLUSIVE", AventStack.ExtentReports.MarkupUtils.ExtentColor.Orange));
                    break;
            }

            Log($"Test finished: {testMethodName} with status {testOutcome}");

            // PageTest automatically closes the browser context and page
            // No need to manually close the browser
        }

        /// <summary>
        /// Global teardown runs once after all tests
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ExtentReportManager.Flush();
            Log("ExtentReports flushed");
        }

        /// <summary>
        /// Helper method for logging
        /// </summary>
        protected void Log(string message)
        {
            Console.WriteLine($"[Playwright] {DateTime.Now:HH:mm:ss} - {message}");
        }
    }
}
