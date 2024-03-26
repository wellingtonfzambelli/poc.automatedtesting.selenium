using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Poc.AutomatedTesting.Selenium.Core;

public abstract class BaseTest : IDisposable
{
    public readonly IWebDriver _driver;
    private readonly bool _closeWindowAfterTestExecution;

    public BaseTest(string urlPage, bool closeWindowAfterTestExecution = true)
    {
        _driver = new EdgeDriver();
        _driver.Navigate().GoToUrl(urlPage);
        _driver.Manage().Window.Maximize();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _closeWindowAfterTestExecution = closeWindowAfterTestExecution;
    }

    public void Dispose()
    {
        if (_closeWindowAfterTestExecution)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}