-- Users
INSERT INTO users(id, identityprovider, username, email, authversion, authhash, activated, role, state, creatorip, connectionGroup, friendGroup) VALUES 
('6465b186-5fc9-46d2-842f-da8542ba9939', 0, 'testadmin', 'admin@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 2, 0, '127.0.0.1', 'a517237f-8457-4f87-a38b-de68c274ff24', 'ddedda75-8dd7-477f-9b91-b12efcb25522'),
('4115ea77-3c13-4a24-9a2e-701893ee0d61', 0, 'user', 'user@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 1, 0, '127.0.0.1', 'f2714eb7-e720-4597-b404-6c3f8250fde6', 'd38bab87-d01c-4733-8155-fc538966b25f'),
('8b63354b-4aa0-4f8c-9b91-ccb332b8939c', 0, 'user2', 'user2@test.com', 1, '$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq', true, 1, 0, '127.0.0.1', 'c8bc8937-7e5c-4899-ac4e-7705a1caa71e', 'ef6f13f8-b47c-43aa-bff4-3a920663def0');

-- User Groups
INSERT INTO userGroups(id, ownerUserId, internal, name, description, rating) VALUES
('a517237f-8457-4f87-a38b-de68c274ff24', '6465b186-5fc9-46d2-842f-da8542ba9939', true, '', 'testadmin connections', 1),
('ddedda75-8dd7-477f-9b91-b12efcb25522', '6465b186-5fc9-46d2-842f-da8542ba9939', true, '', 'testadmin friends', 1),
('f2714eb7-e720-4597-b404-6c3f8250fde6', '4115ea77-3c13-4a24-9a2e-701893ee0d61', true, '', 'user connections', 1),
('d38bab87-d01c-4733-8155-fc538966b25f', '4115ea77-3c13-4a24-9a2e-701893ee0d61', true, '', 'user friends', 1),
('c8bc8937-7e5c-4899-ac4e-7705a1caa71e', '8b63354b-4aa0-4f8c-9b91-ccb332b8939c', true, '', 'user2 connections', 1),
('ef6f13f8-b47c-43aa-bff4-3a920663def0', '8b63354b-4aa0-4f8c-9b91-ccb332b8939c', true, '', 'user2 friends', 1);

-- Session Tokens
INSERT INTO sessionTokens(id, refreshtoken, userId, scope, expires) VALUES
('b4b7349b-40f8-40a2-a829-926f5d5f3124', 'd72dbc18-c70b-4f36-987f-51babf406d5a', '6465b186-5fc9-46d2-842f-da8542ba9939', 1, '2025-12-19 13:24:50.321461');

-- User Profiles
INSERT INTO userProfiles(userId, displayName, biography, heroImageUrl, thumbnailImageUrl, tinyImageUrl) VALUES
('6465b186-5fc9-46d2-842f-da8542ba9939', 'Admin', 'A simple admin account', 'https://i.pravatar.cc/512', 'https://i.pravatar.cc/42', 'https://i.pravatar.cc/32'),
('4115ea77-3c13-4a24-9a2e-701893ee0d61', 'User', 'A simple user account', 'https://i.pravatar.cc/512', 'https://i.pravatar.cc/42', 'https://i.pravatar.cc/32'),
('8b63354b-4aa0-4f8c-9b91-ccb332b8939c', 'User 2', 'A simple user account', 'https://i.pravatar.cc/512', 'https://i.pravatar.cc/42', 'https://i.pravatar.cc/32');

-- Connections
INSERT INTO userGroupMembers(userGroupId, userId) VALUES
('a517237f-8457-4f87-a38b-de68c274ff24', '4115ea77-3c13-4a24-9a2e-701893ee0d61'),
('f2714eb7-e720-4597-b404-6c3f8250fde6', '6465b186-5fc9-46d2-842f-da8542ba9939');
                                                      