-- =========================
-- RESET DATA
-- =========================
DELETE FROM Shifts;
DELETE FROM Users;

-- reset auto increment
DELETE FROM sqlite_sequence WHERE name='Users';
DELETE FROM sqlite_sequence WHERE name='Shifts';


-- =========================
-- SEED USERS (20 users)
-- =========================
INSERT INTO Users (Name, Email, Password, Role, IsFirstLogin) VALUES
-- Admin
('System Admin', 'admin@shiboo.com', 'admin', 'Admin', 0),

-- Members
('Nguyễn Văn A', 'user1@shiboo.com', '123456', 'Member', 1),
('Trần Thị B', 'user2@shiboo.com', '123456', 'Member', 1),
('Lê Văn C', 'user3@shiboo.com', '123456', 'Member', 1),
('Phạm Thị D', 'user4@shiboo.com', '123456', 'Member', 1),
('Hoàng Văn E', 'user5@shiboo.com', '123456', 'Member', 1),
('Đặng Thị F', 'user6@shiboo.com', '123456', 'Member', 1),
('Bùi Văn G', 'user7@shiboo.com', '123456', 'Member', 1),
('Đỗ Thị H', 'user8@shiboo.com', '123456', 'Member', 1),
('Ngô Văn I', 'user9@shiboo.com', '123456', 'Member', 1),
('Vũ Thị J', 'user10@shiboo.com', '123456', 'Member', 1),

('Phan Văn K', 'user11@shiboo.com', '123456', 'Member', 1),
('Trịnh Thị L', 'user12@shiboo.com', '123456', 'Member', 1),
('Mai Văn M', 'user13@shiboo.com', '123456', 'Member', 1),
('Lý Thị N', 'user14@shiboo.com', '123456', 'Member', 1),
('Tạ Văn O', 'user15@shiboo.com', '123456', 'Member', 1),
('Cao Thị P', 'user16@shiboo.com', '123456', 'Member', 1),
('Hồ Văn Q', 'user17@shiboo.com', '123456', 'Member', 1),
('Dương Thị R', 'user18@shiboo.com', '123456', 'Member', 1),
('Kiều Văn S', 'user19@shiboo.com', '123456', 'Member', 1),
('Lương Thị T', 'user20@shiboo.com', '123456', 'Member', 1);


-- =========================
-- SEED SHIFTS
-- =========================
INSERT INTO Shifts (UserId, Date, ShiftName, Note, Status, RequestType) VALUES

-- ===== CA CHÍNH =====
(2, '2026-04-25', 'Ca 1', '', 'Approved', 'Assign'),
(3, '2026-04-25', 'Ca 2', '', 'Approved', 'Assign'),
(4, '2026-04-25', 'Ca 3', '', 'Approved', 'Assign'),

(5, '2026-04-26', 'Ca 1', '', 'Approved', 'Assign'),
(6, '2026-04-26', 'Ca 2', '', 'Approved', 'Assign'),
(7, '2026-04-26', 'Ca 3', '', 'Approved', 'Assign'),

(8, '2026-04-27', 'Ca 1', '', 'Approved', 'Assign'),
(9, '2026-04-27', 'Ca 2', '', 'Approved', 'Assign'),
(10, '2026-04-27', 'Ca 3', '', 'Approved', 'Assign'),

-- ===== REQUEST ĐỔI CA =====
(2, '2026-04-25', 'Ca 1', 'Ca 2', 'Pending', 'Change'),
(3, '2026-04-25', 'Ca 2', 'Ca 3', 'Pending', 'Change'),
(4, '2026-04-25', 'Ca 3', 'Ca 1', 'Pending', 'Change'),

-- ===== REQUEST MỚI =====
(11, '2026-04-28', 'Ca 1', '', 'Pending', 'Request'),
(12, '2026-04-28', 'Ca 2', '', 'Pending', 'Request'),
(13, '2026-04-28', 'Ca 3', '', 'Pending', 'Request'),

-- ===== REJECT =====
(14, '2026-04-29', 'Ca 1', 'Ca 2', 'Rejected', 'Change'),
(15, '2026-04-29', 'Ca 2', 'Ca 3', 'Rejected', 'Change'),

-- ===== RANDOM =====
(16, '2026-04-30', 'Ca 1', '', 'Approved', 'Assign'),
(17, '2026-04-30', 'Ca 2', '', 'Approved', 'Assign'),
(18, '2026-04-30', 'Ca 3', '', 'Approved', 'Assign');