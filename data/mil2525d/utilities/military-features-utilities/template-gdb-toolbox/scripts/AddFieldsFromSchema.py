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

import Utility

from arcpy import env
from os import path

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
    
def addSubtypes(schemasFolder, schema, featureClass, fieldName):
    
    # Sets the field used to determine the subtype of each feature
    # Populates the subtypes for the specified feature class
    
    try:
        arcpy.AddMessage("Starting: AddSubtypes")
        
        # Initialize a dict
        
        subTypes = {}
        
        # Set the subtype field
        
        arcpy.SetSubtypeField_management(featureClass, fieldName)
        
        # Populate the subtypes from the provided subtype schema file
        
        subtypesSchemaFile = os.path.join(schemasFolder, "Subtypes_" + schema + ".csv")
        
        if os.path.exists(subtypesSchemaFile):
            with open(subtypesSchemaFile, 'r') as csvFile:
                reader = csv.reader(csvFile, dialect='excel')
        
                # Skip the headers
                header = next(reader)
                
                defaultSubtype = 0
                isFirst = True
                
                # Read the lines and add them as subtypes
                for line in reader:
                    arcpy.AddSubtype_management(featureClass, int(line[0]), line[1])
                    
                    subTypes[line[0]] = line[1]
                    
                    if isFirst == True:
                        # Set the default subtype value
                        arcpy.SetDefaultSubtype_management(featureClass, int(line[0]))
                        isFirst = False
                
    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
    
    else:
        arcpy.AddMessage("Success! - Completed: AddSubtypes")
            
    finally:
        arcpy.AddMessage("Exiting: AddSubtypes")
        
        return subTypes
    
        
def addAField(featureClass, fieldType, line):   
    if fieldType == "TEXT":
        length = int(line[2])
        arcpy.AddField_management(featureClass, \
                                  field_name=line[0], \
                                  field_type=fieldType, \
                                  field_length=length, \
                                  field_alias=line[3], \
                                  field_is_nullable=line[4])
    else:
        arcpy.AddField_management(featureClass, \
                                  field_name=line[0], \
                                  field_type=fieldType, \
                                  field_alias=line[3], \
                                  field_is_nullable=line[4])
        
        
def addFieldsFromSchema(schemasFolder, featureClass, schema):
    
    # Adds the fields defined in a field schema CSV file to the specified feature class
    
    try:
        arcpy.AddMessage("Starting: AddFieldsFromSchema") 
            
        # Make sure the feature class exists
        
        if arcpy.Exists(featureClass):
            
            subTypes = {}
            
            # Make sure the specfied field schema CSV file exists
            
            fieldSchemaFile = os.path.join(schemasFolder, "Fields_" + schema + ".csv")
            if os.path.exists(fieldSchemaFile):
                with open(fieldSchemaFile, 'r') as csvFile:
                    reader = csv.reader(csvFile, dialect='excel')
            
                    # Skip the headers
                    
                    header = next(reader)
                    
                    # Read all the rows and add fields accordingly
                    
                    fieldName = None
            
                    for line in reader:
                        fieldType = Utility.fieldTypeLookup(line[1])
                        
                        # Add a line as a new field if this is the first occurence of that "field"
                        
                        if line[0] != fieldName:
                            addAField(featureClass, fieldType, line)
                            fieldName = line[0]
                            
                        # Set the domain for the field, including subtype code if necessary
                        
                        if bool(line[5]):
                            if bool(line[8]):
                                arcpy.AssignDomainToField_management(featureClass, line[0], line[5], line[8] + ": " + subTypes[line[8]])
                            else:
                                arcpy.AssignDomainToField_management(featureClass, line[0], line[5])
                            
                        # Set the default value for the field
                        
                        if bool(line[6]):
                            defaultValue = line[6]
                            castedDefault = castValue(fieldType, defaultValue)
                            
                            if bool(line[8]):
                                arcpy.AssignDefaultToField_management(featureClass, line[0], castedDefault, line[8] + ": " + subTypes[line[8]])
                            else:
                                arcpy.AssignDefaultToField_management(featureClass, line[0], castedDefault)
                                
                        # Check to see if this field sets a subtype and if it does, go create the subtypes
                        
                        if line[7] == "True":
                            subTypes = addSubtypes(schemasFolder, schema, featureClass, line[0])
                                     
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
        