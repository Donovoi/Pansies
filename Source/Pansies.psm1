Import-Module $PSScriptRoot\lib\Pansies.dll

if(-not $IsLinux) {
    [PoshCode.Pansies.Console.WindowsHelper]::EnableVirtualTerminalProcessing()
}

# dot source the functions
(Join-Path $PSScriptRoot Private\*.ps1 -Resolve -ErrorAction SilentlyContinue).ForEach{ . $_ }
(Join-Path $PSScriptRoot Public\*.ps1 -Resolve).ForEach{ . $_ }


$Pansies = @{
    fg    = @{}
    bg    = @{}
    esc   = [PoshCode.Pansies.Entities]::EscapeSequences
    nf    = [PoshCode.Pansies.Entities]::NerdFontSymbols
    emoji = [PoshCode.Pansies.Entities]::Emoji
    extra = [PoshCode.Pansies.Entities]::ExtendedCharacters
}

Get-ChildItem Fg: | ForEach-Object {
    $Pansies.fg[$_.Name] = Get-Content $_
}
Get-ChildItem Bg: | ForEach-Object {
    $Pansies.bg[$_.Name] = Get-Content $_
}

Export-ModuleMember -Variable Pansies -Function * -Cmdlet * -Alias * -Variable RgbColorCompleter
