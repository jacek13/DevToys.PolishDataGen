using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Validators;

public class IdentityCardNumberValidator : IPolishIdValidator
{
    public ushort[] ControlMask { get; } = { 7, 3, 1, 9, 7, 3, 1, 7, 3 };

    private const ushort KStartIndex = 3;

    private const ushort InitialAlphabetValue = 10;

    public int CalculateControlNumber(string value)
    {
        var values = new int[9];
        for (ushort i = 0; i < value.Length; i++)
        {
            var asciiOffset = i < KStartIndex ? 'A' - InitialAlphabetValue : '0';
            var number = (value[i] - asciiOffset) * ControlMask[i];
            values[i] = number;
        }

        return values.Sum() % 10;
    }

    public bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
        if (value.Length != 9)
            return false;
        if (!IsTrueForAll(value.AsSpan(KStartIndex, 6), char.IsAsciiDigit))
            return false;
        if (ContainsOnlyExtremeValues(value.AsSpan(KStartIndex, 6)))
            return false;
        if (!IsTrueForAll(value.AsSpan(0, 3), char.IsAsciiLetterUpper))
            return false;
        if (CalculateControlNumber(value) != 0)
            return false;

        return true;
    }

    private static bool IsTrueForAll(ReadOnlySpan<char> span, Func<char, bool> predicate)
    {
        for (var i = 0; i < span.Length; i++)
        {
            if (!predicate(span[i]))
                return false;
        }
        return true;
    }

    private static bool ContainsOnlyExtremeValues(ReadOnlySpan<char> span)
        => ContainsSpecificCharacter(span, '0') || ContainsSpecificCharacter(span, '9');

    private static bool ContainsSpecificCharacter(ReadOnlySpan<char> span, char character)
    {
        for (var i = 0; i < span.Length; i++)
        {
            if (span[i] != character)
                return false;
        }
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
            yield return Strings.PolishDataGen.IdentityCardNumberNotEmptyMessage;
        if (value.Length != 9)
            yield return Strings.PolishDataGen.IdentityCardNumberValidationLengthMessage;
        if (!IsTrueForAll(value.AsSpan(KStartIndex, 6), char.IsAsciiDigit))
            yield return Strings.PolishDataGen.IdentityCardNumberNumericPartValidationMessage;
        if (ContainsOnlyExtremeValues(value.AsSpan(KStartIndex, 6)))
            yield return Strings.PolishDataGen.IdentityCardNumberExtremeValuesValidationMessage;
        if (!IsTrueForAll(value.AsSpan(0, 3), char.IsAsciiLetterUpper))
            yield return Strings.PolishDataGen.IdentityCardNumberPrefixPartValidationMessage;
        if (CalculateControlNumber(value) != 0)
            yield return Strings.PolishDataGen.IdentityCardControlNumberValidationMessage;
    }
}
