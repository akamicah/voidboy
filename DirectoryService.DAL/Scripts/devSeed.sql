INSERT INTO users(id, identityprovider, username, email, authversion, authhash, activated, role, state, creatorip)
VALUES('6465b186-5fc9-46d2-842f-da8542ba9939', 0, 'testadmin', 'admin@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 2, 0, '127.0.0.1');

INSERT INTO sessionTokens(id, refreshtoken, accountid, scope, expires)
VALUES('b4b7349b-40f8-40a2-a829-926f5d5f3124', 'd72dbc18-c70b-4f36-987f-51babf406d5a', '6465b186-5fc9-46d2-842f-da8542ba9939', 1, '2025-12-19 13:24:50.321461');

INSERT INTO users(identityprovider, username, email, authversion, authhash, activated, role, state, creatorip)
VALUES(0, 'user', 'user@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 1, 0, '127.0.0.1');

