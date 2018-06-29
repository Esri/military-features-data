#!/usr/bin/python
# Simple CSV to HTML Table
# Adapted from:  https://gist.github.com/enigmaticape/4016913

import sys
import os
import csv
import string

### Params:
### 1 - input file (.csv)
### 2 - output file (.html)

if len(sys.argv) < 3 :
    print 'Usage: {Input File(.csv)} {Output File (.html)}'
    sys.exit(0)

inputFileName  = sys.argv[1]
outputFileName = sys.argv[2]

if not os.path.exists(inputFileName):
    sys.stderr.write(inputFileName + " not found \n" )
    sys.exit(0)
    
outputFile = None
try :
    outputFile = open(outputFileName, 'w')
except Exception as openOutputEx :
    print('Could not open file for writing: ' + outputFileName)
    sys.exit(0)    

# If Python 2 -> 'rb' or if 3 -> 'r'
if sys.version < '3' : 
    openFormat = 'rb'
else :
    openFormat = 'r'
 
outputFile.write('<HTML>\n<BODY>\n<TABLE border="1">\n')
 
with open(inputFileName, openFormat) as csvfile:
    table_string = ""
    reader       = csv.reader(csvfile)
    
    for row in reader:
        for col in range(5,8):
            row[col] = row[col].replace('![](','<img src="')
            row[col] = row[col].replace('.png)','.png">')
            

        table_string += "<tr>" + \
                        "<td>" + \
                        string.join(row, "</td><td>") + \
                        "</td>" + \
                        "</tr>\n"
    
    outputFile.write(table_string)
    
outputFile.write('</TABLE>\n</BODY>\n</HTML>\n')
