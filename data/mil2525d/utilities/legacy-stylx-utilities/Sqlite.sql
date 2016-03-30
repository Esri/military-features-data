.mode csv
/* TODO: UPDATE PATH BELOW TO PATH ON YOUR LOCAL MACHINE - ONLY FORWARD SLASHES '/' IN PATH - NOT BACKSLASHES '\' */
.import "C:/{TODO UPDATE PATH}" LegacyMapping

CREATE INDEX 'LEGACYMAPPING_KEY2525C' ON 'LegacyMapping' ('Key2525C');

VACUUM;

/* all done - but make sure you check for errors! */
.exit