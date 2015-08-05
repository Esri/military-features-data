# replaces multiple strings in a file (requires Powershell installed)

Param(
    [parameter(Mandatory=$true)]
    [alias("replacefile")]
    $source_file
 )

Write-Host "Checking SourceFile = $source_file"
 
 # These are all of the various "stroke-dasharray" values that were causing problems:
(Get-Content $source_file) | Foreach-Object { 
$_ -replace 'stroke-dasharray="22.1063,13.2638"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="23.3682,14.0209"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="23.4314,14.0588"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="23.5714,14.1429"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="24.1702,14.5021"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="24.5874,14.7524"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="24.267,14.5602"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="25.4169,15.2501"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="25.7843,15.4706"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="25.5417,15.325"', 'stroke-dasharray="75,45"' `
 -replace 'stroke-dasharray="25,15"', 'stroke-dasharray="75,45"'
 } | Out-File $source_file -encoding ASCII
 