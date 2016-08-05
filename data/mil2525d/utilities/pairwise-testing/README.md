# military-features-data / data / mil2525d / utilities / pairwise-testing
=======================================

## Purpose

These files are the configuration and output files used with the [Pairwise Independent Combinatorial Testing (PICT)](http://www.amibugshare.com/pict/help.html) test application.  PICT is used by Military Features to generate combinations of symbol codes for testing.

* To obtain the PICT application here: http://download.microsoft.com/download/f/5/5/f55484df-8494-48fa-8dbd-8c6f76cc014b/pict33.msi

## Instructions for creating 2525 Pairwise test data
* Download PICT application from location specified above. Once it is uploaded, make sure you look at the [PICT Users Guide](http://www.amibugshare.com/pict/help.html). 
* Organize your input text files. The input files should be formatted so that what would be a field in a geodatabase is a key, with the domains being the values. You'll see in [this example](./pict/input/ActivitiesPICT.txt) that fields such as `identity`, `entity`, `modifier1`, and `echelon` are  represented as keys, with a list of domains as their associated values. 
* Open a Command Prompt.
* Navigate (`cd`) to the location of the input file you want to run. 
* Enter `InputFolder> pict InputFile.txt > OutputFile.xls` to run the PICT application. You should see an `OutputFile.xls` appear in the same folder as your input.
* Save each output .xls file as a .csv in a new folder. Make sure your CSV has a OID primary key. 
* Open up ArcGIS Desktop and add this [toolbox](./mil2525c_b2/test_data/Complete_test_scripts/CompleteTestData.tbx) to your project.
* Run the `CreateTestFeatureClasses` model, with the location of your PICT output .csv files as your input folder. 
* Select an output geodatabase and run the model. Check for the output feature layers and add them to the map. For each feature layer, navigate to the Symbology pane, select Dictionary, and link the layer fields with the symbology fields to properly view your data. 
* To view the output pairwise test data, select the 2525 dictionary associated with your data in the Symbology pane.