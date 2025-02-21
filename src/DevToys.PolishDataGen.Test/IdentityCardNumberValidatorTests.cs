using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class IdentityCardNumberValidatorTests
{
    private readonly IdentityCardNumberValidator _validator;

    public IdentityCardNumberValidatorTests()
    {
        _validator = new IdentityCardNumberValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("           ")]
    public void IsValid_ShouldReturnFalse_ForEmptyValues(string identityCardNumber)
    {
        // Act
        var result = _validator.IsValid(identityCardNumber);

        // Assert
        Assert.False(result, $"Expected Identity Card Number '{identityCardNumber}' to be invalid.");
    }

    [Theory]
    [InlineData("ABA300000")]
    [InlineData("ABS123456")]
    public void IsValid_ShouldReturnTrue_ForValidIdentityCardNumber(string identityCardNumber)
    {
        // Act
        var result = _validator.IsValid(identityCardNumber);

        // Assert
        Assert.True(result, $"Expected Identity Card Number '{identityCardNumber}' to be valid.");
    }

    [Theory]
    [InlineData("ABA30000")] // Too short
    [InlineData("ABA30000000")] // Too long
    [InlineData("AB@300000")] // Non-alphanumericals 
    [InlineData("ABAB00000")] // Invalid prefix
    [InlineData("AB0000000")] // Invalid prefix
    [InlineData("ABA400000")] // Invalid control digit
    public void IsValid_ShouldReturnFalse_ForInvalidIdentityCardNumber(string identityCardNumber)
    {
        // Act
        var result = _validator.IsValid(identityCardNumber);

        // Assert
        Assert.False(result, $"Expected Identity Card Number '{identityCardNumber}' to be invalid.");
    }
}
