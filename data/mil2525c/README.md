Using the MIL-STD-2525C Symbol Dictionary
=========================================

Modifier fields
---------------

The symbol dictionary allows users to set various modifier fields to add labels around a symbol. For example, developers using ArcGIS Runtime's [MessageProcessor](https://developers.arcgis.com/java/api-reference/com/esri/core/symbol/advanced/MessageProcessor.html) can call [Message.setProperty(String, String)](https://developers.arcgis.com/java/api-reference/com/esri/core/symbol/advanced/Message.html#setProperty(java.lang.String, java.lang.Object)) to set labels for the symbol.

The following modifier fields are supported in [the current symbol dictionary](https://github.com/Esri/military-features-data/blob/master/data/mil2525c/dictionary/mil2525c.dat). Use the string in the first column when calling Message.setProperty. (Note that you should also set some of the fields specified by the constant fields in the [MessageHelper class](https://developers.arcgis.com/java/api-reference/com/esri/core/symbol/advanced/MessageHelper.html) or let MessageHelper set them for you; those fields are not part of MIL-STD-2525C but are required by the MessageProcessor.)

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