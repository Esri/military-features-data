cd %~dp0\..\RuntimeSymbolExport\bin\Debug

REM # Test APP6B issues raised in #88, all affiliations



RuntimeSymbolExport.exe app6b SFUPSO---------   Friend
RuntimeSymbolExport.exe app6b SHUPSO---------   Hostile
RuntimeSymbolExport.exe app6b SNUPSO---------   Neutral
RuntimeSymbolExport.exe app6b SUUPSO---------   Unknown

RuntimeSymbolExport.exe app6b SFSPNR---------   Friend
RuntimeSymbolExport.exe app6b SHSPNR---------   Hostile
RuntimeSymbolExport.exe app6b SNSPNR---------   Neutral
RuntimeSymbolExport.exe app6b SUSPNR---------   Unknown

RuntimeSymbolExport.exe app6b SFSPPNR--------   Friend
RuntimeSymbolExport.exe app6b SHSPPNR--------   Hostile
RuntimeSymbolExport.exe app6b SNSPPNR--------   Neutral
RuntimeSymbolExport.exe app6b SUSPPNR--------   Unknown

RuntimeSymbolExport.exe app6b SFSPCPSU-------   Friend
RuntimeSymbolExport.exe app6b SHSPCPSU-------   Hostile
RuntimeSymbolExport.exe app6b SNSPCPSU-------   Neutral
RuntimeSymbolExport.exe app6b SUSPCPSU-------   Unknown



cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause