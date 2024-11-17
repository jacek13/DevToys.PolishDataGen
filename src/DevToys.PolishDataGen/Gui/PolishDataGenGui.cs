using DevToys.Api;
using System.ComponentModel.Composition;
using static DevToys.Api.GUI;

namespace DevToys.PolishDataGen.Gui;

[Export(typeof(IGuiTool))]
[Name("PolishDataGen")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uE670',
    GroupName = PredefinedCommonToolGroupNames.Generators,
    ResourceManagerAssemblyIdentifier = nameof(PolishDataGenResourceAssemblyIdentifier),
    ResourceManagerBaseName = "DevToys.PolishDataGen.Strings.PolishDataGen",
    ShortDisplayTitleResourceName = nameof(Strings.PolishDataGen.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(Strings.PolishDataGen.LongDisplayTitle),
    DescriptionResourceName = nameof(Strings.PolishDataGen.Description),
    AccessibleNameResourceName = nameof(Strings.PolishDataGen.AccessibleName))]
internal sealed class PolishDataGenGui : IGuiTool
{
    public UIToolView View => new(Label().Style(UILabelStyle.BodyStrong).Text(Strings.PolishDataGen.PolishDataGenLabel));

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}
