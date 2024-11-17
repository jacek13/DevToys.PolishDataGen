using DevToys.Api;
using System.ComponentModel.Composition;

namespace DevToys.PolishDataGen;

[Export(typeof(IResourceAssemblyIdentifier))]
[Name(nameof(PolishDataGenResourceAssemblyIdentifier))]
internal sealed class PolishDataGenResourceAssemblyIdentifier : IResourceAssemblyIdentifier
{
    public ValueTask<FontDefinition[]> GetFontDefinitionsAsync()
    {
        throw new NotImplementedException();
    }
}
