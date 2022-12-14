CREATE TABLE emailQueue
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    userId UUID REFERENCES users(id) ON DELETE CASCADE,
    model TEXT DEFAULT NULL,
    type INT DEFAULT 0 NOT NULL,
    sendOn TIMESTAMP NOT NULL,
    attempt INT DEFAULT 0,
    sent BOOL DEFAULT FALSE,
    sentOn TIMESTAMP DEFAULT NULL
);

CREATE TRIGGER emailQueue_updated_at
    BEFORE UPDATE
    ON emailQueue
    FOR EACH ROW
    EXECUTE PROCEDURE updated_at_timestamp();