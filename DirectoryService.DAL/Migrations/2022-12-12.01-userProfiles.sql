CREATE TABLE userProfiles
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    userId UUID UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    displayName TEXT DEFAULT 'anonymous',
    biography TEXT DEFAULT '',
    heroImageUrl TEXT DEFAULT NULL,
    thumbnailImageUrl TEXT DEFAULT NULL,
    tinyImageUrl TEXT DEFAULT NULL
);

CREATE TRIGGER userProfiles_updated_at
    BEFORE UPDATE
    ON userProfiles
    FOR EACH ROW
    EXECUTE PROCEDURE updated_at_timestamp();