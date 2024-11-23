namespace DevToys.PolishDataGen.Interfaces;

public interface IPolishIdValidator
{
    /// <summary>
    /// Property that holds the control mask used for validating the check digit.
    /// </summary>
    ushort[] ControlMask { get; }

    /// <summary>
    /// Validates the provided identification number. It will stop validation on first failure.
    /// </summary>
    /// <param name="value">The identification number to validate.</param>
    /// <returns>
    /// True if the identification number is valid; otherwise, false.
    /// </returns>
    bool IsValid(string value);

    /// <summary>
    /// Validates the provided identification number and explains the result. It will run all validation rules.
    /// </summary>
    /// <param name="value">The identification number to validate.</param>
    /// <returns>
    /// A tuple where:
    /// - Result: Indicates whether the identification number is valid.
    /// - Messages: Provides detailed information about the validation process.
    /// </returns>
    (bool Result, IEnumerable<string> Messages) IsValidExplained(string value);

    /// <summary>
    /// Calculates the control number for the provided identification number.
    /// </summary>
    /// <param name="value">The identification number to calculate the control number for.</param>
    /// <returns>
    /// The calculated control number as an integer.
    /// </returns>
    int CalculateControlNumber(string value);
}