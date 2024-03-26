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
        if (isHeadlessMode)
            EnableHeadlessMode();
        else
            EnableDevsMode();

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Navigate().GoToUrl(urlPage);
    }

    private void EnableHeadlessMode()
    {
        var edgeOptions = new EdgeOptions();
        edgeOptions.AddArgument("disk-cache-size=0");
        edgeOptions.AddArgument("window-size=1366x768");
        edgeOptions.AddArgument("headless");

        _driver = new EdgeDriver(edgeOptions);
    }

    private void EnableDevsMode()
    {
        var edgeOptions = new EdgeOptions();
        edgeOptions.AddArgument("disk-cache-size=0");
        edgeOptions.AddArguments("start.maximized");

        _driver = new EdgeDriver(edgeOptions);

        _closeWindowAfterTestExecution = false;
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