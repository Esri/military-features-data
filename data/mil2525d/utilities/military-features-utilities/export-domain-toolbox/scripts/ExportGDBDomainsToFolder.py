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
# ExportGDBDomainsToFolder.py
# Description: Exports all domains from a Military Features GDB
# Requirements: ArcGIS Desktop/Pro
# ----------------------------------------------------------------------------------
import csv
import os
import arcpy

def exportDomains():
	gdb    = arcpy.GetParameter(0)
	folder = arcpy.GetParameter(1)

	if (gdb == '') or (gdb is None):
		arcpy.AddError('Input GDB not provided')
		return

	if (folder == '') or (folder is None):
		arcpy.AddError('Output folder not provided')
		return

	domains = arcpy.da.ListDomains(gdb)

	for domain in domains:
		domainName  = domain.name
		csvFileName = 'Coded_Domain_' + domainName + '.csv'
		if domain.domainType == 'CodedValue':
			arcpy.AddMessage('Exporting Domain: ' + domainName + ' to CSV: ' + csvFileName)

			codedValues = domain.codedValues

			# Create a csv file and writer to export the domains
			# we do this versus the DomainToTable snippet below* 
			# so we can make the format closely match the format of the exported tables
			# at: https://github.com/Esri/joint-military-symbology-xml/tree/master/samples/name_domains_values
			# (so we can then compare the 2 outputs with a "diff" utility)
			csvFullFileName = os.path.join(str(folder), csvFileName)
			csvFile = open(csvFullFileName, 'w', newline='')
			writer = csv.writer(csvFile, delimiter=',')
			writer.writerow(["Name", "Value"])  # make header row

			# sort the codes/keys so things are exported in order
			sortedCodedValues = sorted(codedValues.keys())

			# write each line to the csv in the [description, value] format
			for val in sortedCodedValues:
				writer.writerow([codedValues[val], val])

	# *DomainToTable snippet
	# NOTE a much simpler version of the above if we don't care above the order/format of the export table
	# desc = arcpy.Describe(gdb)
	# domains = desc.domains
	# for domain in domains:
		# csv = 'Coded_Values_' + str(domain) + '.csv'
		# arcpy.AddMessage('Exporting: ' + str(domain) + ' to CSV: ' + csv)
		# table = os.path.join(str(folder), csv)
		# arcpy.DomainToTable_management(gdb, domain, table, 'Value','Name', '#')

if __name__ == '__main__':
	exportDomains()