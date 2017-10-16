# military-overlay

* Military Symbology Styles is a solution for creating style files used by the Dictionary Renderer to render Military Symbols.

## Features

* A solution using a style file with a complex attribute renderer (the Dictionary Renderer) included with ArcGIS Pro, Runtime, and Server
* The stand-alone style files used by the Dictionary Renderer included with ArcGIS Pro, Runtime, and Server to render Military Symbols
* Data included with the [Military Symbology Styles solution](http://solutions.arcgis.com/defense/help/military-symbology-styles/)
    
## Requirements

* ArcGIS Pro 1.4+ 
* ArcGIS Runtime 100.0+
* ArcGIS Server 10.5+

## Using

* For more information see the [Military Symbology Styles solution](http://solutions.arcgis.com/defense/help/military-symbology-styles/).

Includes:

* Styles for each version of the standard
    * [mil2525d](./mil2525d)
    * [mil2525c_b2](./mil2525c_b2) - a style used to draw both C and B Change 2 versions of the standard
* [Utilities and source data for creating the Military Symbology Styles](./utils-and-source-data)

## Viewing the Style Files

You may view the style file with any SQLite viewer. *Note: some viewers have difficulties showing multi-line text data such as the label drawing rules so you should pick a viewer that supports this if you need to view the label rules.*

## Editing the Style Files

While in general we strongly discourage editing the style file, we recognize that special needs and requirements may arise. Please be advised that these edits aren't supported and that use or modification of this data remains governed by the Apache V2 license. 

Example: Suppose you would like to replace the `Friendly Blue` icons with `Lime Green` ones, you might run an update query: 
`UPDATE SymbolInfo SET Json = REPLACE(Json, '128,224,255,255', '224,255,128,255') WHERE (Json LIKE '%128,224,255,255%')`

## Line Area Point Order Exceptions

The control measure line/area symbols that have unique point ordering and/or transformation rules defined in 2525C are captured in the symbol dictionary table "LnAExceptions." Use this table to determine at a glance which symbols have these special drawing rules. If you do not require these rules/transformations, you may be able to edit/modify this table to meet your specific needs. More information on these exception lines/areas can be found [documented here](http://resources.arcgis.com/en/help/main/10.1/index.html#/Creating_features_using_the_geometry_in_a_standard_message/000n0000006v000000/).

If you view the table "LnAExceptions" ([see above](#viewing-the-symbol-dictionary-file)) you will see 2 particular columns of interest "Significant8Chars" and "GCT." "Significant8Chars" defines the middle (characters 3-10) part of the symbol ID that is significant for control measure lines/areas. These are the defined exception rules/enumerations in the "GCT" column: `GCT_INDETERMINATE, GCT_ARROW, GCT_ARROWWITHOFFSET, GCT_ARROWWITHTAIL, GCT_CIRCLE, GCT_CIRCULAR, GCT_FREEHANDARROW, GCT_FREEHANDLINE, GCT_FREEHANDREVERSEARROW, GCT_FREEHANDU, GCT_HOOK, GCT_HORNS, GCT_OPENTRIANGLE, GCT_PARALLELLINES, GCT_PARALLELLINESMIDLINE, GCT_PARALLELLINESWITHTICKS, GCT_RECTANGULAR, GCT_RECTANGULAR1PT, GCT_T, GCT_TRIPLEARROW, GCT_TWOLINE, GCT_TWOLINE3OR4PT, GCT_UORTSHAPE`  There are 3 additional enumerations: `GCT_POINT, GCT_POLYLINE, GCT_POLYGON` that indicate no transformation is required. For more infomation on these rules consult the particular symbol in the standard. There is also a [python class here](https://github.com/Esri/military-feature-toolbox/blob/master/toolboxes/scripts/GeometryConverter.py) that implements these transformations if you prefer to see them in code. 

To modify this table (after you re-consult the [warning here](#editing-the-style-files)), in a SQLite viewer/editor of your choice, edit either the "Significant8Chars" column to change the symbol ID, the "GCT" column to change the transformation rule (for instance to "GCT_POLYLINE" to disable a special transformation), or remove the row(s) altogether (ex: `delete from LnAExceptions where 1` = delete all rows, but doesn't remove the table). 


