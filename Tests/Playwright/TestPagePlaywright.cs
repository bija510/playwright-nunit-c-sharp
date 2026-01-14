using AventStack.ExtentReports.MarkupUtils;
using C_Sharp_Selenium_NUnit.BaseClass;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.Tests.Playwright
{
    [TestFixture]
    public class TestPagePlaywright : PlaywrightBaseTest
    {
        [Test]
        [Category("Smoke")]
        public async Task testPage()
        {
            test!.Info(MarkupHelper.CreateLabel("Verify page loads and take screenshot", AventStack.ExtentReports.MarkupUtils.ExtentColor.Blue));

            await Page.GotoAsync(baseUrl);
            await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.Load);

            var ready = await Page.EvaluateAsync<string>("() => document.readyState");
            Assert.AreEqual("complete", ready, "Page should be fully loaded");

            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string screenshotDir = Path.Combine(projectRoot, "Reports", "Screenshots");
            Directory.CreateDirectory(screenshotDir);

            string screenshotPath = Path.Combine(screenshotDir, "testPage.png");
            await Page.ScreenshotAsync(new() { Path = screenshotPath, FullPage = true });

            test?.Info($"Saved screenshot: {screenshotPath}");
        }
    }
}
