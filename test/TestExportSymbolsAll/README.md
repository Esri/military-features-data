# military-features-data TestExportSymbolsAll

This test application uses ArcGIS Runtime and sqlite4java to export all dictionary symbols. It is used to verify the Military Features Data repository. 

## Features

* Uses the ArcGIS Runtime to export an image of a symbol using the name or symbol identification code (SIDC) 
* Exports all symbols to quickly verify a symbol set

## Requirements

* ArcGIS Runtime SDK for Java - 10.1.1 (or later) 
* Java Development Environment
* Apache Ant - used to compile and run source
* Google Code project: [sqlite4java](https://code.google.com/p/sqlite4java) 

## Instructions

* This project depends on the Google Code SQLite Java Wrapper project: [sqlite4java](https://code.google.com/p/sqlite4java) 
    * To install this dependency:
    * Download the install/zip for this project (this has been test with sqlite4java-282.zip )
    * Copy the jar and dll files for your platform to TestExportSymbolsAll\lib
    * For example, for windows, these files should be in folder TestExportSymbolsAll\lib 
        * sqlite4java-win32-x64.dll
        * sqlite4java-win32-x86.dll
        * sqlite4java.jar
* To build and run:
    * Open Command Prompt>
    * > cd military-features-data\test\TestExportSymbolsAll
    * To Build: > ant
    * To Run, run from the command line supplying the parameters (or "help"): 
        * This example export all area symbols
        * >java -classpath dist -jar dist/ExportSymbols.jar all area
    * NOTE: Some configurations may require that the location of the sqlite4java native binaries (.dll/.so) be added to the path, see the sqlite4java for more information
