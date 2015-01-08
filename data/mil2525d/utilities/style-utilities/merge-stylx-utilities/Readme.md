# military-features-data / data / mil2525d / utilities / style-utilities / merge-stylx-utilities

## Purpose

* Merges:
* A version of the .stylx that is edited and maintain in ArcGIS Pro
    * See: [mil2525d-lines-areas-labels-base-template.stylx](https://github.com/ArcGIS/military-features-pro-data/blob/master/data/core_data/stylxfiles/mil2525d-lines-areas-labels-base-template.stylx)
* -with-
* A version that is automatically generated from [marker symbols and meta-data-tagged source data](https://github.com/Esri/joint-military-symbology-xml)
    * For more information on this process/version, see :
        * [joint-military-symbology-xml repo](https://github.com/Esri/joint-military-symbology-xml)
        * [style-file-utilities](../style-file-utilities)

## Requirements

* 2 Versions of the .stylx files:
    * A Pro/Runtime stylx file with all of the 2525D Point Icons called `mil2525d-points-only.dat`
        * This would have been created [by following the steps/readme here](https://github.com/ArcGIS/military-features-pro-data/tree/master/source/utilities)
    *  A version of the .stylx that is edited and maintained in ArcGIS Pro: [mil2525d-lines-areas-labels-base-template.stylx](https://github.com/ArcGIS/military-features-pro-data/blob/master/data/core_data/stylxfiles/mil2525d-lines-areas-labels-base-template.stylx)
        *  This version has Label Placement definitions, Point, Line, Polygon, and Text Symbols that are manually created and maintained in ArcGIS Pro
*  A Sqlite Editor/Interpreter  capable of executing SQL commands
    *  These steps have been tested using [sqlite3.exe](http://www.sqlite.org/download.html) but you may use the editor of your choice
* An Style Icon specification file: [Military-All-Icons.csv](https://github.com/Esri/joint-military-symbology-xml/blob/master/samples/imagefile_name_category_tags/Military-All-Icons.csv) with the following required fields: `styleItemCategory, styleItemName, styleItemUniqueId, styleItemTags`

## Instructions 

* Copy required files mentioned in Requirements above:
    * `mil2525d-points-only.dat` 
    * `mil2525d-lines-areas-labels-base-template.stylx` 
    * `Military-All-Icons.csv`
    * to this folder (`military-features-pro-data\source\utilities\merge-stylx-utilities`)
* Rename  `mil2525d-lines-areas-labels-base-template.stylx` to `mil2525d.stylx`
* Launch the Sqlite editor/interpreter of your choice
* Follow the SQL commands/steps in [SqliteMergeStylx.sql](./SqliteMergeStylx.sql)
    * Optionally you may edit this file to update the setting/string `{Full Path To}` to the local path and run this as an SQL script
* Exit the Sqlite Editor
* Test and update the resulting `mil2525d.stylx` in this repository


