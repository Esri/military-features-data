
cd %~dp0\..\RuntimeSymbolExport\bin\Debug

RuntimeSymbolExport.exe mil2525d 10134600001203040000

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause