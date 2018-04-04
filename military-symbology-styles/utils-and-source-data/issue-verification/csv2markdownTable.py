#-------------------------------------------------------------------------------
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#   http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#-------------------------------------------------------------------------------
# Name: csv2markdownTable.py
# Usage: python.exe {Input File(.csv)} {Output File (.md)}
# Description: Converts a .csv to a Markdown Table, for .md table format see:
#    https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet#tables
#    Note: not all Markdown viewers support this format
#-------------------------------------------------------------------------------
# Important: the CSV must have a 1st/header row that has the desired 
#            column names to be used
# Method:
#   For every row: 
#     1. Add "|" to start & end of line "|"
#     2. Replace "," with "|"
#   For 1st/header row
#     1. Add header row
#     2. Add a 2nd row with the "|---|---|..."
#-------------------------------------------------------------------------------

import csv
import os
import sys

### Params:
### 1 - input file (.csv)
### 2 - output file (.md)

if len(sys.argv) < 3 :
    print 'Usage: {Input File(.csv)} {Output File (.md)}'
    sys.exit(0)

inputFileName  = sys.argv[1]
outputFileName = sys.argv[2]

if not os.path.exists(inputFileName) :
	print 'Input CSV does not exist: ' + inputFileName 
	sys.exit(0)

print 'Exporting Input CSV: ' + inputFileName + ' to Output MD: ' + outputFileName

inputFile = None
try :
	if sys.version < '3' : 
		inputFile  = open(inputFileName,  'rb')
	else :
		inputFile  = open(inputFileName,  'r')
except Exception as openInputEx :
	print('Could not open file for reading: ' + inputFileName)
	sys.exit(0)

outputFile = None
try :
	outputFile = open(outputFileName, 'w')
except Exception as openOutputEx :
	print('Could not open file for writing: ' + outputFileName)
	sys.exit(0)

reader = csv.reader(inputFile)

mdSeparator = '|'
justifyEntry = '---' # =Left - or- :---: (center) -or- ---: (Right)
rowCount = 0
for row in reader:
	colCount = 0
	line = mdSeparator
	for col in row:
		line += col
		line += mdSeparator # add '|'
		colCount += 1

	line += '\n'
	outputFile.write(line)

	if rowCount == 0:
		# If this is the 1st/header row, add a 2nd row with "|---|---|..."
		line = mdSeparator
		for headerCount in range(0, colCount):
			line += justifyEntry
			line += mdSeparator
		line += '\n'
		outputFile.write(line)
			
	rowCount += 1

print('Done! Lines Processed: ' + str(rowCount))

inputFile.close()
outputFile.close()
