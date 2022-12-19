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
    tags                  TEXT[]           DEFAULT NULL,
    domainId              UUID REFERENCES domains (id) ON DELETE CASCADE NOT NULL,
    path                  TEXT             DEFAULT ''                    NOT NULL,
    thumbnailUrl          TEXT             DEFAULT ''                    NOT NULL,
    imageUrls             TEXT[],
    currentAttendance     INT,
    currentInfo           TEXT,
    currentLastUpdateTime TIMESTAMP,
    currentApiKeyTokenId  UUID,
    creatorIp             TEXT             DEFAULT NULL,
    lastActivity          TIMESTAMP        DEFAULT NULL
);

CREATE TRIGGER places_updated_at
    BEFORE UPDATE
    ON places
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();
    