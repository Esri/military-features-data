# military-features-data / data / mil2525d / core_data / gdbs
=========================================

# Purpose 

This folder contains the Military Features Core Geodatabase Schema format and an empty geodatabase (in .zip format) conforming to the current Military Features schema. 

## Additional Schema Information

The following information is intended to succinctly capture the format of the Military Features Schema for other applications that may depend upon this format. 

## Schema Sections

* [Symbol Display Attributes](#symbol-display-attributes)
* [Modifier (Label) Attributes](#modifier-label-attributes)

## Symbol Display Attributes

The following fields control the display of symbols.

| Attribute Name | Data Type | Range of Values | Default Value/Meaning | Sample Name | Sample Value/Meaning | Explanatory Notes |
| -------------- | --------- | --------------- | ----------- | -------------------- | ----------------- | ----------------- |
| identity | long int | 0-9 | 1 (Unknown) | "Friend" | 3 ("friend") | **REQUIRED** |
| symbolset | short int | 00-99  | (per feature class) | "Air" | 01 ("air") | **REQUIRED** | 
| entity  | long int | N/A | (per feature class) | "Military (Air) : Fixed-Wing" | "110100"  | **REQUIRED** |
| context  | short int | 0-2 | 0 (Reality) | "Reality" | 0 ("reality") | Optional |
| modifier1 | long int | 00-99 | 00 (None) | "Mobility : Air Mobile/Air Assault" | Modifier "01" for Modifier 1 | Optional |
| modifier2 | long int | 00-99 | 00 (None)  | "Close Range and Support : Casualty Staging" | Modifier "05" for Modifier 2 | Optional |
| indicator | long int | 0-7 | 0 (None) | Headquarters | Headquarters=2 | Optional (="HQ/TF/FD") |
| echelon | long int | 0-26 | 0 (None) | Squad  | Squad=12 | Optional |
| mobility | long int | 0,31-52 (None) | 0 | Rail  | Rail=36 |  Optional |
| array | long int | 0,61-62 | 0 (None) | Short Towed Array | Short Towed Array=61 | Optional |
| operationalcondition | long int | 0-5 | 0 (Present) | Planned | Planned=1 | Optional |
| sidc | string/TEXT | string length(8 or 20) | N/A | "01100110" | SymbolSet:"Air"/"01" + Entity:"100110" | Optional-allows len=20 or len=8(identity=unknown) SIDC format |

## Modifier (Label) Attributes

The following modifier fields are supported in [the current Pro Stylx and Runtime Symbol Dictionary](../stylxfiles). 

To view the Label Rules and associations, view the symbol dictionary table "Label Rules." The following table lists the Modifier definitions from 2525C and 2525D and their corresponding attributes in the symbol dictionary.

| Attribute Name | 2525 C Field ID | 2525 D Field ID | Field Title | Notes |
| ------------- | -------------- | --- | ----------------- | ----- |
| additionalinformation  | H | H | Additional Information | |
| additionalinformation2 | H | H | Additional Information 2 | Used in the Convoys and Airspace Coordination Area tactical graphics. |
| combateffectiveness | K | K | Combat Effectiveness | |
| commonidentifier | AF | AF | Common Identifier | |
| credibility | J | J | Evaluation Rating | Credibility rating is second character of Evaluation Rating (J) field. |
| datetimeexpired | W | W | Date-Time Group (DTG) | Second half of Date-Time Group (DTG) (W) field. |
| datetimevalid | W | W | Date-Time Group (DTG) | First half of Date-Time Group (DTG) (W) field. |
| distance | AM | | Distance | |
| equipmentteardowntime | AE | | Equipment Teardown Time | |
| higherformation | M | M | Higher Formation | |
| idmode | P | P | IFF/SIF/AIS | |
| platformtype | AD | AD | Platform Type | |
| quantity | C | C | Quantity | |
| reinforced | F | F | Reinforced or Reduced | |
| reliability | J | J | Evaluation Rating | Reliability rating is first character of Evaluation Rating (J) field. |
| sigintmobility | R2 | R2 | SIGINT Mobility Indicator | |
| signatureequipment | L | L | Signature Equipment | |
| speed | Z | Z | Speed | |
| staffcomment | G | G | Staff Comments | |
| type | V | V | Type | |
| uniquedesignation | T | T | Unique Designation | |
| uniquedesignation2 | T | | Unique Designation 2 | Used as an additional field for Fire Support Lines tactical graphics. |
| x | Y | Y | Location | Longitude in degrees. |
| y | Y | Y | Location | Latitude in degrees. |
| z | X | X | Altitude/Depth | |
| zmax | X | X | Altitude/Depth | Maximum altitude for aviation tactical graphics. |
| zmin | X | X | Altitude/Depth | Minimum altitude for aviation tactical graphics. |
