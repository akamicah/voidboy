CREATE TABLE userGroupMembers
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT NULL,
    userGroupId UUID REFERENCES userGroups(id) ON DELETE CASCADE,
    userId UUID REFERENCES users(id) ON DELETE CASCADE,
    isOwner bool DEFAULT FALSE,    
    CONSTRAINT "userGroupId_userId_unique" UNIQUE (usergroupid, userid)
);

CREATE TRIGGER userGroupMembers_updated_at
    BEFORE UPDATE
    ON userGroupMembers
    FOR EACH ROW
    EXECUTE PROCEDURE updated_at_timestamp();