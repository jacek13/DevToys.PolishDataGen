using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Validators;

public class PeselValidator : IPolishIdValidator
{
    public ushort[] ControlMask { get; } = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

    private const ushort MmStartIndex = 2;

    private const ushort DdStartIndex = 4;

    private const ushort KStartIndex = 10;

    public int CalculateControlNumber(string value)
    {
        var peselDigits = new int[11];
        for (ushort i = 0; i < value.Length - 1; i++)
        {
            var digit = (value[i] - '0') * ControlMask[i];
            peselDigits[i] = digit % 10;
        }

        var result = peselDigits.Sum() % 10;
        return result == 0 ? 0 : 10 - result;
    }

    public bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
        if (value.Length != 11)
            return false;
        if (!value.All(char.IsAsciiDigit))
            return false;
        if (value.All(c => c == '0') || value.All(c => c == '9'))
            return false;
        if (!((int.Parse(value.AsSpan(MmStartIndex, 2)) % 20) is > 0 and < 13))
            return false;
        if (!(int.Parse(value.AsSpan(DdStartIndex, 2)) is > 0 and <= 31))
            return false;
        if (CalculateControlNumber(value) != (value[KStartIndex] - '0'))
            return false;

        return true;
    }

    public (bool Result, IEnumerable<string> Messages) IsValidExplained(string value)
    {
        var validationMessages = GetValidationMessages(value);

        return validationMessages.Any()
            ? (false, validationMessages)
            : (true, []);
    }

    private IEnumerable<string> GetValidationMessages(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            yield return Strings.PolishDataGen.PeselValidationNotEmptyMessage;
            yield break;
        }
        if (value.Length != 11)
        {
            yield return Strings.PolishDataGen.PeselValidationLengthMessage;
            yield break;
        }
        if (!value.All(char.IsAsciiDigit))
        {
            yield return Strings.PolishDataGen.PeselValidationOnlyDigitsMessage;
            yield break;
        }
        if (value.All(c => c == '0') || value.All(c => c == '9'))
            yield return Strings.PolishDataGen.PeselValidationExtremeValuesMessage;
        if (!((int.Parse(value.AsSpan(MmStartIndex, 2)) % 20) is > 0 and < 13))
            yield return Strings.PolishDataGen.PeselValidationMonthRangeMessage;
        if (!(int.Parse(value.AsSpan(DdStartIndex, 2)) is > 0 and <= 31))
            yield return Strings.PolishDataGen.PeselValidationDayRangeMessage;
        if (value.Length == 11 && CalculateControlNumber(value) != (value[KStartIndex] - '0'))
            yield return Strings.PolishDataGen.PeselValidationControlNumberMessage;
    }
}
