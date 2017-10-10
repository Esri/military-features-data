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
import UpdateDomains
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

def createVersionsTable(schemasFolder, geodatabase):
    
    # Creates a version table using the contents of the specified CSV file
    # and today's date.  Inserts the table into the specified geodatabase
    
    try:
        arcpy.AddMessage("Starting: CreateVersionsTable")
        
        currentPath = os.path.dirname(__file__) 
        versionFile = os.path.normpath(os.path.join(currentPath, "../../../style-utilities/merge-stylx-utilities/versions.csv"))
        
        if arcpy.Exists(versionFile):
            # Set the current date as the creation date
            
            outFile = os.path.join(schemasFolder, "temp.csv")
            csvOut = open(outFile, "w")
            
            writer = csv.writer(csvOut, delimiter=',', lineterminator='\n')
            
            # Write headers
            
            writer.writerow(["Item", "Version"])
            
            with open(versionFile, "r") as csvIn:
                reader = csv.reader(csvIn, delimiter=',', lineterminator='\n')
                
                for line in reader:    
                    writer.writerow([line[0], line[1]])
                
            outString = time.strftime("%x")    
            writer.writerow(["automated_creation_date", outString])
            writer.writerow(["last_modification_date", outString])
                
            csvOut.close()         
            
            # Now create the Versions table in the gdb
            
            tableName = "Versions"
            
            arcpy.CreateTable_management(geodatabase, tableName)
            
            table = os.path.join(geodatabase, tableName)
            arcpy.AddField_management(table, "Item", "TEXT", field_length=50)
            arcpy.AddField_management(table, "Version", "TEXT", field_length=50)
            
            arcpy.CopyRows_management(outFile, table)
            
            os.remove(outFile)
        else:
            arcpy.AddError(versionFile + " does not exist.")
            
    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
        
    else:
        arcpy.AddMessage("Success! - Completed: CreateVersionsTable")
        
    finally:
        arcpy.AddMessage("Exiting: CreateVersionsTable")
            
def createTemplateGDB(schemasFolder, destinationFolder, version):
    
    # Creates the specified file geodatabase of the specified version
    
    try:
        arcpy.AddMessage("Starting: CreateTemplateGDB")
        
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
                
                createVersionsTable(schemasFolder, gdbPath)
                
                # Create all the domains
                
                domainPath = os.path.normpath(os.path.join(schemasFolder, "../name_domains_values"))
                UpdateDomains.updateDomains(domainPath, gdbPath)
                        
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
        
    else:
        arcpy.AddMessage("Success! - Completed: CreateTemplateGDB")
        
    finally:
        arcpy.AddMessage("Exiting: CreateTemplateGDB")
    
if __name__ == '__main__':
    schemasFolder = arcpy.GetParameterAsText(0)
    destinationFolder = arcpy.GetParameterAsText(1)
    version = arcpy.GetParameterAsText(2)
    
    createTemplateGDB(schemasFolder, destinationFolder, version)
    