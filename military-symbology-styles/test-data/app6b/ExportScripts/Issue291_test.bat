cd %~dp0\..\RuntimeSymbolExport\bin\Debug
#Test data for issue 291

RuntimeSymbolExport.exe app6b GFCPBOAIC------ COMPLETED MINEFIELD OID   766
RuntimeSymbolExport.exe app6b GFCPBOAIP------ PLANNED MINEFIELD OID   765
RuntimeSymbolExport.exe app6b SFGPUCFRS------ SINGLE ROCKET LAUNCHER OID   189
RuntimeSymbolExport.exe app6b SFGPUCFMSW----- SP WHEELED MORTAR OID   207
RuntimeSymbolExport.exe app6b SFGPUCRLL------ RECONNAISSANCE LIGHT OID   237
RuntimeSymbolExport.exe app6b SFGPUSAW------- MORAL WELFARE RECREATION (MWR) OID   358
RuntimeSymbolExport.exe app6b SFGPUSAWT------ MWR THEATRE OID   359
RuntimeSymbolExport.exe app6b SFGPUSAWC------ MWR CORPS OID   360
RuntimeSymbolExport.exe app6b SFGPEVATW------ TANK LIGHT RECOVERY OID   535
RuntimeSymbolExport.exe app6b SFGPEVATX------ TANK MEDIUM RECOVERY OID   537
RuntimeSymbolExport.exe app6b SFGPEVATY------ TANK HEAVY RECOVERY OID   539
RuntimeSymbolExport.exe app6b S-UPNBR-------- SEABED ROCK/STONE OBSTACLE OTHER OID   701
RuntimeSymbolExport.exe app6b S-UPNBW-------- WRECK OID   702
RuntimeSymbolExport.exe app6b S-UPNM--------- MARINE LIFE OID   703
RuntimeSymbolExport.exe app6b S-UPNA--------- SEA ANOMALY OID   704
RuntimeSymbolExport.exe app6b GFCPBOAIN------ ANTITANK (AT) MINEFIELD OID   767
RuntimeSymbolExport.exe app6b GFCPBOAIS------ SCATTERABLE MINES OID   768
RuntimeSymbolExport.exe app6b GFCPBOAIH------ ANTIPERSONNEL (AP) MINEFIELD REIN OID   769
RuntimeSymbolExport.exe app6b GFCPBOAID------ SCATTERABLE MINEFIELD WITH SELF-D OID   770
RuntimeSymbolExport.exe app6b GFCPBOAV------- EXECUTED VOLCANO MINEFIELD OID   771
RuntimeSymbolExport.exe app6b GFCPBSE-------- FOXHOLE EMPLACEMENT OR WEAPON SIT OID   775
RuntimeSymbolExport.exe app6b GFCPBWM-------- MINIMUM SAFE DISTANCE ZONES OID   778
RuntimeSymbolExport.exe app6b GFOPPHG-------- WRITTEN PROPAGANDA OID   928
RuntimeSymbolExport.exe app6b GHOPPCU-------- RECRUITMENT (COERCED/IMPRESSED) OID   922
RuntimeSymbolExport.exe app6b GHCPMOLAE------ Axis of Advance ENEMY CONFIRMED OID 63
RuntimeSymbolExport.exe app6b GHCPMOLAT------ Axis of Advance ENEMY TEPLATED OID 64

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause