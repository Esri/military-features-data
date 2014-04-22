Military Features Data Usage, Tips, and Tricks for MIL-STD-2525C
=========================================

# Purpose 

The purpose of this document is to quickly and easily capture and clarify any unique usage issues that users of this data have encountered. Also to provide some supplemental information to the existing help documentation for Military Features and its associated data stored in this repo. 

For a starting point, see the [ArcGIS Military Features Help Documentation](http://resources.arcgis.com/en/help/main/10.1/index.html#//000n0000000p000000)


# Sections

* [Using the Symbol Dictionary](#using-the-symbol-dictionary)
* [Using the Message Type Files](#using-the-message-type-files)

# Using the Symbol Dictionary

The symbol dictionary file is generated directly from Military Features feature classes. Some tips for using the symbol dictionary file are included in this section. The symbol dictionary file is [here](./dictionary).

## Viewing the Symbol Dictionary File

You may view the symbol dictionary file with any SQLite viewer. *Note: some viewers have difficulties showing multi-line text data such as the label drawing rules so you should pick a viewer that supports this if you need to view the label rules.*

## Modifier (Label) fields

The symbol dictionary allows users to set various modifier fields to add labels around a symbol. For example, developers using ArcGIS Runtime's [MessageProcessor](https://developers.arcgis.com/java/api-reference/com/esri/core/symbol/advanced/MessageProcessor.html) can call [Message.setProperty(String, String)](https://developers.arcgis.com/java/api-reference/com/esri/core/symbol/advanced/Message.html#setProperty(java.lang.String, java.lang.Object)) to set labels for the symbol.

The following modifier fields are supported in [the current symbol dictionary](./dictionary/mil2525c.dat). Use the string in the first column when calling Message.setProperty. Important Note: where available, you should set the fields specified by the constant fields in the [MessageHelper class](https://developers.arcgis.com/java/api-reference/com/esri/core/symbol/advanced/MessageHelper.html) or let MessageHelper set them for you; those fields are not part of MIL-STD-2525C but are required by the MessageProcessor.

To view the Label Rules and associations, view the symbol dictionary table "Label Rules." The following table lists the Modifier definitions from 2525C and their corresponding attributes in the symbol dictionary.


| Property Name | 2525C Field ID | 2525C Field Title | Notes |
| ------------- | -------------- | ----------------- | ----- |
| additionalinfo2 | H | Additional Information | Used in the Convoys and Airspace Coordination Area tactical graphics. |
| AdditionalInformation | H | Additional Information | |
| additionalinformation | H | Additional Information | |
| combateffectiveness | K | Combat Effectiveness | |
| commonidentifier | AF | Common Identifier | |
| credibility | J | Evaluation Rating | Credibility rating is second character of Evaluation Rating (J) field. |
| datetimeexpired | W | Date-Time Group (DTG) | Second half of Date-Time Group (DTG) (W) field. |
| datetimevalid | W | Date-Time Group (DTG) | First half of Date-Time Group (DTG) (W) field. |
| distance | AM | Distance | |
| equipmentteardowntime | AE | Equipment Teardown Time | |
| higherformation | M | Higher Formation | |
| iff_sif | P | IFF/SIF | |
| platformtype | AD | Platform Type | |
| quantity | C | Quantity | |
| reinforced | F | Reinforced or Reduced | |
| reliability | J | Evaluation Rating | Reliability rating is first character of Evaluation Rating (J) field. |
| sigintmobility | R2 | SIGINT Mobility Indicator | |
| signatureequipment | L | Signature Equipment | |
| speed | Z | Speed | |
| staffcomment | G | Staff Comments | |
| type | V | Type | |
| uniquedesignation | T | Unique Designation | |
| uniquedesignation2 | T | Unique Designation | Used as an additional field for Fire Support Lines tactical graphics. |
| x | Y | Location | Longitude in degrees. |
| y | Y | Location | Latitude in degrees. |
| z | X | Altitude/Depth | |
| zmax | X | Altitude/Depth | Maximum altitude for aviation tactical graphics. |
| zmin | X | Altitude/Depth | Minimum altitude for aviation tactical graphics. |


## Line Area Point Order Exceptions

The symbols that have unique point ordering and/or transformation rules defined in 2525C are captured in the symbol dictionary table "LnAExceptions." Use this table to determine at a glance which symbols have these special drawing rules. If you do not require these rules/transformations, you may be be able to edit/modify this table to meet your specific needs. More information on these exception lines/areas can be found [documented here](http://resources.arcgis.com/en/help/main/10.1/index.html#/Creating_features_using_the_geometry_in_a_standard_message/000n0000006v000000/)

# Using the Message Type Files

The files in the [messagetypes](./messagetypes) folder are the configuration files used by the Runtime Message Processor to process messages. You may modify these files to meet your particular needs. For example, you can increase the value of "symbolScaleFactor" to enlarge the symbols on the map. 

    * [PositionReport.json](./messagetypes/PositionReport.json)
        * "symbolScaleFactor" : 1.0  ==> "symbolScaleFactor" : 2.0 (doubles the size)
