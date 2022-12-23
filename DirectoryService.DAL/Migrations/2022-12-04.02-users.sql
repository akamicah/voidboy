CREATE TABLE users
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    identityProvider SMALLINT DEFAULT 0,
    username TEXT UNIQUE NOT NULL,
    email TEXT UNIQUE,
    authVersion SMALLINT NOT NULL,
    authHash TEXT,
    activated BOOL,
    role SMALLINT NOT NULL,
    state SMALLINT DEFAULT 0,
    creatorIp TEXT DEFAULT '0.0.0.0',
    connectionGroup UUID DEFAULT NULL,
    friendGroup UUID DEFAULT NULL,
    language TEXT DEFAULT 'en'
);

CREATE TRIGGER users_updated_at
    BEFORE UPDATE
    ON users
    FOR EACH ROW
EXECUTE PROCEDURE updated_at_timestamp();