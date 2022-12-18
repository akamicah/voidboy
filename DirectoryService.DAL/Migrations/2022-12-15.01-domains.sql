CREATE TABLE domains
(
    id               UUID PRIMARY KEY                             DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt        TIMESTAMP                                    DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt        TIMESTAMP                                    DEFAULT NULL,
    name             TEXT                                                                   NOT NULL,
    description      TEXT                                         DEFAULT '',
    contactInfo      TEXT                                         DEFAULT '',
    hostNames        JSONB                                        DEFAULT NULL,
    thumbnailUrl     TEXT                                         DEFAULT '',
    images           JSONB                                        DEFAULT NULL,
    maturity         SMALLINT                                     DEFAULT 0,
    visibility       SMALLINT                                     DEFAULT 1                 NOT NULL,
    publicKey        TEXT                                         DEFAULT NULL,
    apiKey           UUID                                         DEFAULT NULL,
    sponsorAccount   UUID REFERENCES users (id) ON DELETE CASCADE DEFAULT NULL,
    iceServerAddress TEXT                                         DEFAULT NULL,
    version          TEXT                                         DEFAULT NULL,
    protocol         TEXT                                         DEFAULT NULL,
    networkAddress   TEXT                                         DEFAULT NULL,
    networkPort      INT                                          DEFAULT 0,
    networkingMode   SMALLINT                                     DEFAULT 0,
    restricted       BOOL                                         DEFAULT FALSE,
    numUsers         INT                                          DEFAULT 0,
    anonUsers        INT                                          DEFAULT 0,
    capacity         INT                                          DEFAULT 0,
    restriction      SMALLINT                                     DEFAULT 1,
    tags             TEXT                                         DEFAULT '',
    registerIp       TEXT                                         DEFAULT NULL,
    lastHeartbeat    TIMESTAMP                                    DEFAULT NULL,
    lastSenderKey    TEXT                                         DEFAULT ''
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
    domain    UUID REFERENCES domains (id) ON DELETE CASCADE NOT NULL,
    manager   UUID REFERENCES users (id) ON DELETE CASCADE   NOT NULL,
    CONSTRAINT "domain_manager_unique" UNIQUE (domain, manager)
);

CREATE TRIGGER domainManagers_updated_at
    BEFORE UPDATE
    ON domainManagers
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();