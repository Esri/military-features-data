# military-features-data / data / mil2525d / utilities / style-utilities / style-file-utilities

## Purpose

* Imports a set of  Enhanced Meta File (.emf) vector image files into ArcGIS .style files
* Used to produce Military Feature Style Files (**point markers only** at this time)

## Requirements

* A set of .emf files to import into selected styles
    * See [../image-conversion-utilities](../image-conversion-utilities) for one method of creating .emf files
* A set of Comma-Separated Value (.csv) files containing the specifications for the images to import 
    * [joint-military-symbology-xml repo icon specification files](https://github.com/Esri/joint-military-symbology-xml/tree/master/samples/imagefile_name_category_tags) 
    * [Sample .csv specification file here](https://github.com/csmoore/csv2ArcGISStyle/blob/master/sample/myStyleSample.csv)
* ArcGIS for Desktop 10.1 (or later) **Advanced License**
    *  ArcGIS for Desktop **Advanced License** is required to create/change cartographic representation rules
* To build the .NET Solution from source in [csv2ArcGISStyle](https://github.com/csmoore/csv2ArcGISStyle) you will also need
    * Visual Studio 2010 or later
    * ArcObjects .NET Engine or Desktop Development Kit (10.1 or later)
    * If you do not need to build from source, you may skip this requirement

## Instructions 

* Locations you will need to note in the steps below:
    * `{Emf_Images_Home}` = _____________________
    * `{Csv_2_ArcGISStyle_Home}` = ____________
    * IMPORTANT: these paths/locations may not contain spaces
* Obtain the set of .emf source files to import and note the location of these files: `{Emf_Images_Home}`
    * Make note of this location: `{Emf_Images_Home}` = __________
    * See [../image-conversion-utilities](../image-conversion-utilities) for one method of creating .emf files
* (Optional) If building from source, build the Style File Import Utility csv2ArcGISStyle project at [csmoore/csv2ArcGISStyle](https://github.com/csmoore/csv2ArcGISStyle)
    * This is a fork of [williamscraigm/csv2ArcGISStyle](https://github.com/williamscraigm/csv2ArcGISStyle) - see that repo for more information on this process/source
* A sample deployment of the Style File Import Utility `csv2ArcGISStyle` is provided in this folder (Windows only)
    * Unzip the file: csv2ArcGISStyleDeployment.zip to a desired location
    * Make note of this location: `{Csv_2_ArcGISStyle_Home}` = __________
    * IMPORTANT: `{Csv_2_ArcGISStyle_Home}` will be the location of the folder that contains the file/executable: `csv2ArcGISStyle.exe`
* Edit the file  `{Csv_2_ArcGISStyle_Home}\CreateAllMilitaryStyles.bat` 
    * Change the value of `C:\{TODO_NO_SPACES_CSV2STYLEHOME}` to the value noted above in `{Csv_2_ArcGISStyle_Home}`
    *  Verify that the .csv file & style names in this .bat file are still current/valid
    *  **IMPORTANT: there should be no spaces in any of these names/paths/parameters in this .bat**
*  Update the .csv's containing the specifications for the images to import in the CsvSourceData folder
    *  The location of these files is at: `{Csv_2_ArcGISStyle_Home}`\CsvSourceData
    *  You will need to update the files in this folder with the latest versions of the specification files from the [joint-military-symbology-xml repo here](https://github.com/Esri/joint-military-symbology-xml/tree/master/samples/imagefile_name_category_tags) 
        *  Obtain the files from the link above and overwrite them in the folder:  `{Csv_2_ArcGISStyle_Home}`\CsvSourceData
    * IMPORTANT: Next, you will need to edit these files to change (i.e. find&replace) the string `{Symbols_Root}` in each specification file/.csv to your local location/full path noted in `{Emf_Images_Home}`
        * Open each file in a text editor (Notepad or Notepad+), find/replace: `{Symbols_Root}` with the full path of `{Emf_Images_Home}`, and save each file.
*  Open a command prompt:
    * `cd {Csv_2_ArcGISStyle_Home}` 
    * `CreateAllMilitaryStyles.bat`
    * Output will be produced at `{Csv_2_ArcGISStyle_Home}\StyleOutputData`
*  Verify the output in the `{Csv_2_ArcGISStyle_Home}\StyleOutputData` folder and check the styles for errors
 


