cd %~dp0\..\RuntimeSymbolExport\bin\Debug
#Fixes Test Bat

RuntimeSymbolExport.exe app6b S-UPNM--------- MARINE LIFE POINT #changed from SFUPNM--------- in the test data

RuntimeSymbolExport.exe app6b S-UPNBW-------- WRECK POINT S-UPNBW--------  #changed from SFUPNBW--------
RuntimeSymbolExport.exe app6b S-UPNBR-------- SEABED ROCK/STONE OBSTACLE OTHER  POINT S-UPNBR--------  #changed from SFUPNBR-------- 
RuntimeSymbolExport.exe app6b S-UPNA--------- SEA ANOMALY POINT SFUPNA---------  #changed from SFUPNA--------- SEA ANOMALY POINT SFUPNA---------

RuntimeSymbolExport.exe app6b GHOPPCU-------- RECRUITMENT (COERCED/IMPRESSED) POINT GHOPPCU--------  #fixed tag in stylx, was missing the C

RuntimeSymbolExport.exe app6b GFCPMOOTF------ FRIENDLY ATTACK POSITION AREA GFCPMOOTF------  # Added new symbol to style for GFCPMOOTF------ FRIENDLY ATTACK POSITION AREA GFCPMOOTF------, dup of GFCPMOOT-------

RuntimeSymbolExport.exe app6b GHCPMOLAE------ ENEMY CONFIRMED LINE GHCPMOLAE------  #ENEMY CONFIRMED Axis of Attack LINE GHCPMOLAE  Added new symbol to style
RuntimeSymbolExport.exe app6b GHCPMOLAT------ ENEMY TEMPLATED LINE GHCPMOLAT------  #ENEMY TEMPLATED Axis of Attack GHCPMOLAT------  Added new symbol to style
RuntimeSymbolExport.exe app6b SFGPUUMSEJ----- JAMMING POINT SFGPUUMSEJ-----   #for some reason the PNG of this point is drawing incorrectly. Frame too small, wavy lines extend beyond edges

RuntimeSymbolExport.exe app6b SFGPUCVC------- COMPOSITE POINT SFGPUCVC------- #Symbol draws incorrectly SFGPUCVC-------.png 
RuntimeSymbolExport.exe app6b GFCPBSE-------- EARTHWORK SMALL TRENCH OR FORTIFICATION, FOXHOLE EMPLACEMENT OR WEAPON SITE    POINT GFCPBSE--------

# GFCPBSW-------- no symbol in standard - remove. There is a mention in table B-IX on page B-39 (141 of pdf) but no corresponding symbol depicted in the standard.
# we do have a GFCPBSE-------- Foxhole, Emplacement, or Weapon Site symbol that is drawing correctly.
# Delete GFCPBSW from test data.
# GFCPBWA-------- no symbol in standard - remove. There is a GFCPBWR--------   Area symbol in the standard doc, and there is a G*C*BWR--- contour lines dose rate symbol. 
# Duplicate symbol id code.
# GFOPPHG-------- WRITTEN PROPAGANDA POINT GFOPPHG--------  #this is failing to draw GFOPPHG--------.png but GFOPPHW--------.png is drawing. APP6b error?
#Drawing Problems




cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause