Describe 'Get-TmMsiToken' {
  It 'Should be available' {
      $cmdlet = Get-Command -Name 'Get-TmMsiToken'
      $cmdlet.CommandType | Should -Be 'Cmdlet'
  }

  It 'Should throw an error if no arguments are provided' {
    { Get-TmMsiToken } | Should -Throw
  }

  It 'Should throw an error if the Client ID is not a GUID' {
    { Get-TmMsiToken -ClientId 'not-a-guid' -Tenant xxx -Scope https://graph.microsoft.com./default } | Should -Throw
  }
}