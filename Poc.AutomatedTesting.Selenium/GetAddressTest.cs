using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace Poc.AutomatedTesting.Selenium;

public sealed class GetAddressTest : IDisposable
{
    private readonly IWebDriver driver;
    private readonly bool closeWindowAfterTestExecution = true;

    public GetAddressTest()
    {
        driver = new EdgeDriver();
        driver.Navigate().GoToUrl("https://buscacepinter.correios.com.br/app/endereco/index.php");
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [Fact]
    public void Fill_CEP_field_and_GetAddress_returns_success()
    {
        // Arrange
        const string cepValue = "30180-001";
        const string cepFieldId = "endereco";
        const string buttonSendId = "btn_pesquisar";

        const string resultAddressXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[1]";
        const string resultAddressValue = "Avenida Amazonas - até 559 - lado ímpar";

        const string resultDistrictXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[2]";
        const string resultDistrictValue = "Centro";

        const string resultCityXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[3]";
        const string resultCityValue = "Belo Horizonte/MG";

        const string resultCepXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[4]";
        const string resultCepValue = "30180-001";

        // Act 
        driver.FindElement(By.Id(cepFieldId)).SendKeys(cepValue);
        driver.FindElement(By.Id(buttonSendId)).Click();

        var resultAddressElement = driver.FindElement(By.XPath(resultAddressXPath));
        var resultDistrictElement = driver.FindElement(By.XPath(resultDistrictXPath));
        var resultCityElement = driver.FindElement(By.XPath(resultCityXPath));
        var resultCepElement = driver.FindElement(By.XPath(resultCepXPath));

        // Assert
        Assert.Equal(resultAddressValue, resultAddressElement.Text);
        Assert.Equal(resultDistrictValue, resultDistrictElement.Text);
        Assert.Equal(resultCityValue, resultCityElement.Text);
        Assert.Equal(resultCepValue, resultCepElement.Text);
    }

    [Theory]
    [InlineData("PRO")]
    [InlineData("CPC")]
    [InlineData("GRU")]
    [InlineData("UOP")]
    public void Fill_CEP_field_and_set_invalid_dropdown_option_returns_fail(string dropDownValue)
    {
        // Arrange
        const string cepValue = "30180-001";
        const string cepFieldId = "endereco";
        const string dropdownId = "tipoCEP";
        const string buttonSendId = "btn_pesquisar";

        const string resultNotFoundXPath = "//*[@id='mensagem-resultado-alerta']/h6";
        const string resultNotFoundValue = "Dados não encontrado";

        // Act 
        driver.FindElement(By.Id(cepFieldId)).SendKeys(cepValue);
        SelectElement dropDown = new SelectElement(driver.FindElement(By.Id(dropdownId)));
        dropDown.SelectByValue(dropDownValue);
        driver.FindElement(By.Id(buttonSendId)).Click();

        var resultNotFoundElement = driver.FindElement(By.XPath(resultNotFoundXPath));

        // Assert
        Assert.Equal(resultNotFoundValue, resultNotFoundElement.Text);
    }

    public void Dispose()
    {
        if (closeWindowAfterTestExecution)
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}