CREATE TABLE places
(
    id                    UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID()     NOT NULL,
    createdAt             TIMESTAMP        DEFAULT CURRENT_TIMESTAMP     NOT NULL,
    updatedAt             TIMESTAMP        DEFAULT NULL,
    name                  TEXT                                           NOT NULL,
    displayName           TEXT                                           NOT NULL,
    description           TEXT             DEFAULT ''                    NOT NULL,
    visibility            SMALLINT         DEFAULT 1                     NOT NULL,
    maturity              SMALLINT         DEFAULT 0                     NOT NULL,
    tags                  JSONB            DEFAULT NULL                  NOT NULL,
    domainId              UUID REFERENCES domains (id) ON DELETE CASCADE NOT NULL,
    path                  TEXT             DEFAULT ''                    NOT NULL,
    thumbnailUrl          TEXT             DEFAULT ''                    NOT NULL,
    currentAttendance     INT,
    currentInfo           JSONB,
    currentLastUpdateTime TIMESTAMP,
    currentApiKeyTokenId  UUID,
    registerIp            TEXT             DEFAULT NULL,
    lastActivity          TIMESTAMP        DEFAULT NULL
);
    
    
    