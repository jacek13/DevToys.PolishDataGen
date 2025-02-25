using System.ComponentModel.Composition;
using DevToys.Api;
using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Generators;
using static DevToys.Api.GUI;

namespace DevToys.PolishDataGen.Gui;

[Export(typeof(IGuiTool))]
[Name("PolishDataGen")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uE8EF',
    GroupName = PredefinedCommonToolGroupNames.Generators,
    ResourceManagerAssemblyIdentifier = nameof(PolishDataGenResourceAssemblyIdentifier),
    ResourceManagerBaseName = "DevToys.PolishDataGen.Strings.PolishDataGen",
    ShortDisplayTitleResourceName = nameof(Strings.PolishDataGen.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(Strings.PolishDataGen.LongDisplayTitle),
    DescriptionResourceName = nameof(Strings.PolishDataGen.Description),
    AccessibleNameResourceName = nameof(Strings.PolishDataGen.AccessibleName))]
internal sealed class PolishDataGenGui : IGuiTool
{
    private readonly IUILabel _label;

    private readonly IUISelectDropDownList _dropdownList;

    private readonly IUIButton _buttonGenerate;

    private readonly IUIButton _buttonClearMemory;

    private readonly IUINumberInput _numberInput;

    private readonly IUIMultiLineTextInput _outputMultiLineText;

    private readonly IUIInfoBar _infoBar;

    private readonly List<string> _results = new();

    private GeneratorType _generatorType = GeneratorType.Pesel;

    private IPolishIdGenerator _generator;

    private int _number = 1;

    private enum GridColumn
    {
        Stretch,
    }

    private enum GridRow
    {
        Settings,
        Results,
    }

    public UIToolView View =>
        new(
            isScrollable: true,
            Grid()
                .ColumnLargeSpacing()
                .RowLargeSpacing()
                .Rows(
                    (GridRow.Settings, Auto),
                    (GridRow.Results, new UIGridLength(1, UIGridUnitType.Fraction))
                )
                .Columns((GridColumn.Stretch, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Cells(
                    Cell(
                        GridRow.Settings,
                        GridColumn.Stretch,
                        Stack()
                            .Vertical()
                            .LargeSpacing()
                            .WithChildren(
                                _label,
                                _numberInput,
                                Stack()
                                    .Horizontal()
                                    .WithChildren(
                                        _buttonGenerate,
                                        _buttonClearMemory,
                                        _dropdownList
                                    )
                            )
                    ),
                    Cell(
                        GridRow.Results,
                        GridColumn.Stretch,
                        _outputMultiLineText.ReadOnly().AlwaysWrap()
                    )
                )
        );

    public PolishDataGenGui()
    {
        _label = Label()
            .Style(UILabelStyle.BodyStrong)
            .Text(Strings.PolishDataGen.PolishDataGenLabel);

        _dropdownList = SelectDropDownList()
            .AlignHorizontally(UIHorizontalAlignment.Left)
            .WithItems(
                Item(text: "Nip", value: GeneratorType.Nip),
                Item(text: "Pesel", value: GeneratorType.Pesel),
                Item(text: "Regon (9-digit)", value: GeneratorType.Regon),
                Item(text: "Regon (14-digit)", value: GeneratorType.RegonLong),
                Item(text: "Identity Card Number", value: GeneratorType.PolishIdentityCard)
            )
            .Select(1)
            .OnItemSelected(OnGeneratorTypeSelected);

        _buttonGenerate = Button()
            .Text(Strings.PolishDataGen.GuiGenerateButtonLabel)
            .OnClick(OnGeneratePressed);

        _buttonClearMemory = Button()
            .Text(Strings.PolishDataGen.GuiClearMemoryButtonLabel)
            .OnClick(OnMemoryClearPressed);

        _numberInput = NumberInput()
            .Text(Strings.PolishDataGen.GuiNumberInputLabel)
            .Minimum(1)
            .Maximum(1_000_000)
            .Step(1.0)
            .Value(1.0)
            .OnValueChanged(OnNumberChanged);

        _outputMultiLineText = MultiLineTextInput()
            .Title(Strings.PolishDataGen.CliMultiLineTextInputTitle)
            .AlwaysShowLineNumber()
            .ReadOnly()
            .Extendable();

        _infoBar = InfoBar()
            .Title(Strings.PolishDataGen.CliInfoBarTitle)
            .Description(Strings.PolishDataGen.CliInfoBarContent)
            .Warning()
            .ShowIcon()
            .Closable();

        _generator = GeneratorFactory.Create(_generatorType);
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }

    private void OnGeneratePressed()
    {
        _generator = GeneratorFactory.Create(_generatorType);
        if (_number == 1)
        {
            _results.Add(_generator.Create());
        }
        else
        {
            _results.AddRange(_generator.CreateMany(_number));
        }
        _outputMultiLineText.Text(string.Join('\n', _results));
    }

    private void OnMemoryClearPressed()
    {
        _results.Clear();
        _outputMultiLineText.Text(string.Empty);
        GC.Collect();
    }

    private void OnNumberChanged(double value)
    {
        _number = Convert.ToInt32(value);
        if (_number >= 100_000)
        {
            _infoBar.Open();
        }
    }

    private void OnGeneratorTypeSelected(IUIDropDownListItem? uIDropDownListItem)
    {
        if (uIDropDownListItem?.Value is GeneratorType type)
        {
            _generatorType = type;
        }
    }
}
