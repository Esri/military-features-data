@ECHO OFF

:: License Apache V2 

:: This script replaces text in a set of SVG files 

:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
:: IMPORTANT/TODO: you must set/correct paths below
:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

:: (1) Source SVG Root Folder 
SET svg_source_folder=C:\{TODO_EDIT_PATH_TO_SVGs}
:: Ex: 
:: SET svg_source_folder=C:\Github\joint-military-symbology-xml\svg\MIL_STD_2525D_Symbols

:: Note: there must be a "Frames" subfolder under the path above
SET svg_frames_source_folder="%svg_source_folder%\Frames"

:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

:: it is the default, but just in case, this enables the needed feature "goto :eof" ("return")
setlocal ENABLEEXTENSIONS

powershell -Command "Write-Host "PowerShell Installed""

:: Check source folder exists
if exist %svg_frames_source_folder% goto prereqs_exists_ok

echo Required Source Folder does not exist: %svg_frames_source_folder%
pause
goto :EOF

:prereqs_exists_ok

:: Note: Use "/r" option if you want to convert a folder and all subfolders (recursion)
:: do ReplaceTextInFiles for each svg found with these matching patterns:

for /r %svg_frames_source_folder% %%i in (*_1.svg) do call :ReplaceTextInFiles %%i

for /r %svg_frames_source_folder% %%i in (*_1c.svg) do call :ReplaceTextInFiles %%i

:: Done
echo Successfully Completed!

pause

goto :EOF

:: ----------------------------------------------------------------------------
:: ReplaceTextInFiles - Replaces text in a file
:: 
:: %1 - Source File (full path)
:: ----------------------------------------------------------------------------

:ReplaceTextInFiles

setlocal ENABLEEXTENSIONS

set source_file="%1"

if "" == %source_file% goto :EOF

:: Check file exists
if exist %source_file% goto exists_ok

echo "Required File does not exist: %source_file%"
goto :EOF

:exists_ok

echo Checking text in: %source_file%

:: Simpler Form if we only had to do 1 replace:
:: powershell -Command "(gc %source_file%) -replace 'stroke-dasharray=\"X,Y\"', 'stroke-dasharray=\"75,45\"' | Out-File %source_file% -encoding ASCII"

:: Our *slightly* more complicated version to replace multiple strings (needs run from standalone script):
powershell -ExecutionPolicy ByPass -file PowerShellReplaceMultiple.ps1 %source_file%

endlocal & goto :EOF
