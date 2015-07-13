# military-features-data / data / mil2525d / core_data / gdbs
=========================================

# Purpose 

This folder contains the Military Features Core Geodatabase Schema format and an empty geodatabase (in .zip format) conforming to the current Military Features schema. 

## Additional Schema Information

The following information is intended to succinctly capture the format of the Military Features Schema for other applications that may depend upon this format. 

## Graphic Display Amplifiers

The following fields control the display of symbols.

| Attribute Name | 2525 D Field ID | Data Type | Range of Values | Default Value/Meaning | Sample Name | Sample Value/Meaning | Explanatory Notes |
| -------------- | --------- | --------- | --------------- | ----------- | -------------------- | ----------------- | ----------------- |
| symbolset | A | short int | 00-99  | (per feature class) | "Air" | 01 ("air") | **REQUIRED** | 
| entity  | A | long int | N/A | (per feature class) | "Military (Air) : Fixed-Wing" | "110100"  | **REQUIRED** |
| modifier1 | A | long int | 00-99 | 00 (None) | "Mobility : Air Mobile/Air Assault" | Modifier "01" for Modifier 1 | Optional |
| modifier2 | A | long int | 00-99 | 00 (None)  | "Close Range and Support : Casualty Staging" | Modifier "05" for Modifier 2 | Optional |
| echelon | B | long int | 0-26 | 0 (None) | Squad  | Squad=12 | Optional |
| indicator | D/S/AB | long int | 0-7 | 0 (None) | Headquarters | Headquarters=2 | Optional (="HQ/TF/FD") |
| identity | E | long int | 0-9 | 1 (Unknown) | "Friend" | 3 ("friend") | **REQUIRED** |
| context  | E | short int | 0-2 | 0 (Reality) | "Reality" | 0 ("reality") | Optional |
| mobility | R | long int | 0,31-52 (None) | 0 | Rail  | Rail=36 |  Optional |
| array | AG | long int | 0,61-62 | 0 (None) | Short Towed Array | Short Towed Array=61 | Optional |
| installation | AC | | | | | | Not in Datamodel |
| operationalcondition | AL | long int | 0-5 | 0 (Present) | Planned | Planned=1 | Optional |
| sidc | | string/TEXT | string length(8 or 20) | N/A | "01100110" | SymbolSet:"Air"/"01" + Entity:"100110" | Optional-allows len=20 or len=8(identity=unknown) SIDC format |

## Text Amplifiers

The following modifier fields are supported in [the current Pro Stylx and Runtime Symbol Dictionary](../stylxfiles). 

To view the Label Rules and associations, view the symbol dictionary table "Label Rules." The following table lists the Modifier definitions from 2525D and their corresponding attributes in the symbol dictionary.

| Attribute Name | 2525 D Field ID | Field Title | Notes |
| ------------- | --- | ----------------- | ----- |
| quantity | C | Quantity | |
| reinforced | F | Reinforced or Reduced | |
| staffcomment | G | Staff Comments | |
| additionalinformation | H | Additional Information | |
| additionalinformation2 | H1 | Additional Information 2 | Used in the Convoys and Airspace Coordination Area tactical graphics. |
| credibility | J | Evaluation Rating | Credibility rating is second character of Evaluation Rating (J) field. |
| reliability | J1 | Evaluation Rating | Reliability rating is first character of Evaluation Rating (J) field. |
| combateffectiveness | K | Combat Effectiveness | |
| signatureequipment | L | Signature Equipment | |
| higherformation | M | Higher Formation | |
| hostile | N | Hostile | |
| idmode | P | IFF/SIF/AIS | |
| direction | Q | Direction of Movement Indicator | |
| sigintmobility | R2 | SIGINT Mobility Indicator | |
| uniquedesignation | T | Unique Designation | |
| uniquedesignation2 | T1 | Unique Designation 2 | Used as an additional field for Fire Support Lines tactical graphics. |
| type | V | Type | |
| datetimevalid | W | Date-Time Group (DTG) | First half of Date-Time Group (DTG) (W) field. |
| datetimeexpired | W1 | Date-Time Group (DTG) | Second half of Date-Time Group (DTG) (W) field. Many systems use W1 as a trigger to delete symbol from map. |
| z | X | Altitude/Depth | Maximum altitude for aviation tactical graphics. |
| z2 | X1 | Altitude/Depth 2 | Minimum altitude for aviation tactical graphics. |
| x | Y | Location | Longitude in degrees. |
| x2 | Y1 | Location | Longitude in degrees 2. |
| y | Y | Location | Latitude in degrees. |
| y2 | Y1 | Location | Latitude in degrees 2. |
| speed | Z | Speed | |
| specialheadquarters | AA | Special C2 Headquarters | Not in Datamodel |
| platformtype | AD | Platform Type | |
| equipmentteardowntime | AE | Equipment Teardown Time | |
| commonidentifier | AF | Common Identifier | |
| distance | AM | Distance | |
| distance1 | AM1 | Distance 2 | |
| azimuth | AN | Azimuth | |
| targetdesignator | AP | Target Designator |
| guardedunit | AQ | Guarded Unit | Not in Datamodel |
| specialdesignator | AR | Special Designator | Not in Datamodel |
| country | AS | Country Code | |
