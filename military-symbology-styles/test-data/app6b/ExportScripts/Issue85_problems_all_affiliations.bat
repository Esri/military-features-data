cd %~dp0\..\RuntimeSymbolExport\bin\Debug

REM # Test APP6B issues raised in #85, all affiliations

REM Machine gun test - known good symbols/SIC

RuntimeSymbolExport.exe app6b SFG-UCATW------ Friend Issue 215
RuntimeSymbolExport.exe app6b SFGPEVATW------- Friend Issue 215 

RuntimeSymbolExport.exe app6b SFGPUUP-------- Friend Landing Support 
RuntimeSymbolExport.exe app6b SHGPUUP-------- Hostile Landing Support 
RuntimeSymbolExport.exe app6b SNGPUUP-------- Neutral Landing Support 
RuntimeSymbolExport.exe app6b SUGPUUP-------- Unknown Landing Support 

RuntimeSymbolExport.exe app6b SFGPEWRH------- Heavy Machine Gun

RuntimeSymbolExport.exe app6b SFG-UCFRS------ Friend SINGLE ROCKET LAUNCHER
RuntimeSymbolExport.exe app6b SHG-UCFRS------ Hostile SINGLE ROCKET LAUNCHER
RuntimeSymbolExport.exe app6b SNG-UCFRS------ Neutral SINGLE ROCKET LAUNCHER
RuntimeSymbolExport.exe app6b SUG-UCFRS------ Unknown SINGLE ROCKET LAUNCHER

RuntimeSymbolExport.exe app6b SFGPUCVRUE----- Friend MEDEVAC ROTARY WING
RuntimeSymbolExport.exe app6b SHGPUCVRUE-----  Hostile MEDEVAC ROTARY WING
RuntimeSymbolExport.exe app6b SNGPUCVRUE----- Neutral MEDEVAC ROTARY WING
RuntimeSymbolExport.exe app6b SUGPUCVRUE----- Unknown MEDEVAC ROTARY WING

RuntimeSymbolExport.exe app6b SFGPUCFMSW----- Friend SP WHEELED MORTAR
RuntimeSymbolExport.exe app6b SHGPUCFMSW----- Hostile SP WHEELED MORTAR
RuntimeSymbolExport.exe app6b SNGPUCFMSW----- Neutral SP WHEELED MORTAR
RuntimeSymbolExport.exe app6b SUGPUCFMSW----- Unknown SP WHEELED MORTAR

RuntimeSymbolExport.exe app6b SFGPUCRLL------ Friend RECONNAISSANCE LIGHT
RuntimeSymbolExport.exe app6b SHGPUCRLL------ Hostile RECONNAISSANCE LIGHT
RuntimeSymbolExport.exe app6b SNGPUCRLL------ Neutral RECONNAISSANCE LIGHT
RuntimeSymbolExport.exe app6b SUGPUCRLL------ Unknown RECONNAISSANCE LIGHT

RuntimeSymbolExport.exe app6b SFGPUSAW------- Friend MORAL, WELFARE, RECREATION(MWR)
RuntimeSymbolExport.exe app6b SHGPUSAW------- Hostile MORAL, WELFARE, RECREATION(MWR)
RuntimeSymbolExport.exe app6b SNGPUSAW------- Neutral MORAL, WELFARE, RECREATION(MWR)
RuntimeSymbolExport.exe app6b SUGPUSAW------- Unknown MORAL, WELFARE, RECREATION(MWR)

RuntimeSymbolExport.exe app6b SFGPUSAWT------ Friend MWR THEATRE
RuntimeSymbolExport.exe app6b SHGPUSAWT------ Hostile MWR THEATRE
RuntimeSymbolExport.exe app6b SNGPUSAWT------ Neutral MWR THEATRE
RuntimeSymbolExport.exe app6b SUGPUSAWT------ Unknown MWR THEATRE

RuntimeSymbolExport.exe app6b SFGPUSAWC------ Friend MWR CORPS
RuntimeSymbolExport.exe app6b SHGPUSAWC------ Hostile MWR CORPS
RuntimeSymbolExport.exe app6b SNGPUSAWC------ Neutral MWR CORPS
RuntimeSymbolExport.exe app6b SUGPUSAWC------ Unknown MWR CORPS

RuntimeSymbolExport.exe app6b SFGPGL--------- Friend UNIT GENERAL LIAISON
RuntimeSymbolExport.exe app6b SHGPGL--------- Hostile UNIT GENERAL LIAISON
RuntimeSymbolExport.exe app6b SNGPGL--------- Neutral UNIT GENERAL LIAISON
RuntimeSymbolExport.exe app6b SUGPGL--------- Unknown UNIT GENERAL LIAISON

RuntimeSymbolExport.exe app6b SFGPEVATW------ Friend TANK LIGHT RECOVERY
RuntimeSymbolExport.exe app6b SHGPEVATW------ Hostile TANK LIGHT RECOVERY
RuntimeSymbolExport.exe app6b SNGPEVATW------ Neutral TANK LIGHT RECOVERY
RuntimeSymbolExport.exe app6b SUGPEVATW------ Unknown TANK LIGHT RECOVERY

RuntimeSymbolExport.exe app6b SFGPEVATX------ Friend TANK MEDIUM RECOVERY
RuntimeSymbolExport.exe app6b SHGPEVATX------ Hostile TANK MEDIUM RECOVERY
RuntimeSymbolExport.exe app6b SNGPEVATX------ Neutral TANK MEDIUM RECOVERY
RuntimeSymbolExport.exe app6b SUGPEVATX------ Unknown TANK MEDIUM RECOVERY

RuntimeSymbolExport.exe app6b SFGPEVATY------ Friend TANK HEAVY RECOVERY
RuntimeSymbolExport.exe app6b SHGPEVATY------ Hostile TANK HEAVY RECOVERY
RuntimeSymbolExport.exe app6b SNGPEVATY------ Neutral TANK HEAVY RECOVERY
RuntimeSymbolExport.exe app6b SUGPEVATY------ Unknown TANK HEAVY RECOVERY

RuntimeSymbolExport.exe app6b SFGPIRSR---H--- Friend SEA SURFACE INSTALLATION, OIL RIG / PLATFORM
RuntimeSymbolExport.exe app6b SHGPIRSR---H--- Hostile SEA SURFACE INSTALLATION, OIL RIG / PLATFORM
RuntimeSymbolExport.exe app6b SNGPIRSR---H--- Neutral SEA SURFACE INSTALLATION, OIL RIG / PLATFORM
RuntimeSymbolExport.exe app6b SUGPIRSR---H--- Unknown SEA SURFACE INSTALLATION, OIL RIG / PLATFORM

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause