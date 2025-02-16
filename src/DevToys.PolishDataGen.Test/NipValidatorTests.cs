using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class NipValidatorTests
{
    private readonly NipValidator _validator;

    public NipValidatorTests()
    {
        _validator = new NipValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("           ")]
    public void IsValid_ShouldReturnFalse_ForEmptyValues(string nip)
    {
        // Act
        var result = _validator.IsValid(nip);

        // Assert
        Assert.False(result, $"Expected NIP '{nip}' to be invalid.");
    }

    [Theory]
    [InlineData("8379142749")]
    [InlineData("7614055661")]
    [InlineData("1162829841")]
    [InlineData("1186065846")]
    public void IsValid_ShouldReturnTrue_ForValidNip(string nip)
    {
        // Act
        var result = _validator.IsValid(nip);

        // Assert
        Assert.True(result, $"Expected NIP '{nip}' to be valid.");
    }

    [Theory]
    [InlineData("123456")] // Too short
    [InlineData("123456789012")] // Too long
    [InlineData("118606A846")] // Non-numeric
    [InlineData("11!6065846")] // Non-numeric
    [InlineData("0000000000")] // Invalid boundary values
    [InlineData("9999999999")] // Invalid boundary values
    [InlineData("2709142740")] // Invalid prefix
    [InlineData("1186065845")] // Invalid control digit
    public void IsValid_ShouldReturnFalse_ForInvalidNip(string nip)
    {
        // Act
        var result = _validator.IsValid(nip);

        // Assert
        Assert.False(result, $"Expected NIP '{nip}' to be invalid.");
    }
}
