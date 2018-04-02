# style-creation-utilities

## Purpose

* These utilities/steps are used to: 
    * Convert a set of (1) Scalable Vector Graphics (SVG/.svg) files and (2) Comma-Delimited/Separated Values (CSV/.csv) specification files into ArcGIS Pro/Runtime Mobile Style File (.stylx) **Point Symbols** 
    * Merge the **Point Symbols** Mobile Style generated (in step above) with a version of the .stylx that is manually created/maintained that contains Lines, Areas, and Labels
    * Validate that the .stylx has been created correctly and can be used in ArcGIS Pro and Runtime

## Requirements

* This repo has been cloned to your local machine
* A local set of Military Feature source .svg images in the expected file/folder format
    * These files are [obtained from the repo here](../svg)  
* ArcGIS Pro 
    * Used to create and verify the Pro Mobile Style (.stylx) file(s)
    * IMPORTANT: the version of Pro should match the version of the Cartographic Information Model (CIM) that you wish to target. Ex: Pro 2.0 targets CIM 2.0
* Access to Military Style creation Pro Addin available at:
    * https://esri.box.com/s/s042ujvrzrhtx7kufz6o8srxcweibeaq 

## General Instructions 

* Download and install the Pro Addin available at:
    * Download from: https://esri.box.com/s/s042ujvrzrhtx7kufz6o8srxcweibeaq 
    * Install the `GenerateSVGStyle.esriAddinX` Addin from: `GenerateSVGStyle\bin\Debug`
* Run Pro and launch the `Generate SVG Style` Addin from the Addin Tab
* Perform the next steps from the `Generate SVG Style` Addin Form
* On the `Generate Style from CSV + SVGs` tab
    * Create a Pro Mobile Style for the **Point** icons
    * Set the following options:
    * **SVG Root Folder**
        * This is the folder that contains the SVG files to convert to style icons
        * The folder with these svg files should be [here](../svg)
        * The local location of the `military-features-data/military-symbology-styles/utils-and-source-data/svg` folder:
    * **Icon Specification CSV File**
        * This is the file that contains the required style metadata for each SVG file
        * Choose the Icon Specification file that matches the desired standard.
        * For example, for mil2525d, this file should be [here](../style-source-files/mil2525d/imagefile_name_category_tags)
        * The local location of this file would be:  `military-features-data/military-symbology-styles/utils-and-source-data/style-source-files/mil2525d/imagefile_name_category_tags`   
    * **Style Output Folder**
        * The destination folder to place the created style
    * **Style Output Filename**
        * The output style filename
    * Click `Generate Style` to create the style for the **Point Symbols**
        * Note: this operation may take several minutes to complete
* Next, on the `Merge Styles` tab
    * Merge the **Point Symbols** icons Mobile Style generated (in step above) with a version of the .stylx that is manually created/maintained that contains Lines, Areas, and Labels
    * Set the following options:
    * **Style to Merge**
        * **Point Symbols** icons Mobile Style
    * **Style to Merge Into**
        * **Lines, Areas, and Labels** Mobile Style maintained in this repo
    * **Merged Style Name**
        * The output style filename of the merged style
    * Click `Merge Styles` to merge the two styles
        * Note: this operation may take several minutes to complete
* Finally, on the `Add Label Rules` tab
    * Add label rules to the icons in the Mobile Style generated (in step above)
    * Set the following options:
    * **Style**
        * Style to add label rules 
    * Click `Execute Script` to add label rules 
* Verify/validate the Mobile Style created in ArcGIS Pro 
    * This file should now contain points, lines, polygon, text, and label placement symbols
    * View | Project View | Styles | {style name} 
    * Change the `Show:` Pull Down to change between the Point Symbol, Line Symbol, etc. types
    * Verify symbols and associated meta data in Pro
    * If possible, also test the style using the Pro and Runtime Dictionary Renderer 
* Finally, update the Mobile Style file in this repo 
