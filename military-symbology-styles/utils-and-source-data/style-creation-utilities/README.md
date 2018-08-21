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

## General Instructions 

* Consult the wiki for detailed instructions:

https://github.com/Esri/military-features-data/wiki/Creating-Dictionary-Renderer-Style-Files