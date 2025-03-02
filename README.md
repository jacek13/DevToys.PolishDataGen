## About project

**PolishDataGen** is a tool for **generating and validating** polish identification numbers. The tool was implemented as an extension to the [DevToys 2.0 preview](https://github.com/DevToys-app) application.

The current version supports generation and validation:
- [PESEL](https://pl.wikipedia.org/wiki/PESEL)
- [NIP](https://pl.wikipedia.org/wiki/Numer_identyfikacji_podatkowej)
- [REGON (9 digits)](https://pl.wikipedia.org/wiki/REGON)
- [REGON (14 digits)](https://romek.info/ut/nip-rego.html#regon)
- [Identity Card Number](https://romek.info/ut/paszport.html#dowodosobisty)

---

The extension works for both GUI and CLI versions of DevToys 2.0.

### CLI

The CLI version supports the generation of larger output sets than the GUI version. There is also an option to generate data concurrently, but this is a work in progress and at this point does not cause much benefit.

#### Generator - examples:

```sh
.\DevToys.CLI.exe pdg -t identity-card-number -n 100
.\DevToys.CLI.exe pdg -t nip -n 100000 -mt
.\DevToys.CLI.exe pdg --type identity-card-number --number 100
.\DevToys.CLI.exe polish-data-generator --type nip --number 10000 --multithreading
```

By default, the program prints the generated data to the terminal, but it is possible to use the `-o`/`--output` switch to define the path to the output folder containing the generated data. The name of the generated file has the format `generated-<NUMBER_OF_IDS>-<DATA_TYPE>-in-<PROCESSING_TIME>-ms.txt`.

```sh
.\DevToys.CLI.exe pdg -t pesel -n 38000000 -o destination
```

#### Validator - examples:

The `pdv`/`polish-data-validator` tool allows verification of a single identifier. The `-d`/`--detailed` switch returns a list of detailed errors. On the other hand, the `-ri`/`--return-integer` switch will cause the process to return an integer value (-1 validation error; 0 - success)

```sh
.\DevToys.CLI.exe pdv -t pesel -i 49144367520
.\DevToys.CLI.exe pdv --type pesel --input 49144367520

# Validation result for '49144367520' using 'Pesel': Invalid
```

```sh
.\DevToys.CLI.exe pdv -t pesel -i 49144367520 -d

# Validation result for '49144367520' using 'Pesel': Invalid
# 
# PESEL Month part must be in range 1 to 12
# PESEL Day part must be in range 1 to 31
# PESEL invalid control number
```

```sh
.\DevToys.CLI.exe pdv -t pesel -i 49144367520 -ri
$LASTEXITCODE

# prints -1
```

### GUI

Download [DevToys 2.0-preview](https://devtoys.app/download) and the DevToys.PolishDataGen nuget package (from the repository or from the NuGet Gallery) and then install the tool in the extensions management tab.

### Launching the project

Unit tests from **DevToys.PolishDataGen.Test.csproj** that verify generators and validators do not need DevToys 2.0 to work, but developing and debugging the **DevToys.PolishDataGen.csproj** requires the configuration described in the [DevToys application documentation](https://devtoys.app/doc/articles/extension-development/getting-started/setup.html?tabs=windows).