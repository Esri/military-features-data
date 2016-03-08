.mode csv
/* TODO: UPDATE PATH BELOW TO PATH ON YOUR LOCAL MACHINE - ONLY FORWARD SLASHES '/' IN PATH - NOT BACKSLASHES '\' */
.import "C:/{TODO UPDATE PATH}" LegacyMapping

CREATE INDEX 'LEGACYMAPPING_KEY2525C' ON 'LegacyMapping' ('Key2525C');

/* WORKAROUND TO GET Special C2 Entity Icons to show up until fixed in source data */
UPDATE LEGACYMAPPING SET ExtraIcon = '10XXXX97' WHERE MainIcon like '%97';
UPDATE LEGACYMAPPING SET MainIcon = REPLACE(MainIcon, '97', '00') WHERE MainIcon like '%97';
UPDATE LEGACYMAPPING SET ExtraIcon = '10XXXX98' WHERE MainIcon like '%98';
UPDATE LEGACYMAPPING SET MainIcon = REPLACE(MainIcon, '98', '00') WHERE MainIcon like '%98';

VACUUM;

/* all done - but make sure you check for errors! */
.exit