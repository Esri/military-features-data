# military-features-data

An ArcGIS for Defense repository for managing data and tools used in ArcGIS Military Symbology solutions. These solutions are used in sketching, planning, monitoring, and publishing Military Symbology using the ArcGIS platform. This data is used to create ArcGIS solutions that require Military Symbology matching detailed specifications such as MIL-STD-2525.

![Image of Military Features Data](ScreenShot.png)

## Sections

* [Features](#features)
* [Requirements](#requirements)
* [Instructions](#instructions)
* [Resources](#resources)
    * [ArcGIS Military Symbology Solutions](#supported-solutions-using-this-data)
* [Issues](#issues)
* [New to Github?](#new-to-github)
* [Contributing](#contributing)
* [Acknowledgments](#acknowledgments)
* [Licensing](#licensing)

## Features

* Solutions targeting Military Symbology in ArcGIS users:
    * Planners and Analysts who sketch complex overlays and plans
    * Operators who visualize real time situational awareness information
    * Users of systems which exchange military overlays with other battlefield systems 
* These solutions include:
    * [Military Features](#military-features)
    * [Military Overlay](#military-overlay)
    * [Military Symbology Styles](#military-symbology-styles)
* The source data and tools used to create ArcGIS Military Symbology data and solutions
* Support for MIL-STD-2525 Military Symbology:
    * MIL-STD-2525D (June, 2014)
    * MIL-STD-2525C (November, 2008)
    * MIL-STD-2525B Change 2 (March, 2007)

## Requirements

Each solution has the following ArcGIS requirements:

* Military Features
    * ArcGIS Desktop 10.3.1+
* Military Overlay
    * ArcGIS Pro 1.4+ 
    * ArcGIS Server 10.5+
* Military Symbology Styles `*`
    * ArcGIS Pro 1.4+ 
    * ArcGIS Runtime 100.0+ 
    * ArcGIS Server 10.5+
    * `*` Note: these styles are installed with these products and maintained here

## Instructions

**IMPORTANT NOTE:** If you just want to download the supported solution, please download the solution directly from the [ArcGIS Military Symbology Solutions site](#supported-solutions-using-this-data). This repo is primarily for editing, recreating, or logging issues with this data.

* Clone/download the repository.
* Determine the Military Symbology solution [(see below)](#arcgis-military-symbology-solutions).
* Edit and/or use the solution in the desired product.
-or-
* Use the utilities provided in this repo to recreate derived data from source data.

### ArcGIS Military Symbology Solutions

#### Military Features

* [Military Features](./military-features)
    * A solution using the ArcGIS 10 standard symbology model
    * A datamodel for drawing and labeling symbols using the ArcGIS Cartographic Renderer and Maplex Labeling Engine 
    * Targets ArcGIS 10 (ArcMap and ArcGIS Engine) and legacy applications/users
    * Data included with the [Military Features solution](http://solutions.arcgis.com/defense/help/military-features/)
    
#### Military Overlay

* [Military Overlay](./military-overlay)
    * A solution using a complex attribute renderer (the Dictionary Renderer) included with ArcGIS Pro, Runtime, and Server
    * A datamodel for drawing and labeling symbols using the Dictionary Renderer
    * Data included with the [Military Overlay solution](http://solutions.arcgis.com/defense/help/military-overlay/)
        
#### Military Symbology Styles

* [Military Symbology Styles](./military-symbology-styles)
    * The stand-alone style files used by the Dictionary Renderer included with ArcGIS Pro, Runtime, and Server
    * Data included with the [Military Symbology Styles solution](http://solutions.arcgis.com/defense/help/military-symbology-styles/)

#### Military Standard Support

The following table lists the versions of the standard supported by each solution.

|Product||Standard||
|---|---|---|---|
||2525D|2525C|2525Bc2|
|Pro 1.4+/Server 10.5+|Military Overlay| Military Overlay `*` |Military Overlay|
|Runtime 100.0+|Military Symbology Styles|Military Symbology Styles|Military Symbology Styles|
|ArcGIS Desktop/Engine 10.3.1+||Military Features||MyMiscellanea

`*` Note The 2525C Military Overlay is not released with the solution download but is available in this repo
    
## Resources

* [ArcGIS for Defense Solutions Website](http://solutions.arcgis.com/defense)
* [ArcGIS for Defense Downloads](http://appsforms.esri.com/products/download/#ArcGIS_for_Defense)
* Learn more about Esri's Solutions [Focused Maps and Apps for Your Organization](http://solutions.arcgis.com/)
* The Official Military Standard Documents
    * [MIL-STD-2525D (June, 2014)](http://www.dtic.mil/doctrine/doctrine/other/ms_2525d.pdf)
    * [MIL-STD-2525C (November, 2008)](http://www.dtic.mil/doctrine/doctrine/other/ms_2525c.pdf)
    * MIL-STD-2525B Change 2 (March, 2007) 

### Supported Solutions Using this Data

Further documentation and supported downloads can be obtained from the solution page:

* [Military Features solution](http://solutions.arcgis.com/defense/help/military-features/)
* [Military Overlay solution](http://solutions.arcgis.com/defense/help/military-overlay/)
* [Military Symbology Styles solution](http://solutions.arcgis.com/defense/help/military-symbology-styles/)
    
## Issues

* Find a bug or want to request a new feature?  Please let us know by submitting an issue.

## New to Github

* [New to Github? Get started here.](https://github.com/Esri/esri.github.com/blob/master/help/esri-getting-to-know-github.html)

## Contributing

Esri welcomes contributions from anyone and everyone. Please see our [guidelines for contributing](./CONTRIBUTING.md).

## Acknowledgments 

This repository reuses software from:

1. [Joint Military Symbology XML](https://github.com/Esri/joint-military-symbology-xml/)
    1. SVG image files and derived data
    2. Governed by the [Apache License, Version 2.0](https://github.com/Esri/joint-military-symbology-xml/blob/master/license.txt)

## Licensing

Copyright 2013-2017 Esri

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

A copy of the license is available in the repository's
[license.txt](license.txt) file.
