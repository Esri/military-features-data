Military Features Data Usage, Tips, and Tricks for APP-6(B)
=========================================

#Purpose 

The purpose of this document is to quickly and easily capture and clarify any unique usage issues that users of this data have encountered. Also to provide some supplemental information to the existing help documentation for Military Features and its associated data stored in this repo. 

For a starting point, see the [ArcGIS Military Features Help Documentation](http://resources.arcgis.com/en/help/main/10.1/index.html#//000n0000000p000000)

IMPORTANT NOTE: Most of this information is common to 2525C and APP6. Please first view the [Usage, Tips, & Tricks information in the 2525C folder](../mil2525c). Only the information unique to APP6 will be listed in this file.

# Sections

* [Using the Symbol Dictionary](#using-the-symbol-dictionary)

# Using the Symbol Dictionary

The symbol dictionary file is generated directly from the APP6 Military Features feature classes. Some tips for using the symbol dictionary file, that are unique to APP6, are included in this section. The symbol dictionary file is [here](./dictionary).

## Line Area Point Order Exceptions

For general information on these see the [mil2525c document](../mil2525c/README.md#line-area-point-order-exceptions). Although these exceptions have not been explicitly defined in APP6, the implementers chose to follow this convention for APP6 at the advice of the symbology committee. As stated in the 2525C topic, if you do not require these rules/transformations, you may be be able to edit/modify the symbol dictionary "LnAExceptions" table to meet your specific needs. 
