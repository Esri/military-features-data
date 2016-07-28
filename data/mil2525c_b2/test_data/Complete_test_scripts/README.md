# military-features-data / data / mil2525c_b2 / test_data / Complete_test_scripts
==========================

# Purpose 

The script and model in this folder automates the workflow for creating comprehensive test datasets for 2525 Military Standards.

## Instructions 

* Connect the `Complete_test_scripts` folder to one of the Military Overlay Information Models for ArcGIS Pro.
* Open the `FilterCSVMilSymbols` Script. For the File to Filter parameter, use any 2525 source .csv, and the script will create output .csv files filtered by their appendix. For example, the source .csv files for 2525 C and Bc2 can be found [here](https://github.com/Esri/military-features-data/tree/v.next/data/mil2525c_b2/test_data/truth_data). 
* Choose an appropriate output folder and run the script. 
* Open the `CreateTestFeatureClasses` model and use your output folder from the previous script as your new input folder.
* Choose the output geodatabase and run the model.
* To ensure that you've created a comprehensive test dataset, make sure the symbology settings are pointing to the correct dictionary. 

__Note__ These scripts currently only work for __Point__ features.
