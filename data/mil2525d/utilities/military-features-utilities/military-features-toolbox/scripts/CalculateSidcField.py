#----------------------------------------------------------------------------------
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
#----------------------------------------------------------------------------------
# CalculateSidcField.py
# Description: Calculates SIDC Field from attributes
# Requirements: ArcGIS Desktop
#----------------------------------------------------------------------------------

import arcpy
import os
import traceback
import uuid

def symbolIdCodeAttributesToCode(attributes) : 

	#  Digits (1 & 2) (Version = "10" = 2525D.0)
	symbolIdCode = '10'  

	# Digit 3 (RealExerciseSimulation)
	if 'context' in attributes : 
		symbolIdCode += attributes['context']
	else : 
		symbolIdCode += '0'

	# Digit 4 (Affiliation)
	if 'identity' in attributes : 
		symbolIdCode += attributes['identity']
	else : 
		symbolIdCode += '0'

	# Digit 5-6
	if 'symbolset' in attributes : 
		symbolIdCode += attributes['symbolset'].zfill(2)
	else : 
		symbolIdCode += '00'

	# Digit 7 (Status)
	if 'operationalcondition' in attributes : 
		symbolIdCode += attributes['operationalcondition']
	else : 
		symbolIdCode += '0'

	# Digit 8 (HQ_TF_FD)
	if 'indicator' in attributes : 
		symbolIdCode += attributes['indicator']
	else : 
		symbolIdCode += '0'
	
	# Digit 9-10
	# Tricky can come from 1 of 3 different attributes
	if 'echelon' in attributes : 
		symbolIdCode += attributes['echelon'].zfill(2)
	else : 
		if 'mobility' in attributes : 
			symbolIdCode += attributes['mobility'].zfill(2)
		else : 
			if 'array' in attributes : 
				symbolIdCode += attributes['array'].zfill(2)
			else : 
				symbolIdCode += '00'

	# Digit 11-16
	if 'entity' in attributes : 
		symbolIdCode += attributes['entity'].zfill(6)
	else : 
		symbolIdCode += '000000'

	# Digit 17-18
	if 'modifier1' in attributes : 
		symbolIdCode += attributes['modifier1'].zfill(2)
	else : 
		symbolIdCode += '00'

	# Digit 19-20
	if 'modifier2' in attributes : 
		symbolIdCode += attributes['modifier2'].zfill(2)
	else : 
		symbolIdCode += '00'
	
	# Final Check:
	symbolIdCodeLength = len(symbolIdCode)
	if symbolIdCodeLength != 20 :
		arcpy.AddWarning("Unexpected Length: Symbol ID Code: " + \
			symbolIdCode + ", Length: " + str(symbolIdCodeLength))

	return symbolIdCode

### calculateSidcField - Calculates a Symbol ID Code field for a Military Feature Class
###
### Params:
### 0 - input_feature_class (FeatureClass) - input Military Feature Class
### 1 - sidc_field (String) - field to store SIDC value
###
def calculateSidcField() :
	
	try :
		arcpy.AddMessage('Starting: CalculateSidcField')

		currentPath = os.path.dirname(__file__)
		defaultDataPath = os.path.normpath(os.path.join(currentPath, r'Data/'))

		# 0 : Get input feature class
		inputFC = arcpy.GetParameter(0)
		if (inputFC == '') or (inputFC is None):
			inputFC = os.path.normpath(os.path.join(defaultDataPath, r'MilitaryOverlay.gdb/MilitaryFeatures/Units'))

		try : 
			desc = arcpy.Describe(inputFC)
		except :
			desc = None

		if desc == None :
			arcpy.AddError('Could not read Input Feature Class: ' + str(inputFC))
			return

		# 1: sidcField
		sidcField = arcpy.GetParameterAsText(1)

		if (sidcField == '') or (sidcField is None):
			sidcField = 'staffcomment'

		arcpy.AddMessage('Running with Parameters:')
		arcpy.AddMessage('0 - Input Military Feature Class: ' + str(inputFC))
		arcpy.AddMessage('1 - SIDC Field: ' + sidcField)

		SYMBOL_ID_FIELD_LIST = ['context', 'identity', 'symbolset', 'entity', 'modifier1', 'modifier2', 'echelon', \
			'mobility', 'array', 'indicator', 'operationalcondition' ]

		# Get a list of available feature class fields (we use this in a few places)
		fieldNameList = []

		for field in desc.Fields:
			fieldNameList.append(field.name)
			if field.name == sidcField :
				if field.type != 'String' : 
					arcpy.AddError('SIDC Field: ' + sidcField + ' is not of type string/text, type: ' + field.type)
					return

		if not (sidcField in fieldNameList) : 
			arcpy.AddError('Could not find field: ' + sidcField)
			return

		# Open an update cursor (if possible)
		try :            
			fieldNameListAsString = ','.join(fieldNameList) # Change into format expected by UpdateCursor
			features = arcpy.gp.UpdateCursor(inputFC, '', None, fieldNameListAsString) 
		except Exception as err: 
			arcpy.AddError('Could not open Input Feature Class ' + str(inputFC))
			arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())           
			return

		featureCount = 0

		for feature in features : 

			featureCount += 1
			arcpy.AddMessage('Processing feature/message: ' + str(featureCount))

			symbolIdCodeAttributes = {}

			# Get all field attributes
			for field in fieldNameList:
				try : 
					featureVal = feature.getValue(field)
				except :
					arcpy.AddWarning('Could not get feature value for field: ' + field)
					featureVal = None
					
				if featureVal is not None:

					if field in SYMBOL_ID_FIELD_LIST : 
						fieldValAsString = str(feature.getValue(field))
						symbolIdCodeAttributes[field] = fieldValAsString

			symbolIdCode = symbolIdCodeAttributesToCode(symbolIdCodeAttributes)

			try : 
				feature.setValue(sidcField, symbolIdCode)

				features.updateRow(feature)
			except :
				arcpy.AddError('Could not update feature value for field: ' + sidcField)

	except Exception as err: 
		arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())

	finally :
		if feature : 
			del feature

		if features : 
			del features

if __name__ == '__main__':
	calculateSidcField()