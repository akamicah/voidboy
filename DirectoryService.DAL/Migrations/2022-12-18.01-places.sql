CREATE TABLE places
(
    id           UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID()           NOT NULL,
    createdAt    TIMESTAMP        DEFAULT CURRENT_TIMESTAMP           NOT NULL,
    updatedAt    TIMESTAMP        DEFAULT CURRENT_TIMESTAMP           NOT NULL,
    name         TEXT                                                 NOT NULL,
    description  TEXT             DEFAULT ''                          NOT NULL,
    visibility   SMALLINT         DEFAULT 1                           NOT NULL,
    maturity     SMALLINT         DEFAULT 0                           NOT NULL,
    tags         TEXT[]           DEFAULT NULL,
    domainId     UUID REFERENCES domains (id) ON DELETE CASCADE       NOT NULL,
    path         TEXT             DEFAULT ''                          NOT NULL,
    thumbnailUrl TEXT             DEFAULT ''                          NOT NULL,
    imageUrls    TEXT[],
    attendance   INT,
    sessionToken UUID REFERENCES sessionTokens (id) ON DELETE CASCADE NOT NULL,
    creatorIp    TEXT             DEFAULT NULL,
    lastActivity TIMESTAMP        DEFAULT NULL
);

CREATE TRIGGER places_updated_at
    BEFORE UPDATE
    ON places
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();

CREATE TABLE placeManagers
(
    id        UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID()    NOT NULL,
    createdAt TIMESTAMP        DEFAULT CURRENT_TIMESTAMP    NOT NULL,
    updatedAt TIMESTAMP        DEFAULT NULL,
    placeId   UUID REFERENCES places (id) ON DELETE CASCADE NOT NULL,
    userId    UUID REFERENCES users (id) ON DELETE CASCADE  NOT NULL,
    CONSTRAINT "place_manager_unique" UNIQUE (placeId, userId)
);

CREATE TRIGGER placeManagers_updated_at
    BEFORE UPDATE
    ON placeManagers
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();