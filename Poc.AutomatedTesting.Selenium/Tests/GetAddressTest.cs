using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Poc.AutomatedTesting.Selenium.Core;

namespace Poc.AutomatedTesting.Selenium.Tests;

public sealed class GetAddressTest : BaseTest
{
    const string _cepValue = "30180-001";

    const string _cepFieldId = "endereco";
    const string _dropdownId = "tipoCEP";
    const string _buttonSendId = "btn_pesquisar";

    public GetAddressTest() : base("https://buscacepinter.correios.com.br/app/endereco/index.php", false)
    { }

    [Fact]
    public void Fill_CEP_field_and_GetAddress_returns_success()
    {
        // Arrange
        const string resultAddressXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[1]";
        const string resultAddressValue = "Avenida Amazonas - até 559 - lado ímpar";

        const string resultDistrictXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[2]";
        const string resultDistrictValue = "Centro";

        const string resultCityXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[3]";
        const string resultCityValue = "Belo Horizonte/MG";

        const string resultCepXPath = "//*[@id='resultado-DNEC']/tbody/tr/td[4]";
        const string resultCepValue = "30180-001";

        // Act 
        _driver.FindElement(By.Id(_cepFieldId)).SendKeys(_cepValue);
        _driver.FindElement(By.Id(_buttonSendId)).Click();

        var resultAddressElement = _driver.FindElement(By.XPath(resultAddressXPath));
        var resultDistrictElement = _driver.FindElement(By.XPath(resultDistrictXPath));
        var resultCityElement = _driver.FindElement(By.XPath(resultCityXPath));
        var resultCepElement = _driver.FindElement(By.XPath(resultCepXPath));

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
        const string resultNotFoundXPath = "//*[@id='mensagem-resultado-alerta']/h6";
        const string resultNotFoundValue = "Dados não encontrado";

        // Act 
        _driver.FindElement(By.Id(_cepFieldId)).SendKeys(_cepValue);
        SelectElement dropDown = new SelectElement(_driver.FindElement(By.Id(_dropdownId)));
        dropDown.SelectByValue(dropDownValue);
        _driver.FindElement(By.Id(_buttonSendId)).Click();

        var resultNotFoundElement = _driver.FindElement(By.XPath(resultNotFoundXPath));

        // Assert
        Assert.Equal(resultNotFoundValue, resultNotFoundElement.Text);
    }
}