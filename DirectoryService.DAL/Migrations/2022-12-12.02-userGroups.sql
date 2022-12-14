CREATE TABLE userGroups
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ownerUserId UUID REFERENCES users(id) ON DELETE CASCADE,
    internal BOOLEAN DEFAULT FALSE,
    name TEXT NOT NULL,
    description TEXT NOT NULL,
    rating INT DEFAULT 1
);

CREATE TRIGGER userGroups_updated_at
    BEFORE UPDATE
    ON userGroups
    FOR EACH ROW
    EXECUTE PROCEDURE updated_at_timestamp();