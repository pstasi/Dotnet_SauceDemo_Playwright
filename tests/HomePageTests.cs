using firstPlaywrightTest.pages;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace firstPlaywrightTest.tests
{
    [Category("HomePageTests")]
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ExampleTest : PageTest
    {
        private IPage _page;
        private HomePage _homePage;

        [SetUp]
        public async Task Setup()
        {
            _page = Page;
            _homePage = new HomePage(_page);
            await _page.GotoAsync("https://saucedemo.com");
        }

        [TearDown]
        public async Task Teardown()
        {
            await _page.CloseAsync();
        }

        [Test]
        [Category("Regression")]
        public async Task ValidateErrorMessageNoUserNoPassword()
        {
            Console.WriteLine("ValidateErrorMessageNoUserPassword");
            await _homePage.ClickLoginButton();
            string? errorMessage = await _homePage.GetHomePageErrorMessage();
            Console.WriteLine(errorMessage);
            if (errorMessage != "Epic sadface: Username is required")
            {
                throw new Exception("Error!");
            }
        }

        [Test]
        [Category("Login")]
        public async Task AttemptLoginTest()
        {
            Console.WriteLine("running attemptlogintest");
            await _homePage.AttemptLogin();
        }
    }
}
