using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace Poc.AutomatedTesting.Selenium.Core;

public abstract class BaseTest : IDisposable
{
    public IWebDriver Driver;
    private bool _closeWindowAfterTestExecution;

    public BaseTest(string urlPage, bool isHeadlessMode) =>
        SetupEdgeBrowser(urlPage, isHeadlessMode);

    private void SetupEdgeBrowser(string urlPage, bool isHeadlessMode)
    {
        if (isHeadlessMode)
            EnableHeadlessMode();
        else
            EnableDevsMode();

        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        Driver.Navigate().GoToUrl(urlPage);
    }

    private void EnableHeadlessMode()
    {
        var edgeOptions = new EdgeOptions();
        edgeOptions.AddArgument("disk-cache-size=0");
        edgeOptions.AddArgument("window-size=1366x768");
        edgeOptions.AddArgument("headless");

        Driver = new EdgeDriver(edgeOptions);
    }

    private void EnableDevsMode()
    {
        var edgeOptions = new EdgeOptions();
        edgeOptions.AddArgument("disk-cache-size=0");
        edgeOptions.AddArguments("start.maximized");

        Driver = new EdgeDriver(edgeOptions);

        _closeWindowAfterTestExecution = false;
    }

    #region Actions

    public void SetTextBoxValueById(string text, string fieldId) =>
        Driver.FindElement(By.Id(fieldId)).SendKeys(text);

    public void ClickButtonById(string buttonId) =>
        Driver.FindElement(By.Id(buttonId)).Click();

    public void SetDropdownValueById(string value, string dropdownId)
    {
        SelectElement dropDown = new SelectElement(Driver.FindElement(By.Id(dropdownId)));
        dropDown.SelectByValue(value);
    }

    public void ClickOutField() =>
        Driver.FindElement(By.XPath("//html")).Click();

    public void WaitElementShows(string element, int seconds = 90)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

        wait.Until((d) =>
        {
            return d.FindElement(By.XPath(element));
        });
    }

    public void WaitElementDisapear(string element, int seconds = 90)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

        wait.Until(d =>
            d.FindElements(By.XPath(element)).Count == 0);
    }

    public bool VerifyIfElementExists(string xPath)
    {
        try
        {
            Driver.FindElement(By.XPath(xPath));
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public void Wait(int valueInMiliseconds) =>
        Thread.Sleep(valueInMiliseconds);

    #endregion

    public void Dispose()
    {
        if (_closeWindowAfterTestExecution)
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}