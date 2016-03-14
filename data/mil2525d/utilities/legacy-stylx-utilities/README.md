Create Legacy Stylx

To create a fresh, brand new, MIL-STD-2525D stylx with MIL-STD-2525C legacy support (one stylx that can be used with both the 2525D dictionary renderer plugin and the 2525D-Legacy dictionary renderer plugin), perform the following steps.

* Follow all of the procedures documented here: https://github.com/Esri/military-features-data/tree/v.next/data/mil2525d/utilities/style-utilities
* Create a local copy of Sqlite.sql that is contained in this folder. 
* Edit the .import line of this SQL script to reflect your local path to a downloaded copy of this file: https://github.com/Esri/joint-military-symbology-xml/blob/master/samples/legacy_support/All_ID_Mapping_C_to_D.csv
* Create a local copy of SqliteScriptRunner.bat that is contained in this folder. 
* Edit the SQLite path in this batch file to point to your local copy of the SQLIte executable.
* Edit the SQLite database path in this batch file to point to the mil2525d.stylx you created earlier.
* Edit the SQLite script path in this batch file to point to the above edited SQLIte script file.
* Run this batch file and then use a SQLite editor to confirm that a new LegacyMapping table has been created in the stylx file.
* Make a copy of the final stylx and rename that copy to mil2525d_to_legacy.stylx

* You can now use the newly created mil2525d.stylx and the mil2525d_to_legacy.stylx files with their corresponding Dictionary renderer plugins.