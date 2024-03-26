using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Poc.AutomatedTesting.Selenium.Core;

public abstract class BaseTest : IDisposable
{
    public IWebDriver _driver;
    private bool _closeWindowAfterTestExecution;

    public BaseTest(string urlPage, bool isHeadlessMode) =>
        SetupEdgeBrowser(urlPage, isHeadlessMode);

    private void SetupEdgeBrowser(string urlPage, bool isHeadlessMode)
    {
        var edgeOptions = new EdgeOptions();
        edgeOptions.AddArgument("disk-cache-size=0");

        if (isHeadlessMode)
        {
            edgeOptions.AddArgument("window-size=1366x768");
            edgeOptions.AddArgument("headless");
            _driver = new EdgeDriver(edgeOptions);
        }
        else // Dev Mode
        {
            edgeOptions.AddArguments("start.maximized");
            _driver = new EdgeDriver(edgeOptions);
            _closeWindowAfterTestExecution = false;
        }

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Navigate().GoToUrl(urlPage);
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