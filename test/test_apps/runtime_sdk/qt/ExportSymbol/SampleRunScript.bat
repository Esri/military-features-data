echo off

REM Disable SSL warning you get with open source Qt and AGS Runtime
SET QT_LOGGING_RULES=qt.network.ssl.warning=false

REM Add Qt and AGS Runtime to Path
SET PATH=%PATH%;C:\Qt\5.7\msvc2015_64\bin
SET PATH=%PATH%;C:\Program Files (x86)\ArcGIS SDKs\Qt100.0\sdk\windows\x64\bin\debug

REM IMPORTANT: You will likely need to change the build folder
cd ..\build-ExportSymbol-Desktop_Qt_5_7_0_MSVC2015_64bit-Debug\debug

REM Simple Test
ExportSymbol.exe

REM Full Test of every symbol
REM ExportSymbol.exe FILE .\Data\All_Entities_SIDCs.csv

pause
