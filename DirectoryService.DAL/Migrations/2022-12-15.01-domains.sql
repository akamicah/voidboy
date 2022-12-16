CREATE TABLE domains
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedAt TIMESTAMP DEFAULT NULL,
    name TEXT NOT NULL,
    description TEXT DEFAULT '',
    contactInfo TEXT DEFAULT '',
    thumbnail TEXT DEFAULT '',
    maturity SMALLINT DEFAULT 0,
    visibility TEXT NOT NULL,
    publicKey TEXT DEFAULT NULL,
    apiKey UUID DEFAULT NULL,
    sponsorAccount UUID REFERENCES users(id) DEFAULT NULL,
    iceServerAddress TEXT DEFAULT NULL,
    version TEXT DEFAULT NULL,
    protocol TEXT DEFAULT NULL,
    networkAddress TEXT DEFAULT NULL,
    networkPort INT DEFAULT 0,
    networkingMode SMALLINT DEFAULT 0,
    restricted BOOL DEFAULT FALSE,
    numUsers INT DEFAULT 0,
    anonUsers INT DEFAULT 0,
    capacity INT DEFAULT 0,
    restriction TEXT DEFAULT 'open',
    tags TEXT DEFAULT '',
    registerIp TEXT DEFAULT NULL,
    lastHeartbeat TIMESTAMP DEFAULT NULL,
    
    
    
    
    
    
    
    
    
    
    
    
    
);

CREATE TABLE domainHostNames
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    domainId UUID REFERENCES domains(id),
    userId UUID REFERENCES users(id)
);

CREATE TABLE domainManagers
(
    id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    domainId UUID REFERENCES domains(id),
    userId UUID REFERENCES users(id)
);

CREATE TRIGGER domains_updated_at
    BEFORE UPDATE
    ON domains
    FOR EACH ROW
    EXECUTE PROCEDURE updated_at_timestamp();