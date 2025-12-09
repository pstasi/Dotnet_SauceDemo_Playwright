using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Playwright;

namespace FirstPlaywrightTest.pages
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
        public async Task EnterUsernameIntoField(string username)
        {
            await _usernameInput.FillAsync(username);
        }

        public async Task EnterPasswordIntoField(string password)
        {
            await _passwordInput.FillAsync(password);
        }

        public async Task ClickLoginButton()
        {
            await _loginButton.ClickAsync();
        }

        public async Task AttemptLogin(string username, string password)
        {
            await EnterUsernameIntoField(username);
            await EnterPasswordIntoField(password);
            await ClickLoginButton();
        }

        public async Task<string> GetHomePageErrorMessage()
        {
            return await _errorMessage.InnerTextAsync();
        }
    }
}
