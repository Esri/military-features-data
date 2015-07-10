# military-features-data / data / mil2525d / utilities / military-features-utilities
==========================

## Purpose

* These utilities are used to create military feature classes, domains, and other feature and test data 
* Individual utilities and steps:
    * Update Military Features Domains - updates the geodatabase domains from source CSV(.csv) files

## Sections

* [Requirements](#requirements)
* [Instructions](#instructions)

## Requirements

* ArcGIS Professional 1.0+ (Toolbox requires ArcGIS Pro to open/use)

## Instructions

### Update Military Features Domains

#### Overview

This utility updates the Geodatabase(GDB) domains of the [Military Features template database](../../core_data/gdbs) with the latest source data/values obtained from the [Joint Military Symbology](https://github.com/Esri/joint-military-symbology-xml) repository. This utility should be run periodically as the Joint Military Symbology repository is refined, improved, and updated.

The source data for this utility is a set of CSV files created in the Joint Military Symbology [`name_domains_values folder`](https://github.com/Esri/joint-military-symbology-xml/tree/master/samples/name_domains_values). The domain name is obtained from the CSV file name (with the "Coded_Domain" part removed) and the domain codes and description are obtained from the file contents.

A Geoprocessing (GP) Tool is then run on the source data to add or replace the GDB domains using the source data.

As a final (optional) validation step, once the domain data is imported into the GDB, the domains are then exported and compared to the original source data.

#### Steps

Importing the domain data:

* Obtain the latest set of Military Features source data and utilities
    * Clone/download this repository to your local machine.
* Obtain the latest set of `name_domains_values` source CSV files
    * Clone/download the  [Joint Military Symbology](https://github.com/Esri/joint-military-symbology-xml) repository.
    * You may also just download the files from the [`name_domains_values folder`](https://github.com/Esri/joint-military-symbology-xml/tree/master/samples/name_domains_values) in this repository
    * Note: although there are addition sample CSV files in this folder (ex. `*_Sample.csv`), the tools ignore these files using a file filter
* **REQUIRED WORKAROUND**
    * There is currently an issue with one of the [source csv files - Coded_Domain_Land_Unit_Entities.csv](https://github.com/Esri/joint-military-symbology-xml/blob/master/samples/name_domains_values/Coded_Domain_Land_Unit_Entities.csv#L193) - there are some **string** entries in this file (containing "XXXX"), but the imported domain codes are expected to be integers
    * These entries, currently the last four lines of Coded_Domain_Land_Unit_Entities.csv, will need to be manually edited/deleted in NotePad or other **text editor** (you should not use MS Excel because this may alter the format of the csv and data)
    * See https://github.com/Esri/joint-military-symbology-xml/issues/201 for the full description of this issue and workaround
* Run ArcGIS Pro
* Navigate to the local location of the  [update-domain-toolbox GeoProcessing Toolbox](./update-domain-toolbox)
    * The toolbox should look similar to this
![Image of Update Domain Toolbox](./screenshots/Toolbox.JPG)
* Run the *Import or Replace All Domains (Military Features)* GP Tool
    * As the `Input Folder` select the `joint-military-symbology-xml/tree/master/samples/name_domains_values` folder
    * As the `Target Geodatabase` select the desired Military Features template geodatabase (usually the one [obtained from here](../../core_data/gdbs)) 
        * IMPORTANT: The *Table To Domain* operation requires an exclusive schema lock on the geodatabase - therefore:
        * You should **not** have this geodatabase open elsewhere (for example, added to the current map) while performing this operation.
        * You must have full editing privileges (Update, Delete, etc.) to any feature class using this domain (mainly an issue if using SDE)
    * The GP Tool parameters will look similar to the following
![Image of Import Domains](./screenshots/ScreenShot.JPG)
* When the tool runs successfully, open the geodatabase in design mode and verify that the domains have been updated with the new source data

Verifying the domain data updates (*Recommended/Optional*):

* After running the *Importing the domain data* steps above
* Run ArcGIS Pro
* Navigate to the local location of the  [update-domain-toolbox GeoProcessing Toolbox](./update-domain-toolbox)
* Run the *Export GDB Domains to Folder* GP Tool
    * As the `Input Workspace` select the Military Features template geodatabase updated while performing the *Importing the domain data* steps above
    * As the `Output Folder` select an empty folder
    * This tool will export all Geodatabase domains to this folder
![Image of Export Domains](./screenshots/ScreenShot2.JPG)
* When the tool runs successfully, open the `Output Folder` and verify that the folder contains one CSV file for each domain stored in the `Input Workspace`
* Using a Diff Utility (such as [WinMerge](http://winmerge.org/)) compare the folder of exported domains to the folder containing the original/source set of `name_domains_values` source CSV files to verify that the exported CSVs match the inported CSVs
    * Note: a Diff Utility may notice some slight differences, for example
        * Domains included in the Geodatabase that are not included in the source data
        * Leading zeroes in the source data imports that are not reflected in the export, for example
 
 ![Image of Domains Diff](./screenshots/DomainDiff.JPG)
