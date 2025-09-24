<#
    Pester tests validating new cursor commands, position helper, and Expand-Variable functionality.
#>

BeforeAll {
    $scriptPath = if ($PSCommandPath) { $PSCommandPath } elseif ($MyInvocation.MyCommand.Path) { $MyInvocation.MyCommand.Path } else { $null }
    if (-not $scriptPath) {
        throw 'Unable to determine script path for test execution.'
    }

    $testsFolder = Split-Path -Parent $scriptPath
    $moduleRoot = Split-Path -Parent $testsFolder
    $script:PublishPath = Join-Path $moduleRoot ('.pester-publish-' + [Guid]::NewGuid().ToString('N'))

    New-Item -ItemType Directory -Path $script:PublishPath | Out-Null

    Push-Location $moduleRoot
    try {
        $publishResult = dotnet publish -c Release -o $script:PublishPath 2>&1
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet publish failed:`n$publishResult"
        }
    }
    finally {
        Pop-Location
    }

    $path = Join-Path $script:PublishPath 'Pansies.dll'
    Import-Module $path -Force
    Set-Variable -Name ModuleAssemblyPath -Scope Script -Value $path
}

AfterAll {
    Remove-Module Pansies -Force -ErrorAction SilentlyContinue
    [System.GC]::Collect()
    [System.GC]::WaitForPendingFinalizers()

    if ($script:PublishPath -and (Test-Path -LiteralPath $script:PublishPath)) {
        Remove-Item -LiteralPath $script:PublishPath -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Describe 'Position helper' {
    It 'formats absolute positions with row and column' {
        $position = [PoshCode.Pansies.Position]::new(3, 4, $true)
        $position.ToString() | Should -Be (([char]0x1b) + "[3;4H")
    }

    It 'formats absolute positions with only column' {
        $position = [PoshCode.Pansies.Position]::new($null, 12, $true)
        $position.ToString() | Should -Be (([char]0x1b) + "[12G")
    }

    It 'formats relative movement' {
        $position = [PoshCode.Pansies.Position]::new(-2, 5, $false)
        $position.ToString() | Should -Be (([char]0x1b) + "[5C") # column takes precedence when positive
    }

    It 'formats relative movement vertically' {
        $position = [PoshCode.Pansies.Position]::new(-3, $null, $false)
        $position.ToString() | Should -Be (([char]0x1b) + "[3A")
    }

    It 'round trips metadata' {
        $original = [PoshCode.Pansies.Position]::new(7, 9, $true)
        $metadata = $original.ToPsMetadata()
        $metadata | Should -Be '7;9;1'

        $roundTrip = [PoshCode.Pansies.Position]::new($metadata)
        $roundTrip.Absolute | Should -BeTrue
        $roundTrip.Line | Should -Be 7
        $roundTrip.Column | Should -Be 9
    }

    It 'throws when metadata is null' {
        $position = [PoshCode.Pansies.Position]::new()
        try {
            $position.FromPsMetadata($null)
            $true | Should -BeFalse -Because 'Expected a MethodInvocationException wrapping ArgumentNullException'
        }
        catch [System.Management.Automation.MethodInvocationException] {
            $_.Exception.InnerException | Should -BeOfType ([System.ArgumentNullException])
        }
    }
}

Describe 'Set-CursorPosition cmdlet' {
    It 'emits absolute escape sequence' {
        $result = Set-CursorPosition -Line 2 -Column 5 -Absolute
        $result | Should -Be (([char]0x1b) + "[2;5H")
    }

    It 'emits relative escape sequence when Line is negative' {
        $result = Set-CursorPosition -Line -4
        $result | Should -Be (([char]0x1b) + "[4A")
    }

    It 'emits relative escape sequence when Column is positive' {
        $result = Set-CursorPosition -Column 6
        $result | Should -Be (([char]0x1b) + "[6C")
    }
}

Describe 'Save/Restore-CursorPosition cmdlets' {
    It 'writes ESC[s to the console' {
        $writer = New-Object System.IO.StringWriter
        $original = [Console]::Out
        try {
            [Console]::SetOut($writer)
            Save-CursorPosition
        }
        finally {
            [Console]::SetOut($original)
        }

        $writer.ToString() | Should -Be (([char]0x1b) + "[s")
    }

    It 'writes ESC8 to the console' {
        $writer = New-Object System.IO.StringWriter
        $original = [Console]::Out
        try {
            [Console]::SetOut($writer)
            Restore-CursorPosition
        }
        finally {
            [Console]::SetOut($original)
        }

        $writer.ToString() | Should -Be (([char]0x1b) + '8')
    }
}

Describe 'Expand-Variable cmdlet' {
    BeforeEach {
        Set-Variable -Name foo -Value 'rainbow' -Scope Script
    }

    AfterEach {
        Remove-Variable -Name foo -Scope Script -ErrorAction SilentlyContinue
        Remove-Variable -Name bar -Scope Script -ErrorAction SilentlyContinue
    }

    It 'replaces variables outside expandable strings with quoted value' {
        $result = Expand-Variable -Content 'Write-Host $variable:foo' -Drive variable
        $result | Should -Be 'Write-Host "rainbow"'
    }

    It 'replaces variables within expandable strings without extra quotes' {
        $result = Expand-Variable -Content '"Color: $variable:foo"' -Drive variable
        $result | Should -Be '"Color: rainbow"'
    }

    It 'escapes characters by default' {
        Set-Variable -Name foo -Value 'He said "Go!"' -Scope Script
        $result = Expand-Variable -Content 'Write-Host $variable:foo' -Drive variable
        $result | Should -Be 'Write-Host "He said `"Go!`""'
    }

    It 'supports unescaped output when requested' {
        Set-Variable -Name foo -Value "Line1`nLine2" -Scope Script
        $result = Expand-Variable -Content 'Write-Host $variable:foo' -Drive variable -Unescaped
    $expected = "Write-Host `"Line1`nLine2`""
        $result | Should -Be $expected
    }

    It 'updates variables in place when requested' {
        Set-Variable -Name bar -Value 'azure' -Scope Script
        Set-Variable -Name foo -Value 'Write-Host $variable:bar' -Scope Script

        $result = Expand-Variable -Path 'variable:foo' -Drive variable -InPlace -Passthru

        $result | Should -Be 'Write-Host "azure"'
        (Get-Variable -Name foo -Scope Script).Value | Should -Be 'Write-Host "azure"'
    }
}
