REM Change back to current directory
pushd %~dp0 

SET PRO_DIR=C:\Program Files\ArcGIS\Pro
IF NOT EXIST "%PRO_DIR%" SET PRO_DIR=%LOCALAPPDATA%\Programs\ArcGIS\Pro

IF EXIST "%PRO_DIR%" goto pro_exists_ok

echo Can't find Pro install at: %PRO_DIR% - can't continue
pause
goto :EOF

:pro_exists_ok

SET DictionaryRenderer=mil2525c_b2
IF NOT EXIST %DictionaryRenderer% goto folder_error

REM Copy to Pro Install
mkdir "%PRO_DIR%\Resources\Dictionaries\%DictionaryRenderer%"
xcopy /v /y /s %DictionaryRenderer% "%PRO_DIR%\Resources\Dictionaries\%DictionaryRenderer%"

echo off
echo ********************************
echo * ArcGIS Pro Dictionary Added: %DictionaryRenderer%
echo ********************************
pause
goto :EOF

:folder_error

echo off
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
echo ! FAILED:
echo ! Could not find Folder: %DictionaryRenderer%
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
pause
