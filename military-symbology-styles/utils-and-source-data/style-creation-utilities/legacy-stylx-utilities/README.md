Legacy (Military Standard) Stylx Utilities

To create a new, MIL-STD-2525D stylx with MIL-STD-2525C/B2 legacy support (one stylx that can be used with both the 2525D dictionary renderer plugin and the 2525C/B2 dictionary renderer), perform the following steps:

* Follow all of the [procedures documented here](../README.md) to create a mil2525d.stylx
* Make a copy of the final stylx and rename that copy to mil2525c_b2.stylx
* Create a local copy of Sqlite.sql that is contained in this folder. 
* Edit the two `.import` lines of this SQL script to reflect your local paths to the downloaded copies of these files: 
	* [All_ID_Mapping_Latest.csv](../../style-source-files/mil2525c_b2/legacy_support)
	* [All_ID_Mapping_Original.csv](../../style-source-files/mil2525c_b2/legacy_support)
* Create a local copy of SqliteScriptRunner.bat that is contained in this folder. 
* Edit the SQLite path in this batch file to point to your local copy of the SQLIte executable.
* Edit the SQLite database path in this batch file to point to the mil2525c_b2.stylx you created earlier.
* Edit the SQLite script path in this batch file to point to the above edited SQLIte script file.
* Run this batch file and then use a SQLite editor to confirm that two new legacy mapping tables have been created in the stylx file.

* You can now use the newly created mil2525c_b2 with its corresponding Dictionary Renderer.