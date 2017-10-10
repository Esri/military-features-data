# military-features-data / data / mil2525d / utilities / style-utilities / merge-stylx-utilities

## Purpose

* Merges:
    * A version of the .stylx that is edited and maintained in ArcGIS Pro
    * See: [mil2525d-lines-areas-labels-base-template.stylx](../../../core_data/stylxfiles/mil2525d-lines-areas-labels-base-template.stylx)
* -with-
    * A version of the .stylx file with the SVG/EMF marker/points-only icons. This version is normally automatically generated from SVG icon marker symbols and meta-data-tagged source data from the [joint-military-symbology-xml repository](https://github.com/Esri/joint-military-symbology-xml)
    * For more information on this data/process/version, see :
        * [joint-military-symbology-xml repo](https://github.com/Esri/joint-military-symbology-xml)
        * [style-file-utilities](../style-file-utilities)

## Requirements

* `(1)` 2 Versions of the .stylx files:
    * A version of the .stylx file with the SVG/EMF icons 
        * A Pro/Runtime stylx file with all of the 2525D Point Icons called `mil2525d-points-only.stylx`
        * This would have been created [by following the steps/readme here](../../utilities)
        * If you don't already have this file, you can create one by (1) importing the 10.X **.style** file with these point icons into ArcGIS Pro and (2) renaming the resulting file to `mil2525d-points-only.stylx` - steps:
            * Import the file ["mil2525d-points-only.style"](https://github.com/Esri/military-features-data/blob/dev/data/mil2525d/core_data/stylefiles/mil2525d-points-only.style) into Pro (Note: this is a 10.X **style** file until you import it into Pro, after which time a **.stylx** file will be created)
            * Rename `Military 2525Delta All.stylx` to `mil2525d-points-only.stylx`
    *  A version of the .stylx that is edited and maintained in ArcGIS Pro: [mil2525d-lines-areas-labels-base-template.stylx](../../../core_data/stylxfiles/mil2525d-lines-areas-labels-base-template.stylx)
        *  This version has Label Placement definitions, Point, Line, Polygon, and Text Symbols that are manually created and maintained in ArcGIS Pro
        *  This file is already included with this repo
*  `(2)` A SQLite Engine/Editor/Interpreter capable of executing SQL commands from a script
    *  These steps have been tested using [sqlite3.exe](http://www.sqlite.org/download.html) but you may use the application/sqlite engine of your choice
*  `(3)` A Style Icon specification file: [Military-All-Icons.csv](https://github.com/Esri/joint-military-symbology-xml/blob/master/samples/imagefile_name_category_tags/Military-All-Icons.csv) with the following required fields: `styleItemCategory, styleItemName, styleItemUniqueId, styleItemTags`

## Instructions 

* Copy/edit/verify the prerequisite files are present
    * Copy the required "Points/Icons-Only" version of the .stylx mentioned in Requirements above to this folder and ensure it is named: `mil2525d-points-only.stylx`
        * This file should now exist locally: `...utilities\style-utilities\merge-stylx-utilities\mil2525d-points-only.stylx`
    * Clone the [joint-military-symbology-xml](https://github.com/Esri/joint-military-symbology-xml) repository locally if you have not already done so.
        * This file should now exist locally: `...joint-military-symbology-xml\samples\imagefile_name_category_tags\Military-All-Icons.csv`
    * Edit the versions.csv file to update the pertinent version information, this information will be inserted into the completed/merged mil2525d.stylx file
        * This file should now be current with the updated/desired version information: `...utilities\style-utilities\merge-stylx-utilities\versions.csv`
* Edit the automated scripts to point to the paths/configuration of your local machine:
    * Use a text editor of your choice, such as Notepad/Notepad++, to edit and save local versions of the following files
    * Edit the batch file: [AutomatedCopyandMerge.bat](./AutomatedCopyandMerge.bat), set/replace the following settings:
        * These settings read `{TODO_EDIT_THIS_PATH}` and are:
        * The local/cloned copy of the Joint Military Symbology repository: `SET LOCAL_JMSXML_REPO_CLONE=________`
        * The path to the SQLite executable/engine: `SET SQLITE_PATH_AND_EXE=________`
        * Save the updated version of the batch file
    * Edit the SQLite script [SqliteMergeStylx.sql](./SqliteMergeStylx.sql) to provide the fully qualified local path to this folder (`...utilities/style-utilities/merge-stylx-utilities`) 
        * **IMPORTANT**: the SQLite engine `sqlite3.exe` will *not* recognize backslashes (`\` - MS-DOS paths) so forward slashes (`/` - Unix-style paths) must be used.
        * Replace all instances (currently 4 instances) of `{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}` with the fully qualified local path to this folder: `....utilities/style-utilities/merge-stylx-utilities` being careful to
            * Use  forward slashes (`/`) rather than backslashes (`\`)
            * Scroll through the entire file to check for replacements, several instances are near the bottom, end of the file
        * Save the updated version of the .sql file
* Run the automated script to merge the two stylx files - [AutomatedCopyandMerge.bat](./AutomatedCopyandMerge.bat)
    * After the batch file has run, verify that there are no errors in the output
* Test and update the resulting `mil2525d.stylx` created. To test you may:
    * Open the mil2525d.stylx in ArcGIS Pro -or-
    * You may replace/update the .stylx used mil2525d DictionaryRenderer by replacing the file at `ArcGIS\Pro\Resources\Dictionaries\mil2525d`
* Once you have tested for correctness, you may wish to update the managed version in this repository at [core_data/stylxfiles](../../../core_data/stylxfiles)



