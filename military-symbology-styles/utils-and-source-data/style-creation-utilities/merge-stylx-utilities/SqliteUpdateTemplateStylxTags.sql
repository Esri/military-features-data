/*
Purpose: this script is an administrative tool to update the *Tags* column of the manually maintained .stylx:
        `mil2525d-lines-areas-labels-base-template.stylx`  
         if/when the source data (as maintained in Military-All-Icons.csv) changes 

Prerequisites: 
1. A version of the .stylx that is edited and maintained in ArcGIS Pro: `mil2525d-lines-areas-labels-base-template.stylx` 
     Copy this file: `mil2525d-lines-areas-labels-base-template.stylx` into the current/merge folder
2. A Sqlite Editor/Interpreter capable of executing SQL commands (ex. sqlite3.exe)
3. The 3 Icon specification files for the **Control Measures**: 
   1. Military-ControlMeasures-Source-Points.csv
   2. Military-ControlMeasures-Source-Lines.csv
   3. Military-ControlMeasures-Source-Areas.csv
   4. Copy these files into the current/merge folder
   5. Additional Notes:
      Obtained from: https://github.com/Esri/joint-military-symbology-xml/blob/master/samples/imagefile_name_category_tags
      With the following required fields: `styleItemCategory, styleItemName, styleItemUniqueId, styleItemTags`

To Run:
1. In this file, replace all instances of `{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}` 
   with the fully qualified local path to this folder: `....utilities/style-utilities/merge-stylx-utilities` 
   being careful to use forward slashes (`/`) rather than backslashes (`\`)
   and save the file 
1. Run sqlite3.exe
2. .read "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/SqliteUpdateTemplateStylxTags.sql"
-or-
sqlite3.exe < "{PATH_TO_LOCAL_MIL_FEATURES_REPO}\style-utilities\merge-stylx-utilities\SqliteUpdateTemplateStylxTags.sql"
*/

.print "Attaching stylx..."
attach "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/mil2525d-lines-areas-labels-base-template.stylx" as mil2525d;

/* Import the icon specification files for the Control Measures */
.print "Importing Military-All-Icons.csv to use for join..."
.mode csv
.import "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/Military-ControlMeasures-Source-Points.csv" NameJoin1

.import "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/Military-ControlMeasures-Source-Lines.csv" NameJoin2

.import "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/Military-ControlMeasures-Source-Areas.csv" NameJoin3

.print "Print current tables attached..."
.tables

/* Now Join on those specification files to update the Tags with the data from the spec files */
.print "Updating Tags where ITEMS.Key *contains* NameJoin.styleItemUniqueId for each csv file..."
INSERT OR REPLACE INTO mil2525d.ITEMS
 SELECT ITEMS.ID,ITEMS.CLASS,ITEMS.CATEGORY,ITEMS.NAME,NameJoin1.styleItemTags,ITEMS.CONTENT,ITEMS.Key,ITEMS.LabelRules
  FROM ITEMS JOIN NameJoin1 ON instr(ITEMS.Key, NameJoin1.styleItemUniqueId) > 0;
  
INSERT OR REPLACE INTO mil2525d.ITEMS
 SELECT ITEMS.ID,ITEMS.CLASS,ITEMS.CATEGORY,ITEMS.NAME,NameJoin2.styleItemTags,ITEMS.CONTENT,ITEMS.Key,ITEMS.LabelRules
  FROM ITEMS JOIN NameJoin2 ON instr(ITEMS.Key, NameJoin2.styleItemUniqueId) > 0;

INSERT OR REPLACE INTO mil2525d.ITEMS
 SELECT ITEMS.ID,ITEMS.CLASS,ITEMS.CATEGORY,ITEMS.NAME,NameJoin3.styleItemTags,ITEMS.CONTENT,ITEMS.Key,ITEMS.LabelRules
  FROM ITEMS JOIN NameJoin3 ON instr(ITEMS.Key, NameJoin3.styleItemUniqueId) > 0;
  
