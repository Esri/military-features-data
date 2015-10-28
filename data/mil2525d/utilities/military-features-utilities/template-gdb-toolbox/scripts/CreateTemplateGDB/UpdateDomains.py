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
import glob

from arcpy import env
from os import path

def updateDomains(domainsFolder, targetGDB):
    try:
        arcpy.AddMessage("Starting: UpdateDomains")
        
        path = os.path.normpath(os.path.join(domainsFolder, "Coded_Domain_*.csv"))
        for fname in glob.glob(path):
            
            # Iterate over each coded domain CSV file in the domain
            # folder and determine the name and description of each domain
            
            theFile = os.path.basename(fname)
            domainFileName, fileExt = os.path.splitext(theFile)
            domainName = domainFileName.replace("Coded_Domain_", " ").strip()
            domainDescription = domainName.replace('_',' ')
            
            # Now update a domain in the target gdb using the information
            # gathered
            
            arcpy.AddMessage("Updating domain " + domainName + "...")
            arcpy.TableToDomain_management(fname, "Value", "Name", targetGDB, domainName, domainDescription, "REPLACE")

    except Exception as err: 
        arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())
        
    finally:
        arcpy.AddMessage("Success! - Completed: UpdateDomains")

if __name__ == '__main__':
    domainsFolder = arcpy.GetParameterAsText(0)
    targetGDB = arcpy.GetParameterAsText(1)
    
    updateDomains(domainsFolder, targetGDB)