
cd %~dp0\..\RuntimeSymbolExport\bin\Debug

RuntimeSymbolExport.exe app6b SJADMF-----N***
RuntimeSymbolExport.exe app6b SFACCH----HB***
RuntimeSymbolExport.exe app6b SDACW-----F-***
RuntimeSymbolExport.exe app6b SDGDEVATH--D***
RuntimeSymbolExport.exe app6b SDUCWM-----C***
RuntimeSymbolExport.exe app6b ODOPHT-----A***

RuntimeSymbolExport.exe app6b SFAPMF-----N***
RuntimeSymbolExport.exe app6b SFACCH----HB***
RuntimeSymbolExport.exe app6b SFACW-----F-***
RuntimeSymbolExport.exe app6b SFGDEVATH--D***
RuntimeSymbolExport.exe app6b SFUCWM-----C***
RuntimeSymbolExport.exe app6b ODOPHT-----A***

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause