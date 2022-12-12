-- Admin User. Username: admin, Password: password
INSERT INTO users(id, identityprovider, username, email, authversion, authhash, activated, role, state, creatorip)
VALUES('6465b186-5fc9-46d2-842f-da8542ba9939', 0, 'testadmin', 'admin@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 2, 0, '127.0.0.1');

INSERT INTO sessionTokens(id, refreshtoken, accountid, scope, expires)
VALUES('b4b7349b-40f8-40a2-a829-926f5d5f3124', 'd72dbc18-c70b-4f36-987f-51babf406d5a', '6465b186-5fc9-46d2-842f-da8542ba9939', 1, '2025-12-19 13:24:50.321461');

INSERT INTO userProfiles(userId, displayName, biography, heroImageUrl, thumbnailImageUrl, tinyImageUrl)
VALUES('6465b186-5fc9-46d2-842f-da8542ba9939', 'Admin', 'A simple admin account', 'https://i.pravatar.cc/512', 'https://i.pravatar.cc/42', 'https://i.pravatar.cc/32');

-- Standard User. Username: user, Password: password
INSERT INTO users(id, identityprovider, username, email, authversion, authhash, activated, role, state, creatorip)
VALUES('4115ea77-3c13-4a24-9a2e-701893ee0d61', 0, 'user', 'user@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 1, 0, '127.0.0.1');

INSERT INTO userProfiles(userId, displayName, biography, heroImageUrl, thumbnailImageUrl, tinyImageUrl)
VALUES('4115ea77-3c13-4a24-9a2e-701893ee0d61', 'User', 'A simple user account', 'https://i.pravatar.cc/512', 'https://i.pravatar.cc/42', 'https://i.pravatar.cc/32');

-- Connect Admin and Standard User
INSERT INTO userConnections(userAId, userBId) 
VALUES('6465b186-5fc9-46d2-842f-da8542ba9939', '4115ea77-3c13-4a24-9a2e-701893ee0d61');

INSERT INTO userConnections(userAId, userBId)
VALUES('4115ea77-3c13-4a24-9a2e-701893ee0d61', '6465b186-5fc9-46d2-842f-da8542ba9939');

-- Standard User. Username: user2, Password: password
INSERT INTO users(id, identityprovider, username, email, authversion, authhash, activated, role, state, creatorip)
VALUES('8b63354b-4aa0-4f8c-9b91-ccb332b8939c', 0, 'user2', 'user2@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 1, 0, '127.0.0.1');

INSERT INTO userProfiles(userId, displayName, biography, heroImageUrl, thumbnailImageUrl, tinyImageUrl)
VALUES('8b63354b-4aa0-4f8c-9b91-ccb332b8939c', 'User 2', 'A simple user account', 'https://i.pravatar.cc/512', 'https://i.pravatar.cc/42', 'https://i.pravatar.cc/32');