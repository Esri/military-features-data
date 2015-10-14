/*
SqliteDeNullifier.sql
Description: (Workaround) Removes JSON Null/"[]" from CIM 1.1 stylx's that cause .stylx's to fail in ArcGIS Runtime Quartz
Notes: these nulls are incorrectly introduced in Pro "Upgrade to Pro/CIM 1.1" - this issue is being reported/tracked/worked
*/

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"effects":[]', '') WHERE (CONTENT LIKE '%,"effects":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"callout":[]', '') WHERE (CONTENT LIKE '%,"callout":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"haloSymbol":[]', '') WHERE (CONTENT LIKE '%,"haloSymbol":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"symbol3DProperties":[]', '') WHERE (CONTENT LIKE '%,"symbol3DProperties":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"markerPlacement":[]', '') WHERE (CONTENT LIKE '%,"markerPlacement":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"shadowColor":[]', '') WHERE (CONTENT LIKE '%,"shadowColor":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"markerGraphics":[]', '') WHERE (CONTENT LIKE '%,"markerGraphics":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"sourceModifiedTime":[],"layerElevation":[],"layer3DProperties":[],"layerMasks":[],"layerTemplate":[],"popupInfo":[],"layers":[],"symbolLayerDrawing":[]', '') WHERE (CONTENT LIKE '%,"sourceModifiedTime":[],"layerElevation":[],"layer3DProperties":[],"layerMasks":[],"layerTemplate":[],"popupInfo":[],"layers":[],"symbolLayerDrawing":[]%');

UPDATE ITEMS SET CONTENT = REPLACE(CONTENT, ',"colorSubstitutions":[],"tintColor":[]', '') WHERE (CONTENT LIKE '%,"colorSubstitutions":[],"tintColor":[]%');

/* clean up */
VACUUM;