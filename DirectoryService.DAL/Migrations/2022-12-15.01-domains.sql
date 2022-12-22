CREATE TABLE domains
(
    id               UUID PRIMARY KEY                             DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt        TIMESTAMP                                    DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt        TIMESTAMP                                    DEFAULT NULL,
    name             TEXT                                                                   NOT NULL,
    description      TEXT                                         DEFAULT '',
    contactInfo      TEXT                                         DEFAULT '',
    thumbnailUrl     TEXT                                         DEFAULT ''                NOT NULL,
    imageUrls        TEXT[]                                       DEFAULT NULL,
    maturity         SMALLINT                                     DEFAULT 0,
    visibility       SMALLINT                                     DEFAULT 1                 NOT NULL,
    publicKey        TEXT                                         DEFAULT NULL,
    sessionToken     UUID REFERENCES sessionTokens (id)           DEFAULT NULL,
    ownerUserId      UUID REFERENCES users (id) ON DELETE CASCADE DEFAULT NULL,
    iceServerAddress TEXT                                         DEFAULT NULL,
    version          TEXT                                         DEFAULT NULL,
    protocolVersion  TEXT                                         DEFAULT NULL,
    networkAddress   TEXT                                         DEFAULT NULL,
    networkPort      INT                                          DEFAULT 0,
    networkingMode   SMALLINT                                     DEFAULT 0,
    restricted       BOOL                                         DEFAULT FALSE,
    capacity         INT                                          DEFAULT 0,
    restriction      SMALLINT                                     DEFAULT 1,
    tags             TEXT[]                                       DEFAULT NULL,
    creatorIp        TEXT                                         DEFAULT NULL,
    lastHeartbeat    TIMESTAMP                                    DEFAULT NULL,
    active           BOOLEAN                                      DEFAULT TRUE,
    anonCount        INT                                          DEFAULT 0,
    userCount        INT                                          DEFAULT 0
);

CREATE TRIGGER domains_updated_at
    BEFORE UPDATE
    ON domains
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();

CREATE TABLE domainManagers
(
    id        UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID()     NOT NULL,
    createdAt TIMESTAMP        DEFAULT CURRENT_TIMESTAMP     NOT NULL,
    updatedAt TIMESTAMP        DEFAULT NULL,
    domainId  UUID REFERENCES domains (id) ON DELETE CASCADE NOT NULL,
    userId    UUID REFERENCES users (id) ON DELETE CASCADE   NOT NULL,
    CONSTRAINT "domain_manager_unique" UNIQUE (domainId, userId)
);

CREATE TRIGGER domainManagers_updated_at
    BEFORE UPDATE
    ON domainManagers
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();