REM SET Below to Python Path on your machine

REM Command Format: python {PYTHONFILE}.py [Params] > [log file]

REM You may need to update this path to python if you get an error
SET PYTHON_PATH="C:\Python27"

%PYTHON_PATH%\python csv2html.py App6b-Test-Results.csv App6b-Test-Results.html

REM %PYTHON_PATH%\python ..\utilities\csv2html.py App6b-Test-Results-IssuesOnly.csv App6b-Test-Results-IssuesOnly.html

echo Done!
pause