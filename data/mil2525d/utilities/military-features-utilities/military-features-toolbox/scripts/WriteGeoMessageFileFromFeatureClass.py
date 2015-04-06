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
# WriteGeoMessageFileFromFeatureClass.py
# Description: Converts Military Feature Class to XML file
# Requirements: ArcGIS Desktop
#----------------------------------------------------------------------------------

import arcpy
import os
import traceback
import uuid

def parsePartToControlPoints(part):
	controlPoints = ''
	sep = ''
	
	try :
	
		for subpart in part:
			try:
				# assume it's a point
				subpartStr = str(subpart.X) + ',' + str(subpart.Y)
				controlPoints = controlPoints + sep + subpartStr
				sep = ';'
			except AttributeError:
				# it's an array of parts, i.e. a part
				controlPoints = controlPoints + sep + parsePartToControlPoints(subpart)
				sep = ';'
								
	except :
		print('Exception in parsePartToControlPoints')
			
	return controlPoints

def parseGeometryToControlPoints(geom):
	try:
		# assume it's a point
		return str(geom.X) + ',' + str(geom.Y)
	except AttributeError:
		# it's not a point
		try:
			controlPoints = ''
			sep = ''
			for i in range(geom.partCount):
				part = geom.getPart(i)
				# part is an array
				for subpart in part:
					controlPoints = controlPoints + sep + parsePartToControlPoints(part)
					sep = ';'
			return controlPoints
		except AttributeError:
			# it's a part
			return parsePartToControlPoints(geom)

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

### writeMessageFile - Converts a Feature Class to an XML file
###
### Params:
### 0 - input_feature_class (FeatureClass)
### 1 - output_message_file (File) - XML Files to export to
### 2 - message_type_field (String)
### 3 - use_domain_codes (Boolean) - Use Domain Codes (instead of Domain Descriptions) 
### 4 - include_symbol_id_code (Boolean) - Include Symbol ID Code
### 5 - sort_fields (string) (ex: sort_fields='STATE_NAME A; POP2000 D') see:
###     http://resources.arcgis.com/en/help/main/10.1/index.html#//018v00000050000000)
###
def writeMessageFile() :
	
	try :
		arcpy.AddMessage('Starting: Write Message File')

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

		shapeType = desc.shapeType
		
		# 1: Get output filename
		outputFile = arcpy.GetParameterAsText(1)

		if (outputFile == '') or (outputFile is None):
		    outputFile = os.path.normpath(os.path.join(defaultDataPath, r'UnitsMessages.xml'))

		# Open Output File for writing (if possible)
		try :            
			messageFile=open(outputFile, 'w')
		except :            
			arcpy.AddError('Could not open Output File for writing: ' + str(outputFile))
			return

		# 2: Message Type Field
		messageTypeField = arcpy.GetParameterAsText(2)   
		
		# 3: useDomainCodes (Boolean) - Use Domain Codes (instead of Domain Descriptions) 
		useDomainCodes = arcpy.GetParameterAsText(3) 

		if (useDomainCodes == '') or (useDomainCodes is None):
			useDomainCodes = False

		# 4: includeSymbolIdCode - Include Symbol Id Code (Boolean)
		includeSymbolIdCode = arcpy.GetParameterAsText(4) 

		if (includeSymbolIdCode == '') or (includeSymbolIdCode is None):
			includeSymbolIdCode = False

		# 5: Sort Order
		orderBy = arcpy.GetParameterAsText(5)       
		
		arcpy.AddMessage('Running with Parameters:')
		arcpy.AddMessage('0 - Input Military Feature Class: ' + str(inputFC))
		arcpy.AddMessage('1 - Output Message XML File: ' + str(outputFile))
		arcpy.AddMessage('2 - MessageTypeField: ' + messageTypeField)
		arcpy.AddMessage('3 - UseDomainCodes: ' + str(useDomainCodes))
		arcpy.AddMessage('4 - IncludeSymbolIdCode: ' + str(includeSymbolIdCode))
		arcpy.AddMessage('5 - orderBy: ' + orderBy)

		# TODO add any other fields/attributes to exclude from XML export
		FIELD_EXCLUDE_LIST = [desc.ShapeFieldName, desc.OIDFieldName, \
							'SHAPE_Length', 'SHAPE_Area' ]

		SYMBOL_ID_FIELD_LIST = ['context', 'identity', 'symbolset', 'entity', 'modifier1', 'modifier2', 'echelon', \
			'mobility', 'array', 'indicator', 'operationalcondition' ]

		# Get a list of available feature class fields (we use this in a few places)
		# Also keep a dictionary of domains used
		fieldNameList = []
		fieldNameToDomainName = {}
		domainNameToDomainValues = {}
		CODE_FIELD_NAME = "code"
		DESCRIPTION_FIELD_NAME = "description"

		for field in desc.Fields:
			fieldNameList.append(field.name)

			if useDomainCodes : 
				# don't do the rest if we are using the domain codes only (and not descriptions)
				# so we don't need to query the GDB for these descriptions
				continue 

			# Generate domains descriptions (needed later to convert domain numbers/values to strings)
			if (field.domain is not None and field.domain != ""):
				fieldNameToDomainName[field.name] = field.domain
				dataPath = desc.path
				# Strip off any FeatureDatasets from the name (TODO: this only works with GDBs for now)
				gdbPath = dataPath.split(".gdb")[0]
				gdbPath += ".gdb"
				arcpy.DomainToTable_management(gdbPath, field.domain, "in_memory/" + field.domain, CODE_FIELD_NAME, DESCRIPTION_FIELD_NAME)

				domainValueToDomainDescriptions = {}
				
				domainRows = arcpy.gp.SearchCursor("in_memory/" + field.domain)
				for domainRow in domainRows:
					code = str(domainRow.getValue(CODE_FIELD_NAME))
					description = domainRow.getValue(DESCRIPTION_FIELD_NAME)
					domainValueToDomainDescriptions[code] = description

				domainNameToDomainValues[field.domain] = domainValueToDomainDescriptions

		wkid = desc.spatialReference.factoryCode

		# Open a search cursor (if possible)
		try :            
			fieldNameListAsString = ','.join(fieldNameList) # Change into format expected by SearchCursor
			features = arcpy.gp.SearchCursor(inputFC, '', None, fieldNameListAsString, orderBy) 
		except Exception as err: 
			arcpy.AddError('Could not open Input Feature Class ' + str(inputFC))
			arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())           
			return

		featureCount = 0

		# In case version/format needed:
		# messageFile.write('<?xml version="1.0" encoding="UTF-8"?>\n')

		messageFile.write('<geomessages>\n')

		for feature in features : 

			featureCount += 1
			arcpy.AddMessage('Processing feature/message: ' + str(featureCount))

			symbolIdCodeAttributes = {}

			# Get shape as string in message control point format x1, y1; x2, y2;
			shape = feature.shape.getPart(0)
			controlPointsString = parseGeometryToControlPoints(shape)

			uniqueId = '{%s}' % str(uuid.uuid4())
			messageType = 'position_report'
			messageAction = 'update'

			# Try to get a message type from field if 1) the parameter is set and 2) the field exists
			if not ((messageTypeField == '') or (messageTypeField is None)):
				try :   
					messageType = feature.getValue(MilitaryUtilities.MessageTypeField)           
				except :             
					messageType = 'position_report' # or (pass)

			messageFile.write('\t<geomessage>\n')
			messageFile.write('\t\t<_id>%s</_id>\n' % uniqueId) 
			messageFile.write('\t\t<_type>%s</_type>\n' % messageType)
			messageFile.write('\t\t<_wkid>%i</_wkid>\n' % wkid)
			messageFile.write('\t\t<_action>%s</_action>\n' % messageAction)     
			messageFile.write('\t\t<_control_points>%s</_control_points>\n' % controlPointsString)  

			# Write out all fields as Tag attributes
			for field in fieldNameList:
				try : 
					# Don't inlcude any on the "Exclude" list (shape, OID, etc.)
					if field in FIELD_EXCLUDE_LIST :
						featureVal = None 
					else :
						featureVal = feature.getValue(field)
				except :
					arcpy.AddWarning('Could not get feature value for field: ' + field)
					featureVal = None
					
				if featureVal is not None:

					fieldValAsString = str(feature.getValue(field))

					if field in SYMBOL_ID_FIELD_LIST : 
						symbolIdCodeAttributes[field] = fieldValAsString

					# if we want to expand the domain code into a description...
					if not useDomainCodes : 
						# convert code to description if its a field with a domain
						if field in fieldNameToDomainName:
							domain = fieldNameToDomainName[field]
							if domain in domainNameToDomainValues : 
								domainValues = domainNameToDomainValues[domain]
								description = domainValues[fieldValAsString]
								fieldValAsString = description # now replace code with description

					messageFile.write('\t\t<'+field+'>' + fieldValAsString + '</' + field + '>\n')

			if includeSymbolIdCode : 
				if "sidc" in fieldNameList :
					pass # it has already been included, don't need to calculate
				else : 
					symbolIdCode = symbolIdCodeAttributesToCode(symbolIdCodeAttributes)
					messageFile.write('\t\t<sidc>%s</sidc>\n' % symbolIdCode)  

			messageFile.write('\t</geomessage>\n')

		messageFile.write('</geomessages>')

	except Exception as err: 
		arcpy.AddError(traceback.format_exception_only(type(err), err)[0].rstrip())

	finally :
		if feature : 
			del feature

		if features : 
			del features

if __name__ == '__main__':
	writeMessageFile()