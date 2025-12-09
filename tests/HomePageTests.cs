using FirstPlaywrightTest.models;
using FirstPlaywrightTest.pages;
using FirstPlaywrightTest.utils;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;



namespace FirstPlaywrightTest.tests
{
    [Category("HomePageTests")]
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class HomePageTests : PageTest
    {
        private IPage _page; //page object being controlled
        private HomePage _homePage; //page object model for homepage
        private HomePageURLs _homePageURLs; //this currently helps us pass the url to the 

        public class LoginTestData
        {
            public string testCaseName { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public string username { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
            public string expectedError { get; set; } = string.Empty;

            public LoginTestData() { }
        }

        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                // Resolve a stable path under the test output directory first
                string filePath = Path.Combine(AppContext.BaseDirectory, "testData", "userlogintestdata", "UserLoginTests.json");
                // fallback to project-relative layout for local runs
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "testData", "userlogintestdata", "UserLoginTests.json");
                }

                if (!File.Exists(filePath))
                {
                    // don't throw during discovery; yield no testcases
                    yield break;
                }

                List<LoginTestData>? data = null;
                try
                {
                    var json = File.ReadAllText(filePath);
                    data = JsonSerializer.Deserialize<List<LoginTestData>>(json);
                }
                catch
                {
                    // swallow errors during discovery so discovery can continue
                    yield break;
                }

                if (data == null)
                    yield break;

                foreach (var testCaseData in data)
                {
                    yield return new TestCaseData(testCaseData)
                        .SetName(string.IsNullOrWhiteSpace(testCaseData.description) ? testCaseData.testCaseName : testCaseData.description);
                }
            }
        }

        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            string env = TestContext.Parameters.Get("TestEnv", "QA");
            _page = Page;
            _homePage = new(_page);
            _homePageURLs = new(env);
            await _page.GotoAsync(_homePageURLs.Url);
        }

        [TearDown]
        public async System.Threading.Tasks.Task Teardown()
        {
            await _page.CloseAsync();
        }

        [Test]
        [Category("Regression")]
        public async System.Threading.Tasks.Task ValidateErrorMessageNoUserNoPassword()
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

        [Ignore("Hidden")]
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async System.Threading.Tasks.Task RunDynamicTest(LoginTestData data)
        {
            await _homePage.AttemptLogin(data.username, data.password);

            await Expect(_homePage._errorMessage).ToContainTextAsync(data.expectedError);
        }

    }
}
