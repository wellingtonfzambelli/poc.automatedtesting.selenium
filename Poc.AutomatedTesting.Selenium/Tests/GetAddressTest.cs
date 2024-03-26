using OpenQA.Selenium;
using Poc.AutomatedTesting.Selenium.Core;

namespace Poc.AutomatedTesting.Selenium.Tests;

public sealed class GetAddressTest : BaseTest
{
    const string _cepValue = "30180-001";

    const string _cepFieldId = "endereco";
    const string _dropdownId = "tipoCEP";
    const string _buttonSendId = "btn_pesquisar";

    public GetAddressTest() : base("https://buscacepinter.correios.com.br/app/endereco/index.php", true)
    { }

    [Fact]
    public void Fill_CEP_field_and_GetAddress_returns_success()
    {
        // Arrange
        string[] results =
        {
            "Avenida Amazonas - até 559 - lado ímpar",
            "Centro",
            "Belo Horizonte/MG",
            "30180-001"
        };

        // Act 
        base.SetTextBoxValueById(_cepValue, _cepFieldId);
        base.ClickButtonById(_buttonSendId);

        // Assert
        for (int i = 0; i < results.Length; i++)
        {
            var resultAddressElement = base.Driver.FindElement(By.XPath($"//*[@id='resultado-DNEC']/tbody/tr/td[{i + 1}]"));
            Assert.Equal(results[i], resultAddressElement.Text);
        }
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
        base.SetTextBoxValueById(_cepValue, _cepFieldId);
        base.SetDropdownValueById(dropDownValue, _dropdownId);
        base.ClickButtonById(_buttonSendId);

        var resultNotFoundElement = base.Driver.FindElement(By.XPath(resultNotFoundXPath));

        // Assert
        Assert.Equal(resultNotFoundValue, resultNotFoundElement.Text);
    }
}