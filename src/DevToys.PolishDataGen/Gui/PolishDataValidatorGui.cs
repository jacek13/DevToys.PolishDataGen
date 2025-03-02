using System.ComponentModel.Composition;
using DevToys.Api;
using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Common;
using DevToys.PolishDataGen.Providers.Validators;
using static DevToys.Api.GUI;

namespace DevToys.PolishDataGen.Gui;

[Export(typeof(IGuiTool))]
[Name(nameof(PolishDataValidatorGui))]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uF86A',
    GroupName = PredefinedCommonToolGroupNames.Testers,
    ResourceManagerAssemblyIdentifier = nameof(PolishDataGenResourceAssemblyIdentifier),
    ResourceManagerBaseName = "DevToys.PolishDataGen.Strings.PolishDataGen",
    ShortDisplayTitleResourceName = nameof(Strings.PolishDataGen.GuiValidationShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(Strings.PolishDataGen.GuiValidationLongDisplayTitle),
    DescriptionResourceName = nameof(Strings.PolishDataGen.GuiValidationDescription),
    AccessibleNameResourceName = nameof(Strings.PolishDataGen.AccessibleName))]
internal class PolishDataValidatorGui : IGuiTool
{
    private readonly IUISelectDropDownList _dropdownList;
    private readonly IUISingleLineTextInput _textInput;
    private readonly IUIStack _validationContainer;

    private IdType _validatorType = IdType.Pesel;
    private IPolishIdValidator _validator;

    public UIToolView View =>
        new UIToolView(
            Stack()
                .Vertical()
                .LargeSpacing()
                .WithChildren(
                    _dropdownList,
                    _textInput,
                    _validationContainer
                )
        );

    public PolishDataValidatorGui()
    {
        _dropdownList = SelectDropDownList()
            .AlignHorizontally(UIHorizontalAlignment.Left)
            .Title(Strings.PolishDataGen.GuiValidationSelectTypeLabel)
            .WithItems(
                Item(text: Strings.PolishDataGen.GuiIdTypeNip, value: IdType.Nip),
                Item(text: Strings.PolishDataGen.GuiIdTypePesel, value: IdType.Pesel),
                Item(text: Strings.PolishDataGen.GuiIdTypeRegon, value: IdType.Regon),
                Item(text: Strings.PolishDataGen.GuiIdTypeIdentityCardNumber, value: IdType.PolishIdentityCard)
            )
            .Select(1)
            .OnItemSelected(OnValidatorTypeSelected);

        _textInput = SingleLineTextInput()
            .Title(Strings.PolishDataGen.GuiValidationTextInputLabel)
            .OnTextChanged(OnInputTextChanged);

        _validationContainer = Stack()
            .Vertical()
            .LargeSpacing();

        _validator = ValidatorFactory.Create(_validatorType);
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }

    private void OnInputTextChanged(string text)
    {
        _validator = ValidatorFactory.Create(_validatorType);
        var (isValid, errorMessages) = _validator.IsValidExplained(text);

        _validationContainer.WithChildren();
        if (isValid)
        {
            _validationContainer.WithChildren(
                InfoBar()
                    .Success()
                    .Title(Strings.PolishDataGen.GuiValidationSuccessTitle)
                    .Description($"'{text}' {Strings.PolishDataGen.GuiValidationIsValidLabel}")
                    .Open()
            );
        }
        else
        {
            var errorInfoBars = errorMessages
                .Select(error =>
                    InfoBar()
                        .Error()
                        .Title(Strings.PolishDataGen.GuiValidationErrorTitle)
                        .Description($"'{text}' {Strings.PolishDataGen.GuiValidationIsInvalidLabel}: {error}")
                        .Open()
                )
                .ToArray();

            _validationContainer.WithChildren(errorInfoBars);
        }
    }

    private void OnValidatorTypeSelected(IUIDropDownListItem? uIDropDownListItem)
    {
        if (uIDropDownListItem?.Value is IdType type)
        {
            _validatorType = type;
            if (!string.IsNullOrWhiteSpace(_textInput.Text))
            {
                OnInputTextChanged(_textInput.Text);
            }
        }
    }
}

