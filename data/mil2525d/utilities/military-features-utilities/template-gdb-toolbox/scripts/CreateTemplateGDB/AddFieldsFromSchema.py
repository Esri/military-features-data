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
# AddFieldsFromSchema.py
# Description: Reads the field descriptions from a field schema CSV file and
# adds those fields to the specified feature class.
# Requirements: ArcGIS Desktop
# ----------------------------------------------------------------------------------

import arcpy, os, sys, traceback
import csv as csv

from arcpy import env
from os import path

def fieldTypeLookup(jmsmlFieldType):
    # Converts a JMSML field type into an Esri field type
    switcher = {
                "Date": "DATE",
                "Double": "DOUBLE",
                "Integer": "LONG",
                "SmallInteger": "SHORT",
                "String": "TEXT",
                }
    
    return switcher.get(jmsmlFieldType, "NONE")

def castValue(valueType, originalValue):
    # Cast input string value to a specified type
    
    if valueType == "DOUBLE":
        castedValue = float(originalValue)
    elif valueType == "LONG":
        castedValue = int(originalValue)
    elif valueType == "SHORT":
        castedValue = int(originalValue)
    else:
        castedValue = originalValue
        
    return castedValue
    
def addFieldsFromSchema(schemasFolder, featureClass, schema):
    
    # Adds the fields defined in a field schema CSV file to the specified feature class
    
    try:
        arcpy.AddMessage("Starting: AddFieldsFromSchema") 
            
        # Make sure the feature class exists
        
        if arcpy.Exists(featureClass):
            
            # Make sure the specfied field schema CSV file exists
            
            fieldSchemaFile = os.path.join(schemasFolder, "Fields_" + schema + ".csv")
            if os.path.exists(fieldSchemaFile):
                with open(fieldSchemaFile, 'r') as csvFile:
                    reader = csv.reader(csvFile, dialect='excel')
            
                    # Skip the headers
                    header = next(reader)
                    
                    # Read all the rows and add fields accordingly
                    for line in reader:
                        
                        fieldType = fieldTypeLookup(line[1])
                        
                        if fieldType == "TEXT":
                            length = int(line[2])
                            
                            
                            arcpy.AddField_management(featureClass, \
                                                      field_name=line[0], \
                                                      field_type=fieldType, \
                                                      field_length=length, \
                                                      field_alias=line[3], \
                                                      field_is_nullable=line[4], \
                                                      field_domain=line[5])
                        else:
                            arcpy.AddField_management(featureClass, \
                                                      field_name=line[0], \
                                                      field_type=fieldType, \
                                                      field_alias=line[3], \
                                                      field_is_nullable=line[4], \
                                                      field_domain=line[5])
                        
                        # Set the default value for the field
                        
                        if bool(line[6]):
                            defaultValue = line[6]
                            castedDefault = castValue(fieldType, defaultValue)
                            arcpy.AssignDefaultToField_management(featureClass, line[0], castedDefault)
                                     
    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
    
    else:
        arcpy.AddMessage("Success! - Completed: AddFieldsFromSchema")
            
    finally:
        arcpy.AddMessage("Exiting: AddFieldsFromSchema")

if __name__ == '__main__':
    schemasFolder = arcpy.GetParameterAsText(0)
    featureClass = arcpy.GetParameterAsText(1)
    schema = arcpy.GetParameterAsText(2)
    
    addFieldsFromSchema(schemasFolder, featureClass, schema)
        