using Microsoft.Playwright;
using System.Threading.Tasks;
using AventStack.ExtentReports.MarkupUtils;
using C_Sharp_Selenium_NUnit.BaseClass;
using C_Sharp_Selenium_NUnit.Data;
using NUnit.Framework;
using System.Threading.Tasks;

namespace PlaywrightTests.Utilities
{
    public class PlaywrightDriver
    {
        public IPlaywright Playwright { get; private set; }
        public IBrowser Browser { get; private set; }
        public IBrowserContext Context { get; private set; }
        public IPage Page { get; private set; }

        public async Task<IPage> LaunchBrowserAsync(bool isHeaded = true, int slowMo = 0)
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = !isHeaded,   // 👈 headed mode control
                SlowMo = slowMo         // 👈 optional: slow motion to visualize steps
            });

            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();

            return Page;
        }

        public async Task CloseAsync()
        {
            await Browser.CloseAsync();
            Playwright.Dispose();
        }

        [Test]  
        public async Task RunHeadedTest()
        {
            var driver = new PlaywrightDriver();
            var page = await driver.LaunchBrowserAsync(isHeaded: true, slowMo: 200);

            await page.GotoAsync("https://playwright.dev");
            await page.ClickAsync("text=Get started");

            await Task.Delay(2000); // pause to view

            await driver.CloseAsync();
        }

    }
    
}

