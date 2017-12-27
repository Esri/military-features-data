### Tools for creating Pro Layer Packages (lpkx)

These steps/tools are used to create a Schema-Only layer package for Military Overlay. 

A layer package is a file that includes the layer properties (ex. drawing settings, renderer, etc.) and data. A schema-only layer package has an empty geodatabase (no data entries).

To create the layer packages:

1. Download the latest version of the Military Overlay solution: http://solutions.arcgis.com/defense/help/military-overlay/
2. Create a Pro project, this will have 2 maps, one for each standard
3. For each map/standard share the Military Overlay layer as a schema-only layer package after performing the workaround below.

**Spatial Index Layer Packaging Workaround**

There is currently a workaround to address an error "Spatial Index Invalid" (See: https://github.com/Esri/military-features-data/issues/287 )

Run the following tools from the LayerPackageUtilities toolbox on each geodatabase being packaged. These tools/models should be run immediately before packaging the (schema only) Military Overlay Layer Packages in ArcGIS Pro:

1. Delete All Features From Workspace
2. Remove Spatial Index 
    a. This is a model that will add a blank spatial index to a layer package
