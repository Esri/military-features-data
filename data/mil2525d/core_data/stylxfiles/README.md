# military-features-data / data / mil2525d / core_data / stylxfiles
==========================

## stylxfiles

**IMPORTANT/WARNING** If you are downloading a single (binary) file in Github - you can **not** (right) click the link and select "Save Link As..." - this download will appear to work but result in a corrupted file. To download a single file, use Github's "View Raw" link/page and then the file should download correctly.

ArcGIS Pro Stylx/Style Files

This folder contains:

* [mil2525d.stylx](./mil2525d.stylx) - The complete/final MIL-STD-2525D (Delta) .stylx file to be used with ArcGIS Pro and Runtime.
    * This file is generated from source SVGs and icon specification files from the [joint-military-symbology-xml repo](https://github.com/Esri/joint-military-symbology-xml) (see that repo from more information)
    * This auto-generated file is then merged with the .stylx below that is edited/maintained in Pro
    * **This version is intended for end-users**
* [mil2525d-lines-areas-labels-base-template.stylx](./mil2525d-lines-areas-labels-base-template.stylx) - A separate version of this .stylx that has only the Label Placement Definitions, Point, Line, Polygon, and Text Symbols that are manually created and maintained in ArcGIS Pro
	* **This version is intended only for use by the Product Engineers of this repo/data**

To use:

[mil2525d.stylx](./mil2525d.stylx)

* In Pro, Add Style to project
	* **IMPORTANT: THIS FILE SHOULD NEVER BE MANUALLY UPDATED/EDITED IN PRO, IF UPDATING ANYTHING, USE THE OTHER INTENDED VERSION BELOW**
* In Runtime 
    * Place the file `mil2525d.stylx` in the folder: `{Runtime Install}\resources\symbols\mil2525d`
    * Requires Versions 10.2.4 or later
    * Version 10.2.4 only - you must 
        * Rename the file extension from `.stylx` to `.dat`
        * Place the file `mil2525d.dat` in the folder: `{Runtime Install}\resources\symbols\mil2525d`

[mil2525d-lines-areas-labels-base-template.stylx](./mil2525d-lines-areas-labels-base-template.stylx)

* In Pro, Add Style to project
* You may edit/correct the symbols and associated meta data in this file in Pro, update this repo with this file as needed (**for repo Product Engineers or others making edits in Pro only**)
