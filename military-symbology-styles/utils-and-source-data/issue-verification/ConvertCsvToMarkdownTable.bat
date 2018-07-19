REM SET Below to Python Path on your machine

REM Command Format: python {PYTHONFILE}.py [Params] > [log file]

REM You may need to update this path to python if you get an error
SET PYTHON_PATH="C:\Python27"

%PYTHON_PATH%\python csv2markdownTable.py App6b-Test-Results.csv App6b-Test-Results.md

REM %PYTHON_PATH%\python ..\utilities\csv2markdownTable-FilterIssues.py App6b-Test-Results.csv App6b-Test-Results-Issues-Only.md

echo Done!
pause