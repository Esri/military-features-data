## stylxfiles
==========================

ArcGIS Professional and Runtime Stylx/Dictionary Files

## Purpose

This folder contains - the .stylx/.dat file to be used with ArcGIS Professional and Runtime.

## Requirements

* ArcGIS Professional
* ArcGIS Runtime SDK (10.2.4+)

## General Instructions 

To use the [mil2525c.stylx](./mil2525c.stylx) from this repo:

* In Pro, Add Style to project
* In Runtime, you must 
    * Rename the file extension from `.stylx` to `.dat`
    * Place the file `mil2525c.dat` in the folder: `{Runtime Install}\resources\symbols\mil2525c`
    * Note: this file is already released with the ArcGIS Runtime SDKs (except Android) - so the above steps would not normally be required
