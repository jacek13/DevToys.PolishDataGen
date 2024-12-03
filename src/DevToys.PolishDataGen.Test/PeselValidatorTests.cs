using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class PeselValidatorTests
{
    private readonly PeselValidator _validator;

    public PeselValidatorTests()
    {
        _validator = new PeselValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("           ")]
    public void IsValid_ShouldReturnFalse_ForEmptyValues(string pesel)
    {
        // Act
        var result = _validator.IsValid(pesel);

        // Assert
        Assert.False(result, $"Expected PESEL '{pesel}' to be invalid.");
    }

    // Valid PESELs:
    [Theory]
    [InlineData("62112672784")]
    [InlineData("12111063748")]
    [InlineData("24012278961")]
    [InlineData("47050644800")]
    [InlineData("22060601520")]
    [InlineData("09322840093")]
    [InlineData("20291107778")]
    public void IsValid_ShouldReturnTrue_ForValidPesel(string pesel)
    {
        // Act
        var result = _validator.IsValid(pesel);

        // Assert
        Assert.True(result, $"Expected PESEL '{pesel}' to be valid.");
    }

    [Theory]
    [InlineData("44051401350")] // Invalid control digit
    [InlineData("12345678901")] // Random invalid number
    [InlineData("4405140135")]  // Too short
    [InlineData("440514013590")] // Too long
    [InlineData("83A61928272")] // Non-numeric
    [InlineData("83A619!8272")] // Non-numeric
    [InlineData("00000000000")] // Invalid control digit
    [InlineData("99999999999")] // Invalid control digit
    [InlineData("83061928271")] // Invalid control digit
    [InlineData("18043037706")] // Invalid control digit
    [InlineData("00140000000")] // Invalid MM numbers digit
    [InlineData("00016400000")] // Invalid DD numbers digit
    public void IsValid_ShouldReturnFalse_ForInvalidPesel(string pesel)
    {
        // Act
        var result = _validator.IsValid(pesel);

        // Assert
        Assert.False(result, $"Expected PESEL '{pesel}' to be invalid.");
    }

    [Theory]
    [InlineData("44051401359", true, new string[0])] // Valid PESEL, no error messages
    [InlineData("4405140135", false, new[] { "PESEL must be 11 characters long" })] // Too short
    public void IsValidExplained_ShouldReturnExpectedResultAndMessages(
        string pesel,
        bool expectedResult,
        string[] expectedMessages)
    {
        // Act
        var (result, messages) = _validator.IsValidExplained(pesel);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedMessages, messages);
    }

    [Theory]
    [InlineData("47050644800", 0)] // Control digit is correct
    [InlineData("09322840093", 3)] // Control digit is correct
    public void CalculateControlNumber_ShouldReturnCorrectValue(string pesel, int expectedControlNumber)
    {
        // Act
        var controlNumber = _validator.CalculateControlNumber(pesel);

        // Assert
        Assert.Equal(expectedControlNumber, controlNumber);
    }
}