/*
Prerequisites: 
1. 2 Versions of the .stylx files:
  1. A Pro/Runtime stylx file with all of the 2525D Point Icons named : `mil2525d-points-to-add-keys-labels.stylx`
  2. A version of the .stylx that is edited and maintained in ArcGIS Pro: `mil2525d.stylx` 
     (This is the copied/renamed file: `mil2525d-lines-areas-labels-base-template.stylx`)
2. A Sqlite Editor/Interpreter capable of executing SQL commands (ex. sqlite3.exe)
3. An Icon specification file: Military-All-Icons.csv 
   1. (Obtained from: https://github.com/Esri/joint-military-symbology-xml/blob/master/samples/imagefile_name_category_tags/Military-All-Icons.csv) 
   2. With the following required fields: `styleItemCategory, styleItemName, styleItemUniqueId, styleItemTags`
*/

/* Conversion Steps - Launch the Sqlite editor/interpreter of your choice */
/* You may be able to run this as .sql script if you edit {Full Path To} to the local path */

attach "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/mil2525d-points-to-add-keys-labels.stylx" as mil2525d_points;
attach "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/mil2525d.stylx" as mil2525d;

/* Example:
attach "C:/Github/military-features-data/source/utilities/merge-stylx-utilities/mil2525d-points-to-add-keys-labels.stylx" as mil2525d_points;
attach "C:/Github/military-features-data/source/utilities/merge-stylx-utilities/mil2525d.stylx" as mil2525d;
--> IMPORTANT/TRICKY NOTE: the import/attach commands require **forward** slashes (even on Windows)
*/

/* ensure all are attached */
.databases

/* Add these required columns to points version of the database */
/* TODO: Pro will eventually need to add these to the default stylx schema, so these commands may no longer be needed at some point */
/* NOTE: This Column("Key") did get added at Pro 1.1 */
/*ALTER TABLE mil2525d_points.ITEMS ADD COLUMN Key TEXT; */ 
/* NOTE: Still need to add this one("LabelRules"): */
ALTER TABLE mil2525d_points.ITEMS ADD COLUMN LabelRules TEXT; 

/* Set the Key and Label Rules for the points version of the database (from Military-All-Icons.csv) */

.mode csv
.import "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/Military-All-Icons.csv" NameJoin

/* Example: 
.import "C:/Github/military-features-data/source/utilities/merge-stylx-utilities/Military-All-Icons.csv" NameJoin
--> IMPORTANT/TRICKY NOTE: the import/attach commands require **forward** slashes (even on Windows)
*/

.tables

/* WARNING: These commands require that the .csv files imported have: 
   A header row that have these -exact- column names: 
   styleItemCategory, styleItemName, styleItemUniqueId, styleItemTags */

/* NOTE: sql the follows looks whacky, but "INSERT OR REPLACE" is how you need to do an update with a Join in sqlite */

/* Create Keys for new items and also need to Fix Long (>255) Tags that got truncated when style created */
INSERT OR REPLACE INTO mil2525d_points.ITEMS
 SELECT ITEMS.ID,ITEMS.CLASS,ITEMS.CATEGORY,ITEMS.NAME,NameJoin.styleItemTags,ITEMS.CONTENT,NameJoin.styleItemUniqueId,ITEMS.LabelRules
  FROM ITEMS JOIN NameJoin ON NameJoin.styleItemName = ITEMS.NAME;

/* Check Tag Length (to make sure that Tags got fixed) */
SELECT COUNT(*) AS Matches FROM (select CLASS, CATEGORY,NAME, TAGS, Key 
 from mil2525d_points.ITEMS where (LENGTH(TAGS) > 254));

DROP TABLE NameJoin;
.tables

/* IMPORTANT: Set the labels for the frames - this(the frame symbol) is how the labels will be 
              associated with a symbol                                                                */
/* TODO: Currently every frame is getting set to the same label set, this will need refined/changed   */
UPDATE mil2525d_points.ITEMS SET LabelRules = '30;31;32;33;34;35;36' where (CATEGORY == 'Frame');

/********************************************************************/
/* Fix these lowercase Key ones and poorly-named categories         */
/* (TODO: these commands may no longer be needed and can be removed 
    when fixed in source data)                                      */

UPDATE mil2525d_points.ITEMS SET Key = REPLACE(Key, 'xxxx', 'XXXX') WHERE (Key LIKE '%xxxx%');

UPDATE mil2525d_points.ITEMS SET CATEGORY = 'Control Measure : Point' 
  where ((CLASS = 3) and (CATEGORY = 'Control Measure : Main Icon'));

UPDATE mil2525d_points.ITEMS SET CATEGORY = 'Meteorological-Atmospheric : Point' 
  where ((CLASS = 3) and (CATEGORY = 'Meteorological - Atmospheric : Main Icon'));

UPDATE mil2525d_points.ITEMS SET CATEGORY = 'Meteorological-Oceanographic : Point' 
  where ((CLASS = 3) and (CATEGORY = 'Meteorological - Oceanographic : Main Icon'));

/********************************************************************/

/* Now merge the 2 databases: 
   mil2525d will now have the point symbols added and the keys set  */

insert into mil2525d.ITEMS (CLASS,CATEGORY,NAME,TAGS,CONTENT,Key,LabelRules)
  select ITEMS.CLASS,ITEMS.CATEGORY,ITEMS.NAME,ITEMS.TAGS,ITEMS.CONTENT,ITEMS.Key,ITEMS.LabelRules 
    from mil2525d_points.ITEMS;

detach mil2525d_points;

/* Add version information */
.mode csv
.import "{FULL-PATH-TO-MERGE-FOLDER-NO-BACKSLASHES}/versions.csv" meta

/* Example: 
.import "C:/Github/military-features-data/source/utilities/merge-stylx-utilities/versions.csv" meta
--> IMPORTANT/TRICKY NOTE: the import/attach commands require **forward** slashes (even on Windows)
*/

/* ensure mil2525d_points detached */
.databases

/* clean up - really shouldn't be any (nothing was deleted), but just in case process changes */
VACUUM;

/* all done - but make sure you check for errors! */
.exit