# ----------------------------------------------------------------------------------
# Copyright 2015 Esri
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
# ----------------------------------------------------------------------------------


#import CSV Module
import csv
#import arcpy
import arcpy

#filtergeom = 'POINT'
#filterappendix = 'A'
inputFileName = arcpy.GetParameterAsText(0)
outputfolder = arcpy.GetParameterAsText(1)

outputFileDict = {"AppendixA_Points.csv": ["A", "POINT"], \
                  "AppendixB_Points.csv": ["B", "POINT"], \
                  "AppendixB_lines.csv": ["B", "LINE"], \
                  "AppendixB_areas.csv": ["B", "AREA"], \
                  "AppendixC_Points.csv": ["C", "POINT"], \
                  "AppendixC_lines.csv": ["C", "LINE"], \
                  "AppendixC_areas.csv": ["C", "AREA"], \
                  "AppendixD_Points.csv": ["D", "POINT"], \
                  "AppendixE_Points.csv": ["E", "POINT"], \
                  "AppendixG_Points.csv": ["G", "POINT"]}

for key in outputFileDict:
    dictionaryEntry = outputFileDict[key]
    filterappendix = dictionaryEntry[0]
    filtergeom = dictionaryEntry[1]
    print(key, outputFileDict[key], filterappendix, filtergeom)

    if outputfolder == "" or outputfolder is None:
        outputfolder = "C:\\Users\\dani8200\\Documents\\MilitarySymbology\\Test Datasets\\PythonforTestDatasets\\Cresults"
    if inputFileName =="" or outputfolder is None:
        inputFileName = outputfolder + "Mil2525C.csv"

    inputfile = open(inputFileName, "r")
    reader = csv.reader(inputfile)

    filterOutputFileName = key
    outputFileName = outputfolder + "\\" + filterOutputFileName
    outputfile = open(outputFileName, 'w')
    writer = csv.writer(outputfile)

    print("Opening " + inputFileName)
    arcpy.AddMessage("Opening " + inputFileName)
    print("Writing " + outputFileName)
    arcpy.AddMessage("Writing " + outputFileName)

    inputRowCount = 0
    outputRowCount = 0

    for row in reader:
        columns = row
        inputRowCount += 1

        outputrow = None
        if len(columns) > 8:
            OID = columns[0]
            Appendix = columns[1]
            FullSIDC = columns[3]
            SIDCbyParts = columns[4]
            HierarchyCode = columns[5]
            Name = columns[6]
            Geometry = columns[7]
            Notes = columns [8]
            if ((Geometry == filtergeom) and (Appendix == filterappendix)) or (inputRowCount == 1):
                outputOID = str(outputRowCount)
                if outputRowCount == 0:
                    outputOID = "OID"
                outputrow = [outputOID, Appendix, FullSIDC, SIDCbyParts, HierarchyCode, Name, Geometry, Notes]
                print(outputrow)
                writer.writerow(outputrow)
                outputRowCount +=1

    arcpy.AddMessage("Done Processing " + outputFileName + " Lines Read = " + str(inputRowCount) + " Lines Found = " + str(outputRowCount))

