# military-overlay / utils-and-source-data / pairwise-test-generation / Complete_test_scripts
==========================

# Purpose 

The script and model in this folder automates the workflow for creating comprehensive test datasets for 2525 Military Standards.

## Instructions 

* In an ArcGIS Pro Project, add one of the Military Overlay Information Models (they are Layer Packages). Connect this `Complete_test_scripts` folder to the project.
* Open the `FilterCSVMilSymbols` Script. For the File to Filter parameter, use any 2525 source .csv, and the script will create output .csv files filtered by their appendix. For example, the source .csv files for 2525 C and Bc2 can be found:
    * [2525C](../../../../military-symbology-styles/test-data/mil2525c/truth-data)
    * [2525Bc2](../../../../military-symbology-styles/test-data/mil2525b2/truth-data)    
* Choose an appropriate output folder and run the script. 
* Open the `CreateTestFeatureClasses` model and use your output folder from the previous script as your new input folder.
* Choose the output geodatabase and run the model.
* To ensure that you've created a comprehensive test dataset, make sure the symbology settings are pointing to the correct dictionary. 

__Note__: These scripts currently only work for __Point__ features.
