/* 
File: SqliteStyleCommands.sql
Purpose: Performs any specific updates that are not currently exposed by the Pro API to update the style
         This that still need to be done using sqlite

Requirements:  
1. A Pro/Runtime compatible stylx file ("Mobile" Style File)
2. A Sqlite Editor/Interpreter capable of executing SQL commands (ex. sqlite3.exe)

*/

/*****************************/
/* SET THE LABELRULES Column */
/*****************************/

/* First: set the general case for all framed symbols here: */
UPDATE ITEMS SET LabelRules = '21;27;29;30;31;32;33;34;35' where (CATEGORY == 'Frame');

/* Next: set the specific cases below: */

/* TODO: Currently every frame is getting set to the same label set, this shoulb be refined/changed below: */

/* TODO: Add Label Rules specific to each symbol set: */
/* Ex: Symbol Set 40: (Icons contain "40_") */
/* 
   UPDATE ITEMS SET LabelRules = '21;27;29;30;31;32;33;34;35' where 
     ((CATEGORY == 'Frame') and (instr(Key, '40_') > 0)); 
*/



