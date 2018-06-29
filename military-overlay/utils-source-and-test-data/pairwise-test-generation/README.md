# military-overlay / utils-and-source-data / pairwise-test-generation
=======================================

## Purpose

These files are the configuration and output files used with the [Pairwise Independent Combinatorial Testing (PICT)](http://www.amibugshare.com/pict/help.html) test application.  PICT is used by Military Features to generate combinations of symbol codes for testing. The source data for the legacy PICT files is located here. 

* To obtain the PICT application here: http://download.microsoft.com/download/f/5/5/f55484df-8494-48fa-8dbd-8c6f76cc014b/pict33.msi

## Instructions for creating 2525 Pairwise test data
* Download PICT application from location specified above. Once it is downloaded, make sure you look at the [PICT Users Guide](http://www.amibugshare.com/pict/help.html). 
* Organize your input text files. The input files should be formatted so that what would be a field in a geodatabase is a key, with the domains being the values. You'll see in [this 2525D example](../pairwise-source-data/mil2525d/pict/input/ActivitiesPICT.txt) that fields such as `identity`, `entity`, `modifier1`, and `echelon` are  represented as keys, with a list of domains as their associated values. 
* Open a Command Prompt.
* Navigate (`cd`) to the location of the input file you want to run. 
* Enter `InputFolder> pict InputFile.txt > OutputFile.xls` to run the PICT application. You should see an `OutputFile.xls` appear in the same folder as your input.
* Save each output .xls file as a .csv in a new folder. Add and OID field at the beginning of each new .csv and populate it with values that count each row, starting at 1. 
* Open up ArcGIS Desktop and add this [toolbox](./Complete_test_scripts/CompleteTestData.tbx) to your project.
* Run the `CreateTestFeatureClasses` model, with the location of your PICT output .csv files as your input folder. 
* Select an output geodatabase and run the model. Check for the output feature layers and add them to the map. For each feature layer, navigate to the Symbology pane, select Dictionary, and choose the fields in the layer that match the ones the dictionary renderer uses to properly view your data.
* To view the output pairwise test data, select the 2525 dictionary associated with your data in the Symbology pane.