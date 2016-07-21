# ----------------------------------------------------------------------------------
# Copyright 2016 Esri
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
# GenerateDomainCSVs.py
# Description: Reads the All_ID_Mapping_Original CSV file and
# selects rows with Legacy Key values of 10 characters, generates
# CSV file for the generation of 2525C or 2525Bc2 geodatabase domains.
# Requirements: ArcGIS Desktop
# ----------------------------------------------------------------------------------

# Import arcpy module
import arcpy, os, traceback
import csv as csv


# variables:
filter_codes_table_Copy = "in_memory\\filter_codes_table_Copy"
All_ID_Mapping_Original_Copy = "in_memory\\All_ID_Mapping_Original_Copy"
All_ID_Mapping_Sel_10_char = "in_memory\\All_ID_Mapping_Sel_10_char"
CharlieName = ""

def load_table_in_memory():
    # Copy Rows
    arcpy.env.workspace = "in_memory"
    arcpy.CopyRows_management(All_ID_Mapping_Original_csv, All_ID_Mapping_Original_Copy, "")

    # Table Select
    if choose_standard == "B2":
        arcpy.TableSelect_analysis(All_ID_Mapping_Original_Copy, All_ID_Mapping_Sel_10_char, "(CHAR_LENGTH ( \"LegacyKey\" ) = 10) AND (\"Standard\" <> 'C') OR \"Standard\" IS Null")
    else:
        arcpy.TableSelect_analysis(All_ID_Mapping_Original_Copy, All_ID_Mapping_Sel_10_char, "(CHAR_LENGTH ( \"LegacyKey\" ) = 10) AND (\"Standard\" <> 'B2') OR \"Standard\" IS Null")

    # Load filter_codes_table to in_memory workspace
    arcpy.CopyRows_management(filter_codes_table, filter_codes_table_Copy, "")

def loop_filter_codes():
    # Iterate filter_codes_table_Copy to get file names, CharlieName, Filter1, Filter2, FilterGeom values
    arcpy.AddMessage("Iterating filter code table.")
    fields = ['CharlieName', 'Filter1', 'Filter2', 'FilterGeom']
    with arcpy.da.SearchCursor(filter_codes_table_Copy, fields) as cursor:
        for row in cursor:
            if row[3] in ["Point", "Line","Area"]:
                FilterGeom = row[3]
                the_query = "LegacyKey LIKE " + "\'" + str(row[1])+ "%\'" + " AND " + "GeometryType = " + "\'" + str(FilterGeom) + "\'"
                
            elif row[2] is not None:
                Filter2 = row[2]
                the_query = "LegacyKey LIKE " + "\'" + str(row[1])+ "%\'" + " OR " + "LegacyKey LIKE " + "\'" + str(row[2])+ "%\'"
                
            else:
                Filter1 = row[1]
                the_query = "LegacyKey LIKE " + "\'" + str(row[1])+ "%\'"

            CharlieName = row[0]    
            write_matching_rows_to_csv(CharlieName, the_query)
                

def write_matching_rows_to_csv(CharlieName, the_query):
    # Write out the CSV domain table for a given row in the filter_codes_table
    checkDupList = []
    arcpy.AddMessage("Writing: " + str(CharlieName))
    outName = "Coded_Domain_" + str(CharlieName) + ".csv"
    outFile = os.path.join(out_domain_location, outName)
    csvOut = open(outFile, "w")
    writer = csv.writer(csvOut, delimiter=',', lineterminator='\n')
    writer.writerow(["Name", "Value"])
    mainFields = ['Name', 'LegacyKey', 'GeometryType', 'Standard']
    mainCursor = arcpy.da.SearchCursor(All_ID_Mapping_Sel_10_char, mainFields, the_query)
    for row in mainCursor:
        if row[1] not in checkDupList:
            writer.writerow([row[0], row[1]])
            checkDupList.append(row[1])
        else:
            arcpy.AddWarning("Duplicate value: " + str(row[1]) + " in All_ID_Mapping table!")
    csvOut.close()

if __name__ == '__main__':
    filter_codes_table = '../Tooldata/Filter_Codes.csv'
    All_ID_Mapping_Original_csv = arcpy.GetParameterAsText(0)
    out_domain_location = arcpy.GetParameterAsText(1)
    choose_standard = arcpy.GetParameterAsText(2)  #Valid values are B2 or C

    if All_ID_Mapping_Original_csv == '':
        All_ID_Mapping_Original_csv = '../Tooldata/All_ID_Mapping_Original.csv'

    if out_domain_location == '':
        out_domain_location = '../Output/'

    if choose_standard == '':
        choose_standard = 'C'


    try:
        load_table_in_memory()
        
    except Exception as err:
            arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
    try:
        loop_filter_codes()
        arcpy.AddMessage("Finished writing domain CSV tables!")
        
    except Exception as err:
            arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())

    # Clean up in memory tables
del All_ID_Mapping_Sel_10_char
del All_ID_Mapping_Original_Copy
del filter_codes_table_Copy
    
    
