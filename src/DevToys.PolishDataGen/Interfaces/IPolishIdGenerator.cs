namespace DevToys.PolishDataGen.Interfaces;

public interface IPolishIdGenerator
{
    /// <summary>
    /// Property that holds the control mask used for generating the check digit.
    /// </summary>
    ushort[] ControlMask { get; }

    /// <summary>
    /// Creates a single identifier according to the rules of the specific generator.
    /// </summary>
    /// <returns>A single valid string representing the identifier.</returns>
    string Create();

    /// <summary>
    /// Creates a specified number of identifiers.
    /// </summary>
    /// <param name="count">The number of identifiers to generate.</param>
    /// <returns>A collection of generated identifiers.</returns>
    IReadOnlyCollection<string> CreateMany(int count);
}
