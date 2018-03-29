cd %~dp0\..\RuntimeSymbolExport\bin\Debug
REM # APP6B Issue 73

RuntimeSymbolExport.exe app6b SFA-MFO-------- MEDEVAC
RuntimeSymbolExport.exe app6b SFAPMFRZ------- ELECTRONIC SURVEILLANCE MEASURES
RuntimeSymbolExport.exe app6b SFAPMFPN------- ANTI SURFACE WARFARE/ASUW
RuntimeSymbolExport.exe app6b SFAPWMAS------- AIR TO SURFACE MISSILE (ASM)
RuntimeSymbolExport.exe app6b SFAPWMAA------- AIR TO AIR MISSILE (AAM)

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause