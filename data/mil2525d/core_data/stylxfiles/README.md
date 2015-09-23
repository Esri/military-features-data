# military-features-data / data / mil2525d / core_data / stylxfiles
==========================

# stylxfiles

This folder contains the .stylx files for using and editing MIL-STD-2525D Military Feature symbols in ArcGIS. "Stylx" files were formally called "symbol dictionary" files in previous product versions so you may see that term used interchangeably also.

This folder contains:

* [mil2525d.stylx](./mil2525d.stylx) - The complete/final MIL-STD-2525D (Delta) .stylx file to be used with ArcGIS Pro and Runtime.
    * This file is included with the Dictionary Renderer/CIM Rule Engine Plugins used by ArcGIS Pro and Runtime.
    * This file is generated from source SVGs and icon specification files from the [joint-military-symbology-xml repo](https://github.com/Esri/joint-military-symbology-xml) (see that repo from more information)
    * This auto-generated file is then merged with the .stylx below that is edited/maintained in Pro
    * **This version is intended for end-users**
* [mil2525d-lines-areas-labels-base-template.stylx](./mil2525d-lines-areas-labels-base-template.stylx) - A separate version of this .stylx that has only the Label Placement Definitions, Point, Line, Polygon, and Text Symbols that are manually created and maintained in ArcGIS Pro
    * **This version is intended only for use by the Product Engineers or maintainers of this repo/data**

## Additional Information

* [Usage](#usage)
* [Version Information](#version-information)
* [Note on Downloading Individual Files](#note-on-downloading-individual-files)

## Usage 

To use these files:

[mil2525d.stylx](./mil2525d.stylx)

* In Pro
    * If simply viewing the style, select "Add Style to project"
        * **IMPORTANT: THIS FILE SHOULD NEVER BE MANUALLY UPDATED/EDITED IN PRO, IF UPDATING ANYTHING, USE THE OTHER INTENDED VERSION BELOW**
	    * This version of the file is [created using this creation/conversion process](../../utilities/style-utilities)
    * If using with the Dictionary Renderer, you will need to update the resource:
        * Copy [mil2525d.stylx](./mil2525d.stylx) to {Pro Install Location}\ArcGIS\Pro\Resources\Dictionaries\mil2525d	
* In Runtime 
    * Place the file `mil2525d.stylx` in the folder: `{Runtime Install}\resources\symbols\mil2525d`
    * Requires Versions 10.2.4 or later
    * Version 10.2.4 only - you must 
        * Rename the file extension from `.stylx` to `.dat`
        * Place the file `mil2525d.dat` in the folder: `{Runtime Install}\resources\symbols\mil2525d`

[mil2525d-lines-areas-labels-base-template.stylx](./mil2525d-lines-areas-labels-base-template.stylx)

* In Pro, Add Style to project
* You may edit/correct the symbols and associated meta data in this file in Pro, update this repo with this file as needed (**for repo Product Engineers or others making edits in Pro only**)
* Before you edit this file in Pro, please read the [Runtime Compatibility Warning section of the style-utilities README.md](../../utilities/style-utilities/README.md#important-warning-on-runtime-compatibility-before-you-begin)
* Once you have completed making any desired edits/changes, you will need to merge this file using the steps/process outlined in [merge-stylx-utilities](../../utilities/style-utilities/merge-stylx-utilities)

Additional Usage Notes:

* When editing these files, ensure that the CIM version used by ArcGIS Pro is compatible with the product you are targeting.
* These files are intended to be viewed and edited in ArcGIS Pro but may be viewed with a SQLite viewer also. 

## Version Information](#version-information)

The following version data is captured in the stylx "meta" table.

|Key|Current value/format/example|Description/Usage|Application Used By|
|---|---|---|---|
|version|1.0|Indicates the database schema for stylx files. |Pro|
|content|json|Indicates the content type of stylx. |Pro|
|cim_version|1.1.0|Indicates the version of CIM specification that is used/supported.|Pro/Runtime|
|forceRGB|true|CIM format option. This must be true to be compatible with ArcGIS Runtime.|Pro|
|vectorizeCM|true|CIM format option. This must be true to be compatible with ArcGIS Runtime.|Pro|
|densifyCurves|true|CIM format option. This must be true to be compatible with ArcGIS Runtime.|Pro|
|runtime_version|1.0.0|Indicates the version of the CIM Plugin API. |CIM Rule Engine Plugin Manager|
|rule_engine_plugin_version|1.0.0|Indicates the version of the CIM Rule Engine plugin dll. Note: this should match the "version" in the manifest/metadata file.|CIM Rule Engine Plugin Manager|
|pro_version|1.1.0|mil2525d plugin administrative entry. Version of ArcGIS Pro.|mil2525d plugin|
|milstylx_version|1.1.0|mil2525d plugin administrative entry. Version of the mil2525.stylx.|mil2525d plugin|
|milstd_version|2525D-0.1|mil2525d plugin administrative entry. Version of MIL-STD-2525.|mil2525d plugin|
|jmsml_version|1.0.1|mil2525d plugin administrative entry. Version of the JMSML used|mil2525d plugin| 

## Note on Downloading Individual Files

**IMPORTANT/WARNING** If you are downloading a single (binary) file in Github - you can **not** (right) click the link and select "Save Link As..." - this download will appear to work but result in a corrupted file. To download a single file, use Github's "View Raw" link/page and then the file should download correctly.
