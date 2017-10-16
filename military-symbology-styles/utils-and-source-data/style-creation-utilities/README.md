# style-creation-utilities

## Purpose

* These utilities are used to convert a set of (1) Scalable Vector Graphics (.svg) files and (2) Comma-Delimited (.csv) specification files into ArcGIS Desktop (.style) and Pro/Runtime (.stylx) style files 
* Individual utilities and steps: 
    * Creates a set of Enhanced Meta Files (.emf) vector image files from a source tree of Scalable Vector Graphics (.svg) files 
    * Imports a set of .emf vector image files into ArcGIS .style files
    * Creates a .stylx from the .style
    * Merges the automatically generated .stylx created (in steps above) with a version of the .stylx that is manually created/maintained
    * Validate that the .stylx has been created correctly and can be viewed in ArcGIS Pro

## Requirements

* This repo has been cloned to your local machine
* A local set of Military Feature source .svg images in the expected file/folder format
    * These files are [obtained from the repo here](../svg)  
* ArcGIS Desktop 10.X (Standard)
    * Used to create and verify the 10.X .style file(s)
* ArcGIS Pro 
    * Used to create and verify the Pro .stylx file(s)
* See each of the readme's for the dependent steps/more information/some additional requirements
    * [image-conversion-utilities](./image-conversion-utilities)
    * [style-file-utilities](./style-file-utilities)
    * [merge-stylx-utilities](./merge-stylx-utilities)

## Important Warning on Runtime Compatibility Before You Begin

**IMPORTANT:** Because the .stylx files in this repo are shared between ArcGIS Pro and Runtime, before you begin creating or editing these .stylx files, you should be aware of and familiar with a very important pre-requisite step. This important step is the setting of a .stylx "ArcGIS Runtime Compatibility Flag" in ArcGIS Pro. This flag disables some Pro symbol features that are currently incompatible with Runtime. Failure to set this flag will result in a .stylx file with symbols that fail to draw in ArcGIS Runtime.

* IMPORTANT: Set the ArcGIS Runtime Compatibility Flag in ArcGIS Pro before doing any .stylx editing operations (importing .style(s), editing .stylx(s), etc.)
    * Justification/Background: ArcGIS Pro and Runtime have some differences in how each application encodes/handles colors, fonts, and arcs/curves.
    * Therefore, **if you are creating a .stylx to be used in both platforms (and in this case you are), you must set a runtime compatibility flag in ArcGIS Pro**
    * To do this:
    * Using the .reg file:
        * Use the .reg file [ForceRuntimeCompatibility.reg](./ForceRuntimeCompatibility.reg)
        * Double click on this file to install this key
    * Manually installing this key -and verifying that this key is set (highly recommended):
        * Run `regedit` 
        * Navigate to `HKEY_CURRENT_USER\Software\ESRI\ArcGISPro` `*`
            * `*` IMPORTANT: verify that the current version of ArcGIS Pro settings are still installed at this location - this location could be subject to change.
        * Add a new key `RTCStyles` as DWORD with value 1
    * Setting this key in Pro, does/forces the following:
        * Converts all colors to use RGB instead of the Pro color model
        * Vectorizes the fonts/character markers (Runtime does not install fonts)
        * Densifies curves (instead of using Bezier curves or circular arcs) 
        * Examples of Json entries incompatible with Runtime include:
            * CIMRGBColor Objects - `"CIMRGBColor","values":[0,0,0,100]` - "CIMRGBColor" color object versus simplified RGBA: `"color":[0,0,0,255]` and last/alpha value of "100" (CIMRGBColor full opaque) vs. "255" (RGBA full opaque)
            * Curve objects - `"curveRings"`
            * Text strings (rather than line paths) - `"textString"`
         * To determine if your .stylx contains incompatible features, you can use SQLite to perform this query for incompatible content: `select * from ITEMS where (Content like '%CIMRGBColor%') or (Content like '%curveRings%') or (CONTENT LIKE '%textString%')` 
         * To determine if your .stylx contains NULL entries which may also impact compatibility with Runtime: `select * from ITEMS WHERE (CONTENT LIKE '%[]%')`
    * Be aware that setting this flag/key could affect the editing of non-ArcGIS Runtime .stylx(s), so if you intend to edit other Pro .stylx(s), you may wish to remove/disable this setting when done this process. 

## General Instructions 

* Verify that the [Runtime Compatibility Flag] has been set on the machine you intend to run ArcGIS Pro. See [the ArcGIS Runtime Compatibility Flag section above](#important-warning-on-runtime-compatibility-before-you-begin)
* Obtain the latest set of Military Feature source .svg images in the expected file/folder format
    * These files should be [here](../svg)
    * Note the local location of the `military-features-data/military-symbology-styles/utils-and-source-data/svg` folder:
    * `{Svg_Images_Home}` = C:\{local path}_______ 
        * Ex: `{Svg_Images_Home}` = `C:\Github\military-features-data\military-symbology-styles\utils-and-source-data\svg`
* Convert the .svg files to .emf 
    * See [image-conversion-utilities](./image-conversion-utilities) for the details of this process
    * Note the location of these converted images:
    * `{Emf_Images_Home}` = ____________
        * Ex: `{Emf_Images_Home}` = `C:\Github\military-features-data/military-symbology-styles/utils-and-source-data/emf`
* Convert/import the .emf files to .style files using the Style File Import Utility (csv2ArcGISStyle)
    * See [style-file-utilities](./style-file-utilities) for the details of this process
* Open ArcGIS Desktop (ArcMap) and verify/validate the output style files produced by checking these styles for errors
    * ArcMap | Customize | Style Manager (Add Style to List)
    * Close ArcMap
    * Once you have verified/validated the output style files, you may want to rename them
    * Note: names *without spaces* were used during the conversion process so you may wish to change these names at this time (if you updating the .styles in this repo)
* Create a Pro .stylx file for the "mil2525d-points-only.style"
    * Open ArcGIS Pro and import the file `mil2525d-points-only.style` 
        * Project | Styles | Import Style - *(Note this operation may take several minutes to complete)*
    * Verify/validate that the file imported correctly and the icons look as expected
        * View | Project View | Styles | mil2525d-points-only 
        * *Note: if scrolling through all of the symbols, it may take 30-60 seconds for all of the style symbols to initially show*
    * Close ArcGIS Pro and locate the newly-created `mil2525d-points-only.stylx` (Note: this converted/imported file now has a  **stylx** file extension)
* Merge `mil2525d-points-only.stylx` (created above) with `mil2525d-lines-areas-labels-base-template.stylx` to create: `mil2525d.stylx`
    * See [merge-stylx-utilities](./merge-stylx-utilities) for the details of this process, including generally
        * Copy the file created above: `mil2525d-points-only.stylx`
        * Copy (from this repo) the latest copy of [mil2525d-lines-areas-labels-base-template.stylx](../style-source-files)
        * Merge the 2 files into a single file: `mil2525d.stylx`
*  Verify/validate the final/resulting `mil2525d.stylx`
    *  Verify that there are no unmapped Symbol Keys
        * To verify that all symbol keys were created correctly by the merge process above, perform the following:
        * Use the SQLite editor of your choice to perform a query for missing/unmapped keys: 
        * `select * from ITEMS WHERE Key REGEXP '(^[A-Za-z]+)'`
        * Note: This query tests for keys that start with an alphabetic character - all of the imported mil2525d keys should start with a numeric character - this query should not return any rows from the mil2525d icon source data (it may return some alphabetic rows that existed in the style prior to the merge so you may want to also do this query before making changes to compare the before vs. after)
    *  Verify/validate the final `mil2525d.stylx` file created in ArcGIS Pro 
        * This file should now contain points, lines, polygon, text, and label placement symbols
        * View | Project View | Styles | mil2525d 
        * Change the `Show:` Pull Down to change between the Point Symbol, Line Symbol, etc. types
        * Verify symbols and associated meta data
* If creating a stylx for the legacy (2525C or 2525Bc2) standards, see: [legacy-stylx-utilities](./legacy-stylx-utilities)
* Finally, update [the mil2525d.stylx file in this repo](../../mil2525d)
 
