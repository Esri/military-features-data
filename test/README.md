# military-features-data Test Applications

These test applications are used to verify the Military Features Data repository. 

## Features

* Uses the ArcGIS Runtime to export an image of a symbol using the name or symbol identification code (SIDC) 

## Requirements

* ArcGIS Runtime SDK for Java - 10.1.1 (or later) 
* Java Development Environment
* Apache Ant - used to compile and run source

## Instructions

* In general, to build and run one of the test applications:
    * Open Command Prompt>
    * > cd military-features-data\test\ {Project/Application}
    * To Build: > ant
    * To Run with default options: > ant run
    * To Run with command line options: 
        * > java -classpath dist -jar dist/ExportSymbol.jar "Name or SIDC"
* Some applications have additional install/run dependencies
    * See the Readme provided with the project for any specific configuration instructions

## Licensing

Copyright 2013 Esri

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

A copy of the license is available in the repository's license.txt file.

[](Esri Tags: ArcGIS Defense and Intelligence Military Feature Military Features 2525 APP6)
[](Esri Language: Java)
