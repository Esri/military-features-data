# ----------------------------------------------------------------------------------
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
# ExportGDBFieldsToFolder.py
# Description: Exports all fields and field properties from a Military Features GDB
#      as a set of csvs to the folder selected
#-------------------------------------------------------------------------------
# Requires: ArcGIS Desktop/Pro, arcpy, Python 2 or 3
#-------------------------------------------------------------------------------

import csv
import os
import sys

import arcpy

def exportFields():
	gdb    = arcpy.GetParameter(0)
	folder = arcpy.GetParameter(1)

	if (gdb == '') or (gdb is None):
		arcpy.AddError('Input GDB not provided')
		gdb = 'C:/Github/military-features-data/data/mil2525d/core_data/gdbs/MilitaryOverlay.gdb'
		# return

	if (folder == '') or (folder is None):
		arcpy.AddError('Output folder not provided')
		folder = 'C:/TestData/ExportedFields'
		# return

	# Check Input
	try : 
		desc = arcpy.Describe(gdb)
		if desc == None :
			arcpy.AddError('Could not open GDB: ' + str(gdb))
			print('Exiting...')
			return
	except Exception as openEx :
		arcpy.AddError('Could not open GDB: ' + str(gdb))
		print('Exiting...')
		return

	# Set the workspace for ListFeatureClasses
	arcpy.env.workspace = gdb

	featureClasses = arcpy.ListFeatureClasses(feature_dataset='MilitaryOverlay')

	for featureClass in featureClasses : 

		csvFileName = 'Fields_' + str(featureClass) + '.csv'

		arcpy.AddMessage('Exporting ' + str(featureClass) + ' to CSV: ' + csvFileName)

		# Create a csv file and writer to export the fields
		csvFullFileName = os.path.join(str(folder), csvFileName)
		
		if sys.version < '3' :      # Python 2 or 3 check for csv difference
			csvFile = open(csvFullFileName, 'wb', newline='')
		else :
			csvFile = open(csvFullFileName, 'w', newline='')

		writer = csv.writer(csvFile, delimiter=',')

		# Expected order/format:
		# field_name,field_type,field_length,field_alias,nullability,field_domain
		# Make header row:
		writer.writerow(["field_name", "field_type", "field_length", "field_alias", "nullability", "field_domain"])

		fields = arcpy.ListFields(featureClass)

		for field in fields : 
			
			# Skip OID & Shape fields
			if 'OBJECTID' in field.name or 'SHAPE' in field.name or 'Shape' in field.name :
				print('Skipping field: ' + field.name)
				continue

			if field.isNullable : 
				nullability = 'NULLABLE'
			else :
				nullability = 'NON_NULLABLE'

			row = [field.name, str(field.type), str(field.length), field.aliasName, \
			    nullability, field.domain]

			arcpy.AddMessage('Field Name: ' + field.name + ', Properties= ' + str(row))
			writer.writerow(row)

		csvFile.close()

if __name__ == '__main__':
	exportFields()