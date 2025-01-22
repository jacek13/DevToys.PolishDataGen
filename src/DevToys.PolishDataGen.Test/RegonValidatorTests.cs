using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class RegonValidatorTests
{
    private readonly RegonValidator _validator;

    public RegonValidatorTests()
    {
        _validator = new RegonValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("           ")]
    public void IsValid_ShouldReturnFalse_ForEmptyValues(string regon)
    {
        // Act
        var result = _validator.IsValid(regon);

        // Assert
        Assert.False(result, $"Expected REGON '{regon}' to be invalid.");
    }

    // Random generated valid REGONSs:
    [Theory]
    [InlineData("034483590")]
    [InlineData("932088789")]
    [InlineData("755625014")]
    [InlineData("373517926")]
    [InlineData("237887620")]
    [InlineData("753632487")]
    [InlineData("672063678")]
    [InlineData("614167124")]
    [InlineData("251798124")]
    [InlineData("123456785")]
    [InlineData("12345678512347")]

    public void IsValid_ShouldReturnTrue_ForValidRegon(string regon)
    {
        // Act
        var result = _validator.IsValid(regon);

        // Assert
        Assert.True(result, $"Expected REGON '{regon}' to be valid.");
    }

    [Theory]
    [InlineData("12345")] // Too short
    [InlineData("01234567891")] // Between 9 and 14
    [InlineData("6593839991234234")] // Too long
    [InlineData("6593A3999")] // Non-numeric
    [InlineData("65938399!")] // Non-numeric
    [InlineData("@593A3999")] // Non-numeric
    [InlineData("1234567851234!")] // Non-numeric
    [InlineData("123456A8512347")] // Non-numeric
    [InlineData("$23456A8512347")] // Non-numeric
    [InlineData("000000000")] // Invalid boundary values
    [InlineData("999999999")] // Invalid boundary values
    [InlineData("00000000000000")] // Invalid boundary values
    [InlineData("99999999999999")] // Invalid boundary values
    [InlineData("659383998")] // Invalid control digit
    [InlineData("659383991")] // Invalid control digit
    [InlineData("12345678512346")] // Invalid control digit
    public void IsValid_ShouldReturnFalse_ForInvalidRegon(string regon)
    {
        // Act
        var result = _validator.IsValid(regon);

        // Assert
        Assert.False(result, $"Expected REGON '{regon}' to be invalid.");
    }
}
