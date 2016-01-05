/*
SqliteDeNullifier.sql
Description: (Workaround) Removes JSON Null/"[]" from CIM 1.1/1.2 stylx's that cause .stylx's to fail in ArcGIS Runtime Quartz
Notes: these nulls are incorrectly introduced in Pro "Upgrade to Pro/CIM 1.1/1.2" - this issue is being reported/tracked/worked
*/

.print "Starting..."

/* replace spurious nulls [] */
.print "Removing nulls..."

/* Common Issues Found: */

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"effects":[]', '') WHERE (CONTENT LIKE '%,"effects":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"callout":[]', '') WHERE (CONTENT LIKE '%,"callout":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"haloSymbol":[]', '') WHERE (CONTENT LIKE '%,"haloSymbol":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"symbol3DProperties":[]', '') WHERE (CONTENT LIKE '%,"symbol3DProperties":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"markerPlacement":[]', '') WHERE (CONTENT LIKE '%,"markerPlacement":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"shadowColor":[]', '') WHERE (CONTENT LIKE '%,"shadowColor":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"markerGraphics":[]', '') WHERE (CONTENT LIKE '%,"markerGraphics":[]%');

/* Less Common Issues Found (these may be old and no longer issues & can be checked/removed at some point): */

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"sourceModifiedTime":[]', '') WHERE (CONTENT LIKE '%,"sourceModifiedTime":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"layerElevation":[]', '') WHERE (CONTENT LIKE '%,"layerElevation":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"layer3DProperties":[]', '') WHERE (CONTENT LIKE '%,"layer3DProperties":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"layerMasks":[]', '') WHERE (CONTENT LIKE '%,"layerMasks":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"layerTemplate":[]', '') WHERE (CONTENT LIKE '%,"layerTemplate":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"popupInfo":[]', '') WHERE (CONTENT LIKE '%,"popupInfo":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"charts":[]', '') WHERE (CONTENT LIKE '%,"charts":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"layers":[]', '') WHERE (CONTENT LIKE '%,"layers":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"symbolLayerDrawing":[]', '') WHERE (CONTENT LIKE '%,"symbolLayerDrawing":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"colorSubstitutions":[]', '') WHERE (CONTENT LIKE '%,"colorSubstitutions":[]%');
UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"tintColor":[]', '') WHERE (CONTENT LIKE '%,"tintColor":[]%');

/* clean up */
.print "VACUUM..."
VACUUM;

.print "DONE"