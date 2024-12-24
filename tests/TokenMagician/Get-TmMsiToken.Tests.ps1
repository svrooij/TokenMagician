Describe 'Get-TmMsiToken' {
  It 'Should be available' {
      $cmdlet = Get-Command -Name 'Get-TmMsiToken'
      $cmdlet.CommandType | Should -Be 'Cmdlet'
  }
}