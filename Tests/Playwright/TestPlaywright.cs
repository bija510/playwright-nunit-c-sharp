using AventStack.ExtentReports.MarkupUtils;
using C_Sharp_Selenium_NUnit.BaseClass;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace C_Sharp_Selenium_NUnit.Tests.Playwright
{
    [TestFixture]
    public class TestPlaywright : PlaywrightBaseTest
    {
        [Test]
        [Category("Smoke")]
        public async Task test()
        {
            test!.Info(MarkupHelper.CreateLabel("Verify login form fields and attempt submit", AventStack.ExtentReports.MarkupUtils.ExtentColor.Blue));

            string loginUrl = baseUrl.EndsWith("/") ? baseUrl + "login" : baseUrl + "/login";
            await Page.GotoAsync(loginUrl);

            var emailLocator = Page.Locator("input[type=\"email\"]");
            var passLocator = Page.Locator("input[type=\"password\"]");

            bool emailVisible = await emailLocator.IsVisibleAsync().ConfigureAwait(false);
            bool passVisible = await passLocator.IsVisibleAsync().ConfigureAwait(false);

            // If explicit email/password inputs are not present, try common placeholders
            if (!emailVisible)
            {
                emailVisible = await Page.Locator("input[placeholder=\"Username\"]").IsVisibleAsync().ConfigureAwait(false);
            }

            if (!passVisible)
            {
                passVisible = await Page.Locator("input[placeholder=\"Password\"]").IsVisibleAsync().ConfigureAwait(false);
            }

            Assert.IsTrue(emailVisible, "Email/username input should be visible on login page");
            Assert.IsTrue(passVisible, "Password input should be visible on login page");

            // Fill fields if present
            try
            {
                if (await emailLocator.CountAsync() > 0)
                {
                    await emailLocator.FillAsync(ConfigReader.Load().UserName ?? userName);
                }
                else if (await Page.Locator("input[placeholder=\"Username\"]").CountAsync() > 0)
                {
                    await Page.Locator("input[placeholder=\"Username\"]").FillAsync(userName);
                }

                if (await passLocator.CountAsync() > 0)
                {
                    await passLocator.FillAsync(ConfigReader.Load().Password ?? password);
                }
                else if (await Page.Locator("input[placeholder=\"Password\"]").CountAsync() > 0)
                {
                    await Page.Locator("input[placeholder=\"Password\"]").FillAsync(password);
                }
            }
            catch (Exception ex)
            {
                test?.Warning($"Filling inputs failed: {ex.Message}");
            }

            var submit = Page.Locator("button[type=submit], input[type=submit]");
            if (await submit.CountAsync() > 0)
            {
                await submit.First.ClickAsync();
            }

            test?.Info("Login form interaction completed");
        }
    }
}
