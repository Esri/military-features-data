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
# ExportGDBSubTypesToFolder.py
# Description: Exports all GDBSubTypes from a Military Features GDB to a set of csvs
# Requirements: arcpy, ArcGIS Desktop/Pro, Python 2 or 3
# ----------------------------------------------------------------------------------

import csv
import os
import sys

import arcpy

def exportSubTypes():

	gdb    = arcpy.GetParameter(0)
	folder = arcpy.GetParameter(1)

	if (gdb == '') or (gdb is None):
		arcpy.AddError('Input GDB not provided')
		gdb = 'C:/Github/military-features-data/data/mil2525d/core_data/gdbs/MilitaryOverlay.gdb'
		# return

	if (folder == '') or (folder is None):
		arcpy.AddError('Output folder not provided')
		folder = 'C:/Data/ExportedSubtypes'
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

		csvFileName = 'SubTypes_' + str(featureClass) + '.csv'

		arcpy.AddMessage('Exporting: ' + str(featureClass) + ' to CSV: ' + csvFileName)

		# Create a csv file and writer to export the fields
		csvFullFileName = os.path.join(str(folder), csvFileName)
		
		if sys.version < '3' :      # Python 2 or 3 check for csv difference
			csvFile = open(csvFullFileName, 'wb', newline='')
		else :
			csvFile = open(csvFullFileName, 'w', newline='')

		writer = csv.writer(csvFile, delimiter=',')

		#Expected order/format:
		# subtype_code, subtype_field, subtype_name, field_name, field_default_value, field_domain
		#Make header row:
		writer.writerow(["subtype_code", "subtype_field", "subtype_name", "field_name", \
		   "field_default_value", "field_domain", "is_subtype_default"])

		print('----------------------------------------------')
		print('Exporting: ' + str(featureClass))
		print('----------------------------------------------')

		subtypes = arcpy.da.ListSubtypes(featureClass)
		# print(subtypes)

		subtype_code, subtype_field, subtype_name, field_name, field_default_value, field_domain, is_subtype_default = \
			'NOT_SET', 'NOT_SET', 'NOT_SET', 'NOT_SET', 'None', '', 'False'

		# Adapted from iterating over subtypes solution borrowed from:
		#  http://gis.stackexchange.com/questions/104539/how-to-get-the-name-of-a-subtype-field-in-a-feature-class-in-python

		for stcode in subtypes:
			print('-------------------------')
			print('Code: ' + str(stcode))
			print('-------------------------')

			subtype_code = str(stcode)

			stdict = subtypes[stcode]

			if 'SubtypeField' in stdict : 
				subtype_field = stdict['SubtypeField'] 
			if 'Name' in stdict :
				subtype_name = stdict['Name'] 
			if 'Default' in stdict :
				is_subtype_default = str(stdict['Default'] )

			for stkey in stdict :
				if stkey == 'FieldValues':
					print('Fields:')
					fields = stdict[stkey]
					for field in fields : 
						fieldvals = fields[field]
						print(' --Field name: {0}'.format(field))
						field_name = field
						print(' --Field default value: {0}'.format(fieldvals[0]))
						field_default_value = fieldvals[0]
						if field_default_value is None :
							field_default_value = 'None'
						if not fieldvals[1] is None:
							print(' --Domain name: {0}'.format(fieldvals[1].name))
							field_domain = fieldvals[1].name

						writer.writerow([subtype_code, subtype_field, subtype_name, field_name, \
						   field_default_value, field_domain, is_subtype_default])

						field_name, field_default_value, field_domain = '', 'None', ''

				elif stkey == 'SubtypeField' :
					print('(SubtypeField): {0}: {1}'.format(stkey, stdict[stkey]))
				elif stkey == 'Name' :
					print('(Name): {0}: {1}'.format(stkey, stdict[stkey]))
				elif stkey == 'Default' : 
					print('(Default): {0}: {1}'.format(stkey, stdict[stkey]))
				else :    
					print('OTHER VALUE: {0}: {1}'.format(stkey, stdict[stkey]))

		csvFile.close()

if __name__ == '__main__':
	exportSubTypes()