# military-features-data / data / mil2525d / sample_data
==========================

# Purpose 

The data in this folder contains sample data demonstrating the military features symbology schema including reference samples and test data using military features.

## General Instructions 

See the Component Sections for more information:

* Geodatabases (.gdbs) using Military Feature data
	* See [gdbs](./gdbs)
* Projects - ArcGIS Professional Stand-alone Project Files (.ppkx)
	* See [projects](./projects)
	* Note: ArcGIS Professional required
	
## Stepts to create a Comprehensive Test Dataset

The purpose of these steps is to make repeatable the process of creating test datasets for MIL-STD-2525 Projects (This workflow is the same for D, C, BC2 and App6 symbol sets).

1. Open Microsoft Excel.
2. Open a Master Excel file, containing information about all of the symbols in a particular version of the standard.
3. Create a filter on Appendix, Affiliation, and Geometry.
4. Sort on each field in the order above.
5. Copy the results to a new tab in Excel.
6. Rename thte Count field to OID.
7. In the OID field, create a sequential number for the first 3 records, then drag your cursor down, this will autopopulate the field (Ex. 1, 2, 3, ...)
8. Save this as document as a .csv.
9. Repeat steps until all desired appendices have been derived.
10. Open ArcGIS Pro and create a new map.
11. Add the csv files to the project using "add data." The tables will be added to your contents pane"
12. Under Analysis, search for the Create Fishnet GP tool. 
	* The output feature class will be the feature class you want to create for a particular appendix.
	* Change the template extent to "Current Display"
	* Use the attached spreadsheet for the appendix you want to test to figure out the number of rows and columns.
		For example, if there are 100 symbols in an appendix, you could create a fishnet with 10 rows and 10 columns. 
	* Select "Create Label Points"
13. Run the Create Fishnet tool.
	* the only output that is pertinent to this workflow are the label points.
14. Join the .csv of your desired appendix to the Fishnet output.
	* right-click on the Fishnet output and "Add Join"
	* The input and output join fields will both be OID
	* The join table will be the corresponding appendix .csv.
	* Run to excecute tool and check to make sure that all of the .csv fields were properly joined to the fishnet points.
15. Symbolize the points using the dictionary renderer
	* Under the symbology pane for the fishnet feature layer, select your desired dictionary renderer and ensure the fields are properly matched up.

