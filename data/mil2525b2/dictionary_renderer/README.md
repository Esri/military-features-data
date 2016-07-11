Dictionary Renderer for MIL-STD-2525B Change 2
==============================================

# Purpose 

This folder contains the Dictionary Renderer Plugin/Rule Engine for the specified standard version. 

# Requirements

The Dictionary Renderer requires one of the following supported ArcGIS products:

* ArcGIS Pro 1.2
* ArcGIS Server 10.4

# Using the Dictionary Renderer

To add a new Dictionary Renderer to ArcGIS Pro (only mil2525d is installed with the ArcGIS Pro), perform the following:

* Ensure ArcGIS Pro is not running 
* Copy the folder for the desired standard version included in this folder to the ArcGIS Pro `Resources\Dictionaries` installation location.
    * A [script is provided](./InstallDictionaryRenderer-ArcGISPro.bat) to copy/deploy this folder (requires Pro to be installed at default location)
        * This script may need to be "Run As Administrator" if ArcGIS Pro is installed under `C:\Program Files`
    * To manually copy, copy the `mil2525b2` folder and contents from this folder into `{ProInstallLocation`*`}\ArcGIS\Pro\Resources\Dictionaries`
        * `*` Pro is normally installed to (1) `C:\Program Files\ArcGIS\Pro` (All Users) -or- (2) `%LOCALAPPDATA%\Programs\ArcGIS\Pro` (Single User)
* Run ArcGIS Pro, select the Dictionary Renderer on the Symbology Tab and ensure the new standard is available
    * See the [ArcGIS Pro Help Page](https://pro.arcgis.com/en/pro-app/help/mapping/symbols-and-styles/dictionary-renderer.htm) for more information
