using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Playwright;

namespace firstPlaywrightTest.pages
{
    public class HomePage
    {
        private readonly IPage _page;
        private readonly ILocator _usernameInput;
        private readonly ILocator _passwordInput;
        private readonly ILocator _loginButton;
        public readonly ILocator _errorMessage;


        public HomePage(IPage page)
        {
            _page = page;
            _usernameInput = page.GetByRole(AriaRole.Textbox, new() { Name = "Username"});
            _passwordInput = page.GetByRole(AriaRole.Textbox, new() { Name = "Password"});
            _loginButton = page.Locator("//input[@id='login-button']");
            _errorMessage = page.Locator("//*[@data-test='error']");

        }

        public async Task EnterUsernameIntoField()
        {
            await _usernameInput.FillAsync("standard_user");
        }

        public async Task EnterPasswordIntoField()
        {
            await _passwordInput.FillAsync("secret_sauce");
        }

        public async Task ClickLoginButton()
        {
            await _loginButton.ClickAsync();
        }

        public async Task AttemptLogin()
        {
            await EnterUsernameIntoField();
            await EnterPasswordIntoField();
            await ClickLoginButton();
        }

        public async Task<string> GetHomePageErrorMessage()
        {
            return await _errorMessage.InnerTextAsync();
        }
    }
}
