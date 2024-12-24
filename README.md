# TokenMagician 🪄

[![PowerShell] gallery version][badge_powershell]][link_powershell]
[![PowerShell gallery downloads][badge_powershell_downloads]][link_powershell]
[![License][badge_license]][link_license]

A PowerShell module that will help you get tokens from Entra ID using a managed identity as federated credential [PowerShell Gallery][link_powershell].

## Installation

You can install the module from the PowerShell Gallery by running the following command:

```powershell
Install-Module -Name TokenMagician
```

## Usage

```powershell
# Import the module
Import-Module TokenMagician

# Get a token
$token = Get-TmMsiToken -TenantId 'your-tenant-id' -ClientId 'your-client-id' -Scope 'https://graph.microsoft.com/.default'

# Use the token
Connect-MgGraph -AccessToken $token
```

[badge_license]: https://img.shields.io/github/license/svrooij/TokenMagician?style=for-the-badge
[link_license]: https://github.com/svrooij/TokenMagician/blob/main/LICENSE.txt
[badge_powershell]: https://img.shields.io/powershellgallery/v/TokenMagician?style=for-the-badge&logo=powershell&logoColor=white
[badge_powershell_downloads]: https://img.shields.io/powershellgallery/dt/TokenMagician?style=for-the-badge&logo=powershell&logoColor=white
[link_powershell]: https://www.powershellgallery.com/packages/TokenMagician/