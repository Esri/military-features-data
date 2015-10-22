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
# CreateTemplateGDB.py
#
# Description: Creates an empty file geodatabase from the contents of
# an input CSV file.  
#
# Adds a feature dataset and feature classes based on a supplied schema.  
#
# Adds fields to all those feature classes based on their supplied schemas.
#
# Requirements: ArcGIS Desktop
# ----------------------------------------------------------------------------------

import arcpy, os, sys, traceback
import AddFieldsFromSchema
import csv as csv
import time

from arcpy import env
from os import path

def geometryTypeLookup(jmsmlGeometryType):
    
    # Converts a JMSML symbol type into an Esri GIS geometry type
    
    switcher = {
                "Point": "POINT",
                "Line": "POLYLINE",
                "Area": "POLYGON",
                }
    return switcher.get(jmsmlGeometryType, "NONE")

def createVersionsTable(schemasFolder, versionFile, geodatabase):
    
    # Creates a version table using the contents of the specified CSV file
    # and today's date.  Inserts the table into the specified geodatabase
    
    try:
        arcpy.AddMessage("Starting: CreateVersionsTable")
        
        if arcpy.Exists(versionFile):
            # Set the current date as the creation date
            
            outFile = os.path.join(schemasFolder, "temp.csv")
            csvOut = open(outFile, "w")
            writer = csv.writer(csvOut, delimiter=',', lineterminator='\n')
            
            with open(versionFile, "r") as csvIn:
                reader = csv.reader(csvIn, delimiter=',', lineterminator='\n')
                
                for line in reader:
                    if line[0] == "automated_creation_date" or line[0] == "last_modification_date":
                        outString = time.strftime("%x")
                    else:
                        outString = line[1]
                        
                    writer.writerow([line[0], outString])
                    
                csvOut.close()         
                    
            # Now delete the original versions file and replace it with the new
            
            os.remove(versionFile)
            os.rename(outFile, versionFile)
            
            # Now create the Versions table in the gdb
            
            tableName = "Versions"
            
            arcpy.CreateTable_management(geodatabase, tableName)
            
            table = os.path.join(geodatabase, tableName)
            arcpy.AddField_management(table, "Item", "TEXT", field_length=50)
            arcpy.AddField_management(table, "Version", "TEXT", field_length=50)
            
            arcpy.CopyRows_management(versionFile, table)
    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
        
    finally:
        arcpy.AddMessage("Success! - Stopping: CreateVersionsTable")
            
def createTemplateGDB(schemasFolder, destinationFolder, version):
    
    # Creates the specified file geodatabase of the specified version
    
    try:
        arcpy.AddMessage("Starting: CreateEmptyGDB")
        
        # Create a new file gdb in supplied folder, using supplied schema specification  
               
        # Schema details are read from CSV files created by JMSML
        
        # Open the schemas CSV file which contains the basic structure of the gdb
        # we are going to create
        
        schemaFile = os.path.join(schemasFolder, "Schemas.csv")
        
        with open(schemaFile, 'r') as csvFile:
            reader = csv.reader(csvFile, dialect='excel')
            
            # Skip the headers
            header = next(reader)
            
            # Read the second line, the line of data should be a geodatabase schema line
            
            line = next(reader)
            
            if line[0] == "SchemaContainer":
                arcpy.AddMessage("Creating file geodatabase " + line[1] + "...")
                
                gdbName = line[1] + ".gdb"
                gdbPath = os.path.join(destinationFolder, gdbName)
                
                arcpy.env.workspace = destinationFolder
  
                if(arcpy.Exists(gdbName)):
                    arcpy.Delete_management(gdbName)
           
                arcpy.CreateFileGDB_management(destinationFolder, gdbName, version)
                
                # Create a Versions table from the contents of a CSV file
                
                createVersionsTable(schemasFolder, os.path.join(schemasFolder, "Versions.csv"), gdbPath)
                        
                # Read the next line of data.  It should be a dataset and its metadata
                
                line = next(reader)
                
                if line[0] == "SchemaSet":
                    arcpy.AddMessage("Creating feature dataset " + line[1] + "...")
                    
                    # Fetch the dataset's spatial reference
                    
                    dataSet = line[1]
                    sr = arcpy.SpatialReference(int(line[17]))
                    
                    arcpy.CreateFeatureDataset_management(gdbPath, dataSet, spatial_reference=sr)
                    
                    # Read the following lines of the CSV and create a feature class for each
                    
                    for line in reader:
                        if line[0] == "Schema":
                            fcName = line[1]
                            geoType = line[2]
                            alias = line[3]
                            srID = line[17]
                            
                            arcpy.AddMessage("Creating feature class " + fcName + "...")
                            
                            geometryType = geometryTypeLookup(geoType)
                            
                            if geometryType != "NONE":
                                out_path = os.path.join(gdbPath, dataSet)
                                
                                if srID == "":
                                    arcpy.CreateFeatureclass_management(out_path, fcName, geometryType, has_m="DISABLED", has_z="ENABLED")
                                else:
                                    sr = arcpy.SpatialReference(int(srID))
                                    arcpy.CreateFeatureclass_management(out_path, fcName, geometryType, spatial_reference=sr, has_m="DISABLED", has_z="ENABLED")
                                
                                featureClass = os.path.join(out_path, fcName)
                                arcpy.AlterAliasName(featureClass, alias)
                                
                                AddFieldsFromSchema.addFieldsFromSchema(schemasFolder, featureClass, fcName)
                        
        
    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
        
    finally:
        arcpy.AddMessage("Success! - Stopping: CreateEmptyGDB")
    
if __name__ == '__main__':
    schemasFolder = arcpy.GetParameterAsText(0)
    destinationFolder = arcpy.GetParameterAsText(1)
    version = arcpy.GetParameterAsText(2)
    
    createTemplateGDB(schemasFolder, destinationFolder, version)
    