cd %~dp0\..\RuntimeSymbolExport\bin\Debug

REM # All Frames (Position 2)	
RuntimeSymbolExport.exe app6b SPGPUCI--------	PENDING
RuntimeSymbolExport.exe app6b SUGPUCI--------	UNKNOWN
RuntimeSymbolExport.exe app6b SAGPUCI--------	ASSUMED FRIEND
RuntimeSymbolExport.exe app6b SNGPUCI--------	NEUTRAL
RuntimeSymbolExport.exe app6b SHGPUCI--------	HOSTILE
RuntimeSymbolExport.exe app6b SSGPUCI--------	SUSPECT
RuntimeSymbolExport.exe app6b SJGPUCI--------	JOKER
RuntimeSymbolExport.exe app6b SKGPUCI--------   FAKER
RuntimeSymbolExport.exe app6b SOGPUCI--------   NONE

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause