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
    private readonly IUILabel _label = Label()
        .Style(UILabelStyle.BodyStrong)
        .Text(Strings.PolishDataGen.PolishDataGenLabel);

    // TODO move strings to resources
    private readonly IUIButton _buttonGenerate = Button()
        .Text("Generate");

    // TODO profile memory to find proper limits
    private readonly IUINumberInput _numberInput = NumberInput()
        .Text("Number of data to generate")
        .Minimum(1)
        .Maximum(1_000_000)
        .Step(1.0);

    private readonly IUIMultiLineTextInput _outputMultiLineText = MultiLineTextInput()
        .Extendable()
        .AlwaysShowLineNumber()
        .ReadOnly();

    public UIToolView View
        => new UIToolView(
            Stack()
                .Vertical()
                .WithChildren(_label, _numberInput, _buttonGenerate, _outputMultiLineText)
        );

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }

    // TODO make OnGenerate method
    // TODO make OnGeneratorTypeSelected method
    // TODO make OnNumberChanged
}
