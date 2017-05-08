---
external help file: Pansies.dll-Help.xml
online version: 
schema: 2.0.0
---

## SYNOPSIS
Backwards compatible Write-Host replacement which writes customized output to a host, but using full RGB color values.

## SYNTAX

```
Write-Host [[-Object] <Object>] [-NoNewline] [-Separator <Object>] [-ForegroundColor <RgbColor>] [-BackgroundColor <RgbColor>]
```

## DESCRIPTION
The **Write-Host** cmdlet customizes output.
You can specify the color of text by using the *ForegroundColor* parameter, and you can specify the background color by using the *BackgroundColor* parameter.
The *Separator* parameter lets you specify a string to use to separate displayed objects.
The particular result depends on the program that is hosting Windows PowerShell.

## EXAMPLES

### Example 1: Write to the console and include a separator
```
PS C:\> Write-Host (2,4,6,8,10,12) -Separator ", +2= "
2, +2= 4, +2= 6, +2= 8, +2= 10, +2= 12
```

This command displays the even numbers from 2 through 12.
The *Separator* parameter is used to add the string ", +2= " (comma, space, +, 2, =, space).

### Example 2: Write with different text and background colors
```
PS C:\> Write-Host (2,4,6,8,10,12) -Separator ", -> " -ForegroundColor DarkGreen -BackgroundColor white
```

This command displays the even numbers from 2 through 12.
It uses the *ForegroundColor* parameter to output dark green text and the *BackgroundColor* parameter to display a white background.

### Example 3: Write with different text and background colors
```
PS C:\> Write-Host "Red on white text." -ForegroundColor "#FF0000" -BackgroundColor "#FFFFFF"
Red on white text.
```

This command displays the string "Red on white text." The text is red, as defined by the *ForegroundColor* parameter.
The background is white, as defined by the *BackgroundColor* parameter.

## PARAMETERS

### -BackgroundColor
Specifies the background color. 
There is no default.
The acceptable values for this parameter are:

CSS Hex representations of RGB colors like "#FF00FF" or "FF00FF"

XTerm indexes like "xt138" or "123"

Console Color names:

- Black
- DarkBlue
- DarkGreen
- DarkCyan
- DarkRed
- DarkMagenta
- DarkYellow
- Gray
- DarkGray
- Blue
- Green
- Cyan
- Red
- Magenta
- Yellow
- White

```yaml
Type: RgbColor
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ForegroundColor
Specifies the text color.
There is no default.
The acceptable values for this parameter are:

CSS Hex representations of RGB colors like "#FF00FF" or "FF00FF"

XTerm indexes like "xt138" or "123"

Console Color names:

- Black
- DarkBlue
- DarkGreen
- DarkCyan
- DarkRed
- DarkMagenta
- DarkYellow
- Gray
- DarkGray
- Blue
- Green
- Cyan
- Red
- Magenta
- Yellow
- White

```yaml
Type: RgbColor
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoNewline
Specifies that the content displayed in the console does not end with a newline character.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Object
Specifies objects to display in the console.

```yaml
Type: Object
Parameter Sets: (All)
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Separator
Specifies a separator string to the output between objects displayed on the console.

```yaml
Type: Object
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Object
You can pipe objects to be written to the host.

## OUTPUTS

### None
**Write-Host** sends the objects to the host.
It does not return any objects.
However, the host might display the objects that **Write-Host** sends to it.

## NOTES

## RELATED LINKS

[Clear-Host](../Microsoft.PowerShell.Core/Functions/Clear-Host.md)

[Write-Debug](Write-Debug.md)

[Write-Error](Write-Error.md)

[Write-Output](Write-Output.md)

[Write-Progress](Write-Progress.md)

[Write-Verbose](Write-Verbose.md)

[Write-Warning](Write-Warning.md)


