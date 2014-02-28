# military-features-data TestExportSymbol

This test application uses ArcGIS Runtime to export a single dictionary symbol. It is used as a simple test of Military Features and Runtime. 

## Features

* Uses the ArcGIS Runtime to export an image of a symbol using the name or symbol identification code (SIDC) 

## Requirements

* ArcGIS Runtime SDK for Java - 10.1.1 (or later) 
* Java Development Environment
* Apache Ant - used to compile and run source

## Instructions

* To build and run:
    * Open Command Prompt>
    * > cd military-features-data\test\TestExportSymbol
    * To Build: > ant
    * To Run, run from the command line supplying the parameters (or "help"): 
        * This example export all area symbols
        * >java -classpath dist -jar dist/ExportSymbol.jar {SIDC or Name}

## License

Copyright 2013-2014 Esri

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.