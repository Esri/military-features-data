.mode csv
/* TODO: UPDATE PATH BELOW TO PATH ON YOUR LOCAL MACHINE - ONLY FORWARD SLASHES '/' IN PATH - NOT BACKSLASHES '\' */
.import "C:/{TODO UPDATE PATH}" LegacyMappingLatestIcons
.import "C:/{TODO UPDATE PATH}" LegacyMappingOriginalIcons

CREATE INDEX 'LEGACYMAPPINGLATESTICONS_LEGACYKEY' ON 'LegacyMappingLatestIcons' ('LegacyKey');
CREATE INDEX 'LEGACYMAPPINGORIGINALICONS_LEGACYKEY' ON 'LegacyMappingOriginalIcons' ('LegacyKey');

VACUUM;

/* all done - but make sure you check for errors! */
.exit