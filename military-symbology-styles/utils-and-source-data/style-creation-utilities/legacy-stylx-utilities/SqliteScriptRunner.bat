:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
:: General purpose SQLite SQL Script Runner
:: IMPORTANT/TODO: you must set/correct the paths below: 
:: The expected workflow is to edit this file to set the SQLITE_SCRIPT variable
:: and save this files as "SqliteScriptRunner-local.bat"
:: ("-local" is an extention .gitignore is set to ignore in this repo)
:: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

:: INPUTS/SETTINGS:
:: (1) Path to Sqlite executable/engine
:: If you are using a tool other than sqlite3.exe, you may need to edit that also
SET SQLITE_PATH_AND_EXE=C:{TODO SET PATH}sqlite3.exe
:: EXAMPLE: SET SQLITE_PATH_AND_EXE=C:\Installs\Sqlite\sqlite3.exe

:: (2) SQLITE DATABASE to run script against
SET SQLITE_DATABASE=C:{TODO SET PATH}\mil2525c_b2.stylx

:: (3) SQL Script to Run against database
SET SQLITE_SCRIPT=C:{TODO SET PATH}\Sqlite-local.sql

:: it is the default, but just in case, this enables the needed feature "goto :eof" ("exit/return")
setlocal ENABLEEXTENSIONS

:: Check the Sqlite exe exists
if exist "%SQLITE_PATH_AND_EXE%" goto prereqs_sqlite_exists_ok

echo "ERROR: Required Sqlite exe/engine does not exist: %SQLITE_PATH_AND_EXE%"
goto :pauseit

:prereqs_sqlite_exists_ok

:: Check that the sql file/db exists
:: just check that an expected folder exists
if exist "%SQLITE_DATABASE%" goto prereqs_database_exists_ok

echo "ERROR: could not find SQL database: %SQLITE_DATABASE%"
goto :pauseit

:prereqs_database_exists_ok

:: Check that the script exists
:: just check that an expected folder exists
if exist "%SQLITE_SCRIPT%" goto prereqs_script_exists_ok

echo "ERROR: could not find SQL script: %SQLITE_SCRIPT%"
goto :pauseit

:prereqs_script_exists_ok

:: Prerequisites have been verified, now execute the SQLite Command: 
:: ex: sqlite3.exe myfile.db < script.sql

"%SQLITE_PATH_AND_EXE%" "%SQLITE_DATABASE%" < "%SQLITE_SCRIPT%"

echo ******* IMPORTANT: Now verify the output and check for any errors *******

:pauseit
Pause
