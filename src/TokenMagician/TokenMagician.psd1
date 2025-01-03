﻿@{
    # Script module or binary module file associated with this manifest.
    # This loads the psm1 file, which in turn loads the TokenMagician.Loader.dll which will then load the actual module.
    # This is to setup the Assembly Load Context for the module and its dependencies.
    RootModule = 'TokenMagician.dll'

    # Version number of this module.
    ModuleVersion = '0.1.0'

    # ID used to uniquely identify this module.
    GUID = '9b702a14-27c1-45cb-a690-71c537fc31be'

    # Author of this module.
    Author = 'Stephan van Rooij (@svrooij)'

    # Company or vendor that produced this module.
    CompanyName = 'Stephan van Rooij'

    Copyright = 'Stephan van Rooij 2024, licensed under GNU GPLv3'

    # Description of this module.
    Description = 'Get multi tenant tokens using managed identity.'

    # Minimum version of the Windows PowerShell engine required by this module.
    PowerShellVersion = '7.2'

    # Minimum version of the .NET Framework required by this module.
    # DotNetFrameworkVersion = '4.7.2'

    # Processor architecture (None, X86, Amd64) supported by this module.
    # ProcessorArchitecture = 'None'

    # Modules that must be imported into the global environment prior to importing this module.
    # RequiredModules = @()

    # Assemblies that must be loaded prior to importing this module.
    # RequiredAssemblies = @(
    #     "Microsoft.Extensions.Logging.Abstractions.dll",
    #     "SvR.ContentPrep.dll",
    #     "System.Buffers.dll",
    #     "System.Memory.dll",
    #     "System.Numerics.Vectors.dll",
    #     "System.Runtime.CompilerServices.Unsafe.dll"
    # )

    # Script files (.ps1) that are run in the caller's environment prior to importing this module.
    # ScriptsToProcess = @()

    # Type files (.ps1xml) that are loaded into the session prior to importing this module.
    # TypesToProcess = @()

    # Format files (.ps1xml) that are loaded into the session prior to importing this module.
    # FormatsToProcess = @()

    # Modules to import as nested modules of the module specified in RootModule/ModuleToProcess.
    # NestedModules = @()

    # Functions to export from this module.
    # FunctionsToExport = @()

    # Cmdlets to export from this module.
    # CmdletsToExport = @(
    #     "Get-TmMsiToken"
    # )

    # Variables to export from this module.
    # VariablesToExport = @()

    # Aliases to export from this module.
    # AliasesToExport = @(
    #     "Get-MsiToken"
    # )

    # List of all files included in this module.
    # FileList = @(
    #     "TokenMagician.psd1",
    #     "TokenMagician.psm1",
    #     "TokenMagician.dll-Help.xml",
    #     "TokenMagician.dll"
    # )

    # Private data to pass to the module specified in RootModule/ModuleToProcess.
    PrivateData = @{
        PSData = @{
            Tags = @('Tokens', 'Managed-Identity', 'Multi-Tenant', 'EntraID')

            LisenceUri = 'https://github.com/svrooij/TokenMagician/blob/main/LICENSE.txt'
            ProjectUri = 'https://github.com/svrooij/TokenMagician/'
            ReleaseNotes = 'Get multi tenant tokens using managed identity'
        }
    }

    # HelpInfo URI of this module.
    HelpInfoURI = 'https://github.com/svrooij/TokenMagician/'
}