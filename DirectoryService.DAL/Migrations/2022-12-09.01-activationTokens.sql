CREATE TABLE activationTokens
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT NULL,
    accountId UUID REFERENCES users(id) ON DELETE CASCADE,
    expires TIMESTAMP NOT NULL,
    deleted BOOL DEFAULT FALSE
);

CREATE TRIGGER activationTokens_updated_at
    BEFORE UPDATE
    ON activationTokens
    FOR EACH ROW
    EXECUTE PROCEDURE updated_at_timestamp();