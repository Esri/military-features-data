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
# UpdateDomains.py
#
# Description: Updates the domains in a specified military features geodatabase,
# using the domain CSV files found in the specified folder.
#
# Requirements: ArcGIS Desktop
# ----------------------------------------------------------------------------------
import arcpy, os, sys, traceback
import csv
import glob

import Utility

from arcpy import env
from os import path

def findNameAndDescription(fname, replaceType):
    
    # Find and return the domain's name and description from its filename
    
    theFile = os.path.basename(fname)
    domainFileName, fileExt = os.path.splitext(theFile)
    domainName = domainFileName.replace(replaceType, " ").strip()
    domainDescription = domainName.replace('_',' ')
            
    return (domainName, domainDescription)

    
def updateDomains(domainsFolder, targetGDB):
    try:
        arcpy.AddMessage("Starting: UpdateDomains")
        
        # Update all coded (list) domains
        
        path = os.path.normpath(os.path.join(domainsFolder, "Coded_Domain_*.csv"))
        
        for fname in glob.glob(path):
            
            # Find the domain's name and description and then add it to the gdb
            
            domainName, domainDescription = findNameAndDescription(fname, "Coded_Domain_")
            
            arcpy.AddMessage("Updating domain " + domainName + "...")
            arcpy.TableToDomain_management(fname, "Value", "Name", targetGDB, domainName, domainDescription, "REPLACE")
            
        # Update all range domains
        
        path = os.path.normpath(os.path.join(domainsFolder, "Range_Domain_*.csv"))
        
        for fname in glob.glob(path):
            
            # Find the domain's name and description and then add it to the gdb
            
            domainName, domainDescription = findNameAndDescription(fname, "Range_Domain_")
                      
            arcpy.AddMessage("Updating domain " + domainName + "...")
            
            # Set the range domain's min and max values
            
            with open(fname, 'r') as csvFile:
                reader = csv.reader(csvFile, dialect='excel')
        
                # Skip the header, use the second line
                header = next(reader)
                line = next(reader)
                
                arcpy.CreateDomain_management(targetGDB, domainName, domainDescription, Utility.fieldTypeLookup(line[0]), "RANGE")
                arcpy.SetValueForRangeDomain_management(targetGDB, domainName, int(line[1]), int(line[2]))


    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
    
    else:
        arcpy.AddMessage("Success! - Completed: UpdateDomains")
        
    finally:
        arcpy.AddMessage("Exiting: UpdateDomains")

if __name__ == '__main__':
    domainsFolder = arcpy.GetParameterAsText(0)
    targetGDB = arcpy.GetParameterAsText(1)
    
    updateDomains(domainsFolder, targetGDB)