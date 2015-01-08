# military-features-data / data / mil2525d / utilities / style-utilities

## Purpose

* These utilities are used to convert a set of (1) Scalable Vector Graphics (.svg) files and (2) Comma-Delimited (.csv) specification files into ArcGIS Desktop (.style) and Pro/Runtime (.stylx) style files 
* Individual utilities and steps: 
    * Creates a set of Enhanced Meta Files (.emf) vector image files from a source tree of Scalable Vector Graphics (.svg) files 
    * Imports a set of .emf vector image files into ArcGIS .style files
    * Creates a .stylx from the .style
    * Merges the automatically generated .stylx created (in steps above) with a version of the .stylx that is manually created/maintained

## Requirements

* This repo has been cloned to your local machine
* A set of Military Feature source .svg images in the expected file/folder format
    * These files are currently [obtained from here](https://github.com/Esri/joint-military-symbology-xml/tree/master/svg) (but note that this location is subject to change)
* ArcGIS Desktop 10.X (Standard)
    * Used to create and verify the 10.X .style file(s)
* ArcGIS Pro 
    * Used to create and verify the Pro .stylx file(s)
* See each of the readme's for the dependent steps/more information/some additional requirements
    * [image-conversion-utilities](./image-conversion-utilities)
    * [style-file-utilities](./style-file-utilities)
    * [merge-stylx-utilities](./merge-stylx-utilities)

## General Instructions 

* Obtain the latest set of Military Feature source .svg images in the expected file/folder format
    * These files may be [obtained from here](https://github.com/Esri/joint-military-symbology-xml/tree/master/svg)
    * Unzip the files from the set above to your local machine and note the location
    * `{Svg_Images_Home}` = ____________
* Convert the .svg files to .emf 
    * See  [image-conversion-utilities](./image-conversion-utilities) for the details of this process
    * Note the location of these converted images, `{Emf_Images_Home}` = ____________
* Convert the .emf files to .style files using the Style File Import Utility (csv2ArcGISStyle)
    * See [style-file-utilities](./style-file-utilities) for the details of this process
* Open ArcGIS Desktop (ArcMap) and verify/validate the output style files produced by checking these styles for errors
    * ArcMap | Customize | Style Manager (Add Style to List)
    * Close ArcMap
    * Once you have verified/validated the output style files, you may want to rename them
    * Note: names *without spaces* were used during the conversion process so you may wish to change these names at this time (if you updating the .styles in this repo)
* Set the ArcGIS Runtime Compatibility Flag in ArcGIS Pro
    * IMPORTANT: ArcGIS Pro and Runtime have some differences in how each application handles colors, fonts, and arcs/curves.
    * Therefore, **if you are creating a .stylx to be used in both platforms (and in this case you are), you must set a runtime compatibility flag in ArcGIS Pro**
    * To do this:
	* Using the .reg file:
	    * Use the .reg file [ForceRuntimeCompatibility.reg](./ForceRuntimeCompatibility.reg)
	* Manual :
        * Run `regedit` 
        * Navigate to `HKEY_CURRENT_USER\Software\ESRI\ArcGISPro1.0`
        * Add a new key `RTCStyles` as DWORD with value 1
    * Setting this key in Pro, does/forces the following:
        * Converts all colors to use RGB instead of the Pro color model
        * Vectorizes the fonts/character markers (Runtime does not install fonts)
        * Densifies curves (instead of using Bezier curves or circular arcs) 
* Create a Pro .stylx file for the "All Icons" style file
    * Rename the style file created that contains with all the icons `Military-2525Delta-All-Icons.style` to `mil2525d-points-only.style`
    * Open ArcGIS Pro and import the file `mil2525d-points-only.style` 
        * Project | Styles | Import Style - *(Note this operation may take several minutes to complete)*
    * Verify/validate that the file imported correctly and the icons look as expected
        * View | Project View | Styles | mil2525d-points-only 
        * *Note: it may take 30-60 seconds for the style symbols to initially show*
    * Close ArcGIS Pro and locate the newly-created `mil2525d-points-only.stylx` (Note: this converted/imported file now has a  **stylx** file extension)
* Merge  `mil2525d-points-only.stylx` (created above) with `mil2525d-lines-areas-labels-base-template.stylx` to create: `mil2525d.stylx`
    * See [merge-stylx-utilities](./merge-stylx-utilities) for the details of this process, including generally
    	* Copy the file created above:  `mil2525d-points-only.stylx`
    	* Copy (from this repo) the latest copy of [mil2525d-lines-areas-labels-base-template.stylx](../../core_data/stylxfiles)
    	* Merge the 2 files into `mil2525d.stylx`
*  Verify/validate the final/resulting `mil2525d.stylx`
    *  Verify/validate the final `mil2525d.stylx` file created in ArcGIS Pro 
        * This file should now contain points, lines, polygon, text, and label placement symbols
        * View | Project View | Styles | mil2525d 
        * Change the `Show:` Pull Down to change between the Point Symbol, Line Symbol, etc. types
        * *Note: it may take 30-60 seconds to switch between the types*
        * Verify symbols and associated meta data
* Finally, update [the .stylx file in this repo](../../core_data/stylxfiles)
 
