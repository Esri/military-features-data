:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
:: IMPORTANT/TODO: you must set/correct paths below: "TODO_"
:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

:: INPUTS/SETTINGS:
:: (1) Joint Military Symbology Cloned Repo location: 
:: Cloned from: https://github.com/Esri/joint-military-symbology-xml to local machine
SET LOCAL_JMSXML_REPO_CLONE=C:\{TODO_EDIT_THIS_PATH}\joint-military-symbology-xml
:: EXAMPLE: SET LOCAL_JMSXML_REPO_CLONE=C:\Github\joint-military-symbology-xml

:: (2) Path to Sqlite executable/engine
:: If you are using a tool other than sqlite3.exe, you may need to edit that also
SET SQLITE_PATH_AND_EXE=C:\{TODO_EDIT_THIS_PATH}\sqlite3.exe
:: EXAMPLE: SET SQLITE_PATH_AND_EXE=C:\Installs\Sqlite\sqlite3.exe

:: (3) Military Features Data Root Folder (military-features-data) 
:: No need to set this, but do note it assumes it is being run from: 
:: .\military-features-data\data\mil2525d\utilities\style-utilities\merge-stylx-utilities
:: Just make it relative to this folder (this is easier than using this relative path everywhere)
SET LOCAL_MIL_FEATURES_REPO_2525D=..\..\..

:: (4) Name of the stylx file that has only the Point/SVG symbols
:: IMPORTANT: the script assumes this file will be in the same folder: 
:: ...style-utilities\merge-stylx-utilities
SET POINTS_ONLY_STYLX=mil2525d-points-only.stylx

:: it is the default, but just in case, this enables the needed feature "goto :eof" ("exit/return")
setlocal ENABLEEXTENSIONS

:: Check the JMSXML_REPO folder exists
if exist "%LOCAL_JMSXML_REPO_CLONE%" goto prereqs_jmsxml_exists_ok

echo "ERROR: Required local joint-military-symbology-xml repo does not exist: %LOCAL_JMSXML_REPO_CLONE%"
goto :pauseit

:prereqs_jmsxml_exists_ok

:: Check the Sqlite exe exists
if exist "%SQLITE_PATH_AND_EXE%" goto prereqs_sqlite_exists_ok

echo "ERROR: Required Sqlite exe/engine does not exist: %SQLITE_PATH_AND_EXE%"
goto :pauseit

:prereqs_sqlite_exists_ok

:: Check that we are in the expected military-features-data folder
:: just check that an expected folder exists
if exist "%LOCAL_MIL_FEATURES_REPO_2525D%\core_data" goto prereqs_milfeatures_exists_ok

echo "ERROR: This .bat does not appear to be in the expected folder: %LOCAL_MIL_FEATURES_REPO_2525D%"
goto :pauseit

:prereqs_milfeatures_exists_ok

if exist "%POINTS_ONLY_STYLX%" goto prereqs_points_only_stylx_exists_ok
echo "ERROR: Could not find points only stylx in current folder: %POINTS_ONLY_STYLX%"
goto :pauseit

:prereqs_points_only_stylx_exists_ok

copy "%LOCAL_MIL_FEATURES_REPO_2525D%\core_data\stylxfiles\mil2525d-lines-areas-labels-base-template.stylx" mil2525d.stylx

copy %POINTS_ONLY_STYLX% mil2525d-points-to-add-keys-labels.stylx

"%SQLITE_PATH_AND_EXE%" < "%LOCAL_MIL_FEATURES_REPO_2525D%\utilities\style-utilities\merge-stylx-utilities\SqliteMergeStylx.sql"

echo ******* IMPORTANT: Now verify the output and check for any errors *******

:pauseit
Pause
