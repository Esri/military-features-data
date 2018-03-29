cd %~dp0\..\RuntimeSymbolExport\bin\Debug

REM # Equipment Mobility Modifiers
RuntimeSymbolExport.exe app6b SFGPEWM---MO---   MOBILITY WHEELED/LIMITED CROSS COUNTRY
RuntimeSymbolExport.exe app6b SFGPEWM---MP---   MOBILITY CROSS COUNTRY
RuntimeSymbolExport.exe app6b SFGPEWM---MQ---   MOBILITY TRACKED
RuntimeSymbolExport.exe app6b SFGPEWM---MR---   MOBILITY WHEELED AND TRACKED COMBINATION
RuntimeSymbolExport.exe app6b SFGPEWM---MS---	MOBILITY TOWED
RuntimeSymbolExport.exe app6b SFGPEWM---MT---   MOBILITY RAIL
RuntimeSymbolExport.exe app6b SFGPEWM---S----   MOBILITY SPACE
RuntimeSymbolExport.exe app6b SFGPEWM---MM---   MOBILITY MISSILE
RuntimeSymbolExport.exe app6b SFGPEWM---MH---   MOBILITY HELICOPTER
RuntimeSymbolExport.exe app6b SFGPEWM---M----   MOBILITY EQUIPMENT
RuntimeSymbolExport.exe app6b SFGPEWM---MF---	MOBILITY FIXED WING


cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause