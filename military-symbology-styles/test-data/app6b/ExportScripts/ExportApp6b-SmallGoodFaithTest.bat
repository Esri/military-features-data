cd %~dp0\..\RuntimeSymbolExport\bin\Debug

REM # A good faith test of App6b Symbol Cases 
REM # Using Various Appendices and Modifiers	

REM # Test Bad Data	
RuntimeSymbolExport.exe app6b 123456789012345  Completely Bad (Invalid Symbol)    
RuntimeSymbolExport.exe app6b S234UCI--------  Only Good Coding Scheme, Function  (Invalid Symbol)
RuntimeSymbolExport.exe app6b SF34UCI--------  Bad Battle Dimension, Status (Invalid Symbol)
RuntimeSymbolExport.exe app6b SF3PUCI--------  Bad Battle Dimension (Invalid Symbol)
RuntimeSymbolExport.exe app6b SFG4UCI--------  Bad Status (Invalid Symbol)
RuntimeSymbolExport.exe app6b SFGP123--------  Bad Function Code (should still show frame)
		
REM # Basic Symbol and Frame	
RuntimeSymbolExport.exe app6b SFGPUCI--------	FRIEND
		
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

REM # Equipment Frame	
RuntimeSymbolExport.exe app6b SFGPEWM--------

		
REM # All Status (Position 4)	
RuntimeSymbolExport.exe app6b SFGPUCI--------	
RuntimeSymbolExport.exe app6b SFGAUCI--------	PLANNING/ANTICIPATED/DASHED FRAME

		
REM # Some Test Modifiers (Position 11)	
RuntimeSymbolExport.exe app6b SFGPUCI---A----	
RuntimeSymbolExport.exe app6b SFGPUCI---B----	
RuntimeSymbolExport.exe app6b SFGPUCI---C----	
RuntimeSymbolExport.exe app6b SFGPUCI---D----	
RuntimeSymbolExport.exe app6b SFGPUCI---E----	
RuntimeSymbolExport.exe app6b SFGPUCI---F----	

REM Mobility
RuntimeSymbolExport.exe app6b SFGPUCI---MF---	
RuntimeSymbolExport.exe app6b SFGPUCI---MH---	
RuntimeSymbolExport.exe app6b SFGPUCI---NM---	
		
REM # Echelon (Position 12) 
RuntimeSymbolExport.exe app6b SFGPUCI----A---	
RuntimeSymbolExport.exe app6b SFGPUCI----B---	
RuntimeSymbolExport.exe app6b SFGPUCI----C---	
RuntimeSymbolExport.exe app6b SFGPUCI----D---	
RuntimeSymbolExport.exe app6b SFGPUCI----E---	
RuntimeSymbolExport.exe app6b SFGPUCI----F---	
RuntimeSymbolExport.exe app6b SFGPUCI----G---	
RuntimeSymbolExport.exe app6b SFGPUCI----H---	
RuntimeSymbolExport.exe app6b SFGPUCI----I---	
RuntimeSymbolExport.exe app6b SFGPUCI----J---	
RuntimeSymbolExport.exe app6b SFGPUCI----K---	
RuntimeSymbolExport.exe app6b SFGPUCI----L---	
RuntimeSymbolExport.exe app6b SFGPUCI----M---	

		
REM # Position 11 and 12 (Echelon+Modifer)	
RuntimeSymbolExport.exe app6b SFGPUCI---AA---	
RuntimeSymbolExport.exe app6b SFGPUCI---AE---	
RuntimeSymbolExport.exe app6b SFGPUCI---BF---	
RuntimeSymbolExport.exe app6b SFGPUCI---CG---	
RuntimeSymbolExport.exe app6b SFGPUCI---DH---	
RuntimeSymbolExport.exe app6b SFGPUCI---EI---	
RuntimeSymbolExport.exe app6b SFGPUCI---FC---	
RuntimeSymbolExport.exe app6b SFGPUCI---GD---	
		
REM # Some Test ANNEX D (Each Battle Dimension)	
REM # Space	
RuntimeSymbolExport.exe app6b SFPPS----------	

REM # Air	
RuntimeSymbolExport.exe app6b SFAPMFF--------

REM FONT/ICON CENTER CHECKS (SHOULD BE CENTERED AND SAN-SERIF/ARIAL FONT):
RuntimeSymbolExport.exe app6b SFAPMFFI-------
RuntimeSymbolExport.exe app6b SFAPMFJ--------
	
REM # Ground Equipment	
RuntimeSymbolExport.exe app6b SFGPEVAL-------	
REM # Ground Units	
RuntimeSymbolExport.exe app6b SFGPUCIL-------	
REM # Ground Installation	
RuntimeSymbolExport.exe app6b SFGPIR----H----	
RuntimeSymbolExport.exe app6b SFGPIRSR--H----   
REM # Sea Surface	
RuntimeSymbolExport.exe app6b SFSPCL---------	
REM # Sub Surface	
RuntimeSymbolExport.exe app6b SFUPSN---------	

cd %~dp0\..\RuntimeSymbolExport\bin\Debug
copy *.png ..\..\images
del *.png

pause
