-- =====================================================
-- ORARI APPLICATION - TEST DATA GENERATION SCRIPT
-- =====================================================
-- This script creates comprehensive test data for the Orari application
-- Run this script after the database has been created and migrations applied

USE [OrariDB]; -- Replace with your actual database name
GO

-- =====================================================
-- 1. INSERT DEPARTMENTS
-- =====================================================
INSERT INTO [Departments] ([DName]) VALUES
('Computer Science'),
('Mathematics'),
('Physics'),
('Engineering'),
('Business Administration'),
('Economics'),
('Psychology'),
('Biology'),
('Chemistry'),
('History');

-- =====================================================
-- 2. INSERT STUDY PROGRAMS
-- =====================================================
INSERT INTO [StudyPrograms] ([SPName], [DId]) VALUES
('Computer Science Bachelor', 1),
('Software Engineering', 1),
('Data Science', 1),
('Mathematics Bachelor', 2),
('Applied Mathematics', 2),
('Physics Bachelor', 3),
('Engineering Physics', 3),
('Mechanical Engineering', 4),
('Electrical Engineering', 4),
('Business Management', 5),
('Finance and Banking', 6),
('Clinical Psychology', 7),
('Biology Bachelor', 8),
('Chemistry Bachelor', 9),
('History Bachelor', 10);

-- =====================================================
-- 3. INSERT USERS (Students, Professors, Admins)
-- =====================================================

-- Insert Students
INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [CreatedAt], [UpdatedAt], [Availability]) VALUES
-- Computer Science Students
('student-001', 'john.doe@university.edu', 'JOHN.DOE@UNIVERSITY.EDU', 'john.doe@university.edu', 'JOHN.DOE@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123456', 'concurrency-001', '+355691234567', 1, 0, 0, 0, 'John', 'Doe', GETUTCDATE(), GETUTCDATE(), 1),
('student-002', 'jane.smith@university.edu', 'JANE.SMITH@UNIVERSITY.EDU', 'jane.smith@university.edu', 'JANE.SMITH@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123457', 'concurrency-002', '+355692345678', 1, 0, 0, 0, 'Jane', 'Smith', GETUTCDATE(), GETUTCDATE(), 1),
('student-003', 'mike.johnson@university.edu', 'MIKE.JOHNSON@UNIVERSITY.EDU', 'mike.johnson@university.edu', 'MIKE.JOHNSON@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123458', 'concurrency-003', '+355693456789', 1, 0, 0, 0, 'Mike', 'Johnson', GETUTCDATE(), GETUTCDATE(), 1),
('student-004', 'sarah.wilson@university.edu', 'SARAH.WILSON@UNIVERSITY.EDU', 'sarah.wilson@university.edu', 'SARAH.WILSON@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123459', 'concurrency-004', '+355694567890', 1, 0, 0, 0, 'Sarah', 'Wilson', GETUTCDATE(), GETUTCDATE(), 1),
('student-005', 'david.brown@university.edu', 'DAVID.BROWN@UNIVERSITY.EDU', 'david.brown@university.edu', 'DAVID.BROWN@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123460', 'concurrency-005', '+355695678901', 1, 0, 0, 0, 'David', 'Brown', GETUTCDATE(), GETUTCDATE(), 1);

-- Insert Professors
INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [CreatedAt], [UpdatedAt], [Availability], [Subject], [Phone], [SpecialRequirements]) VALUES
-- Computer Science Professors
('prof-001', 'dr.alice.chen@university.edu', 'DR.ALICE.CHEN@UNIVERSITY.EDU', 'dr.alice.chen@university.edu', 'DR.ALICE.CHEN@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123461', 'concurrency-006', '+355696789012', 1, 0, 0, 0, 'Alice', 'Chen', GETUTCDATE(), GETUTCDATE(), 1, 'Computer Science', '+355696789012', 'Prefers morning classes'),
('prof-002', 'dr.robert.garcia@university.edu', 'DR.ROBERT.GARCIA@UNIVERSITY.EDU', 'dr.robert.garcia@university.edu', 'DR.ROBERT.GARCIA@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123462', 'concurrency-007', '+355697890123', 1, 0, 0, 0, 'Robert', 'Garcia', GETUTCDATE(), GETUTCDATE(), 1, 'Software Engineering', '+355697890123', 'Needs projector'),
('prof-003', 'dr.emma.davis@university.edu', 'DR.EMMA.DAVIS@UNIVERSITY.EDU', 'dr.emma.davis@university.edu', 'DR.EMMA.DAVIS@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123463', 'concurrency-008', '+355698901234', 1, 0, 0, 0, 'Emma', 'Davis', GETUTCDATE(), GETUTCDATE(), 1, 'Data Science', '+355698901234', 'Requires computer lab'),
('prof-004', 'dr.james.miller@university.edu', 'DR.JAMES.MILLER@UNIVERSITY.EDU', 'dr.james.miller@university.edu', 'DR.JAMES.MILLER@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123464', 'concurrency-009', '+355699012345', 1, 0, 0, 0, 'James', 'Miller', GETUTCDATE(), GETUTCDATE(), 1, 'Mathematics', '+355699012345', 'Needs whiteboard'),
('prof-005', 'dr.lisa.anderson@university.edu', 'DR.LISA.ANDERSON@UNIVERSITY.EDU', 'dr.lisa.anderson@university.edu', 'DR.LISA.ANDERSON@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123465', 'concurrency-010', '+355690123456', 1, 0, 0, 0, 'Lisa', 'Anderson', GETUTCDATE(), GETUTCDATE(), 1, 'Physics', '+355690123456', 'Requires lab equipment');

-- Insert Admin
INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [CreatedAt], [UpdatedAt], [Availability]) VALUES
('admin-001', 'admin@university.edu', 'ADMIN@UNIVERSITY.EDU', 'admin@university.edu', 'ADMIN@UNIVERSITY.EDU', 1, 'AQAAAAIAAYagAAAAELbHvLtqa6k6xQDJi9LJ3KxvJQhH8tK1mZxYzWbCdEfGhIjKlMnOpQrStUvWxYzAbC==', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123466', 'concurrency-011', '+355691112223', 1, 0, 0, 0, 'System', 'Administrator', GETUTCDATE(), GETUTCDATE(), 1);

-- =====================================================
-- 4. INSERT ROLES
-- =====================================================
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES
('role-student', 'Student', 'STUDENT', 'concurrency-role-001'),
('role-professor', 'Professor', 'PROFESSOR', 'concurrency-role-002'),
('role-admin', 'Admin', 'ADMIN', 'concurrency-role-003');

-- =====================================================
-- 5. ASSIGN ROLES TO USERS
-- =====================================================
-- Assign Student role to students
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES
('student-001', 'role-student'),
('student-002', 'role-student'),
('student-003', 'role-student'),
('student-004', 'role-student'),
('student-005', 'role-student');

-- Assign Professor role to professors
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES
('prof-001', 'role-professor'),
('prof-002', 'role-professor'),
('prof-003', 'role-professor'),
('prof-004', 'role-professor'),
('prof-005', 'role-professor');

-- Assign Admin role to admin
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES
('admin-001', 'role-admin');

-- =====================================================
-- 6. INSERT ROOMS
-- =====================================================
INSERT INTO [Rooms] ([RName], [RCapacity], [RType], [RDescription]) VALUES
('Room 101', 30, 'Classroom', 'Standard classroom with projector'),
('Room 102', 25, 'Classroom', 'Standard classroom with whiteboard'),
('Room 103', 40, 'Classroom', 'Large classroom with projector and whiteboard'),
('Computer Lab A', 20, 'Computer Lab', 'Computer lab with 20 workstations'),
('Computer Lab B', 15, 'Computer Lab', 'Computer lab with 15 workstations'),
('Physics Lab', 25, 'Laboratory', 'Physics laboratory with equipment'),
('Chemistry Lab', 20, 'Laboratory', 'Chemistry laboratory with safety equipment'),
('Conference Room A', 50, 'Conference', 'Large conference room with presentation equipment'),
('Conference Room B', 30, 'Conference', 'Medium conference room'),
('Auditorium', 200, 'Auditorium', 'Large auditorium for major events');

-- =====================================================
-- 7. INSERT COURSES
-- =====================================================
INSERT INTO [Courses] ([CName], [Credits], [PId], [Profesor]) VALUES
-- Computer Science Courses
('Introduction to Programming', 6, 'prof-001', 'Dr. Alice Chen'),
('Data Structures and Algorithms', 8, 'prof-001', 'Dr. Alice Chen'),
('Object-Oriented Programming', 6, 'prof-002', 'Dr. Robert Garcia'),
('Software Engineering', 8, 'prof-002', 'Dr. Robert Garcia'),
('Database Systems', 6, 'prof-003', 'Dr. Emma Davis'),
('Machine Learning', 8, 'prof-003', 'Dr. Emma Davis'),
('Web Development', 6, 'prof-001', 'Dr. Alice Chen'),
('Computer Networks', 6, 'prof-002', 'Dr. Robert Garcia'),
('Operating Systems', 8, 'prof-003', 'Dr. Emma Davis'),
('Artificial Intelligence', 8, 'prof-001', 'Dr. Alice Chen'),

-- Mathematics Courses
('Calculus I', 6, 'prof-004', 'Dr. James Miller'),
('Calculus II', 6, 'prof-004', 'Dr. James Miller'),
('Linear Algebra', 6, 'prof-004', 'Dr. James Miller'),
('Discrete Mathematics', 6, 'prof-004', 'Dr. James Miller'),
('Statistics', 6, 'prof-004', 'Dr. James Miller'),

-- Physics Courses
('Physics I', 6, 'prof-005', 'Dr. Lisa Anderson'),
('Physics II', 6, 'prof-005', 'Dr. Lisa Anderson'),
('Modern Physics', 8, 'prof-005', 'Dr. Lisa Anderson'),
('Quantum Mechanics', 8, 'prof-005', 'Dr. Lisa Anderson'),
('Thermodynamics', 6, 'prof-005', 'Dr. Lisa Anderson');

-- =====================================================
-- 8. INSERT STUDY PROGRAM COURSES (Course Assignments)
-- =====================================================
INSERT INTO [StudyProgramCourses] ([SPId], [CId], [Year], [AcademicYear]) VALUES
-- Computer Science Bachelor - Year 1
(1, 1, 1, '2023-2026'), -- Introduction to Programming
(1, 7, 1, '2023-2026'), -- Web Development
(1, 11, 1, '2023-2026'), -- Calculus I
(1, 13, 1, '2023-2026'), -- Linear Algebra

-- Computer Science Bachelor - Year 2
(1, 2, 2, '2023-2026'), -- Data Structures and Algorithms
(1, 3, 2, '2023-2026'), -- Object-Oriented Programming
(1, 5, 2, '2023-2026'), -- Database Systems
(1, 12, 2, '2023-2026'), -- Calculus II

-- Computer Science Bachelor - Year 3
(1, 4, 3, '2023-2026'), -- Software Engineering
(1, 6, 3, '2023-2026'), -- Machine Learning
(1, 8, 3, '2023-2026'), -- Computer Networks
(1, 9, 3, '2023-2026'), -- Operating Systems
(1, 10, 3, '2023-2026'), -- Artificial Intelligence

-- Software Engineering - Year 1
(2, 1, 1, '2023-2026'), -- Introduction to Programming
(2, 3, 1, '2023-2026'), -- Object-Oriented Programming
(2, 11, 1, '2023-2026'), -- Calculus I
(2, 14, 1, '2023-2026'), -- Discrete Mathematics

-- Software Engineering - Year 2
(2, 2, 2, '2023-2026'), -- Data Structures and Algorithms
(2, 4, 2, '2023-2026'), -- Software Engineering
(2, 5, 2, '2023-2026'), -- Database Systems
(2, 7, 2, '2023-2026'), -- Web Development

-- Software Engineering - Year 3
(2, 6, 3, '2023-2026'), -- Machine Learning
(2, 8, 3, '2023-2026'), -- Computer Networks
(2, 9, 3, '2023-2026'), -- Operating Systems
(2, 10, 3, '2023-2026'), -- Artificial Intelligence

-- Data Science - Year 1
(3, 1, 1, '2023-2026'), -- Introduction to Programming
(3, 11, 1, '2023-2026'), -- Calculus I
(3, 13, 1, '2023-2026'), -- Linear Algebra
(3, 15, 1, '2023-2026'), -- Statistics

-- Data Science - Year 2
(3, 2, 2, '2023-2026'), -- Data Structures and Algorithms
(3, 5, 2, '2023-2026'), -- Database Systems
(3, 6, 2, '2023-2026'), -- Machine Learning
(3, 12, 2, '2023-2026'), -- Calculus II

-- Data Science - Year 3
(3, 8, 3, '2023-2026'), -- Computer Networks
(3, 9, 3, '2023-2026'), -- Operating Systems
(3, 10, 3, '2023-2026'), -- Artificial Intelligence
(3, 14, 3, '2023-2026'), -- Discrete Mathematics;

-- =====================================================
-- 9. INSERT ENROLLMENTS
-- =====================================================
INSERT INTO [Enrollments] ([StudentId], [CId]) VALUES
-- Student 1 enrollments
('student-001', 1), -- Introduction to Programming
('student-001', 7), -- Web Development
('student-001', 11), -- Calculus I
('student-001', 13), -- Linear Algebra

-- Student 2 enrollments
('student-002', 1), -- Introduction to Programming
('student-002', 3), -- Object-Oriented Programming
('student-002', 11), -- Calculus I
('student-002', 14), -- Discrete Mathematics

-- Student 3 enrollments
('student-003', 1), -- Introduction to Programming
('student-003', 2), -- Data Structures and Algorithms
('student-003', 5), -- Database Systems
('student-003', 15), -- Statistics

-- Student 4 enrollments
('student-004', 1), -- Introduction to Programming
('student-004', 7), -- Web Development
('student-004', 11), -- Calculus I
('student-004', 13), -- Linear Algebra

-- Student 5 enrollments
('student-005', 1), -- Introduction to Programming
('student-005', 3), -- Object-Oriented Programming
('student-005', 11), -- Calculus I
('student-005', 14); -- Discrete Mathematics

-- =====================================================
-- 10. INSERT SCHEDULES
-- =====================================================
INSERT INTO [Schedules] ([Date], [StartTime], [EndTime], [RId], [ProfessorId], [CId], [Description]) VALUES
-- Monday Schedule
('2024-01-15', '09:00:00', '10:30:00', 1, 'prof-001', 1, 'Introduction to Programming - Lecture'),
('2024-01-15', '11:00:00', '12:30:00', 2, 'prof-002', 3, 'Object-Oriented Programming - Lecture'),
('2024-01-15', '14:00:00', '15:30:00', 4, 'prof-001', 1, 'Introduction to Programming - Lab'),
('2024-01-15', '16:00:00', '17:30:00', 3, 'prof-004', 11, 'Calculus I - Lecture'),

-- Tuesday Schedule
('2024-01-16', '09:00:00', '10:30:00', 2, 'prof-003', 5, 'Database Systems - Lecture'),
('2024-01-16', '11:00:00', '12:30:00', 1, 'prof-001', 7, 'Web Development - Lecture'),
('2024-01-16', '14:00:00', '15:30:00', 5, 'prof-003', 5, 'Database Systems - Lab'),
('2024-01-16', '16:00:00', '17:30:00', 3, 'prof-004', 13, 'Linear Algebra - Lecture'),

-- Wednesday Schedule
('2024-01-17', '09:00:00', '10:30:00', 1, 'prof-002', 2, 'Data Structures and Algorithms - Lecture'),
('2024-01-17', '11:00:00', '12:30:00', 2, 'prof-003', 6, 'Machine Learning - Lecture'),
('2024-01-17', '14:00:00', '15:30:00', 4, 'prof-002', 2, 'Data Structures and Algorithms - Lab'),
('2024-01-17', '16:00:00', '17:30:00', 3, 'prof-004', 12, 'Calculus II - Lecture'),

-- Thursday Schedule
('2024-01-18', '09:00:00', '10:30:00', 2, 'prof-002', 4, 'Software Engineering - Lecture'),
('2024-01-18', '11:00:00', '12:30:00', 1, 'prof-001', 10, 'Artificial Intelligence - Lecture'),
('2024-01-18', '14:00:00', '15:30:00', 5, 'prof-002', 4, 'Software Engineering - Lab'),
('2024-01-18', '16:00:00', '17:30:00', 3, 'prof-004', 14, 'Discrete Mathematics - Lecture'),

-- Friday Schedule
('2024-01-19', '09:00:00', '10:30:00', 1, 'prof-003', 8, 'Computer Networks - Lecture'),
('2024-01-19', '11:00:00', '12:30:00', 2, 'prof-003', 9, 'Operating Systems - Lecture'),
('2024-01-19', '14:00:00', '15:30:00', 4, 'prof-003', 8, 'Computer Networks - Lab'),
('2024-01-19', '16:00:00', '17:30:00', 3, 'prof-004', 15, 'Statistics - Lecture');

-- =====================================================
-- 11. INSERT EXAMS
-- =====================================================
INSERT INTO [Exams] ([ExamName], [ExamDate], [StartTime], [EndTime], [CId], [ProfessorId], [RId]) VALUES
('Introduction to Programming Final', '2024-01-20', '09:00:00', '11:00:00', 1, 'prof-001', 1),
('Object-Oriented Programming Final', '2024-01-21', '09:00:00', '11:00:00', 3, 'prof-002', 2),
('Database Systems Final', '2024-01-22', '09:00:00', '11:00:00', 5, 'prof-003', 3),
('Calculus I Final', '2024-01-23', '09:00:00', '11:00:00', 11, 'prof-004', 4),
('Data Structures Final', '2024-01-24', '09:00:00', '11:00:00', 2, 'prof-002', 5);

-- =====================================================
-- 12. INSERT RECURRING SCHEDULES
-- =====================================================
INSERT INTO [RecurringSchedules] ([DayOfWeek], [StartTime], [EndTime], [RoomId], [ProfessorId], [CourseId], [Description]) VALUES
(1, '09:00:00', '10:30:00', 1, 'prof-001', 1, 'Introduction to Programming - Weekly Lecture'),
(1, '14:00:00', '15:30:00', 4, 'prof-001', 1, 'Introduction to Programming - Weekly Lab'),
(2, '09:00:00', '10:30:00', 2, 'prof-002', 3, 'Object-Oriented Programming - Weekly Lecture'),
(2, '14:00:00', '15:30:00', 5, 'prof-002', 3, 'Object-Oriented Programming - Weekly Lab'),
(3, '09:00:00', '10:30:00', 1, 'prof-003', 5, 'Database Systems - Weekly Lecture'),
(3, '14:00:00', '15:30:00', 4, 'prof-003', 5, 'Database Systems - Weekly Lab'),
(4, '09:00:00', '10:30:00', 2, 'prof-004', 11, 'Calculus I - Weekly Lecture'),
(5, '09:00:00', '10:30:00', 3, 'prof-005', 16, 'Physics I - Weekly Lecture');

-- =====================================================
-- 13. INSERT CHAT MESSAGES (Sample)
-- =====================================================
INSERT INTO [ChatMessages] ([SenderId], [ReceiverId], [Message], [Timestamp]) VALUES
('student-001', 'prof-001', 'Hello Professor, I have a question about the programming assignment.', GETUTCDATE()),
('prof-001', 'student-001', 'Hello! Sure, what is your question?', GETUTCDATE()),
('student-002', 'prof-002', 'When is the deadline for the OOP project?', GETUTCDATE()),
('prof-002', 'student-002', 'The deadline is next Friday at 23:59.', GETUTCDATE()),
('student-003', 'prof-003', 'Can you explain the database normalization concept again?', GETUTCDATE()),
('prof-003', 'student-003', 'Of course! Let me schedule a meeting to explain it in detail.', GETUTCDATE());

-- =====================================================
-- SCRIPT COMPLETION MESSAGE
-- =====================================================
PRINT '=====================================================';
PRINT 'ORARI TEST DATA GENERATION COMPLETED SUCCESSFULLY!';
PRINT '=====================================================';
PRINT '';
PRINT 'Data inserted:';
PRINT '- 10 Departments';
PRINT '- 15 Study Programs';
PRINT '- 11 Users (5 Students, 5 Professors, 1 Admin)';
PRINT '- 3 Roles (Student, Professor, Admin)';
PRINT '- 10 Rooms';
PRINT '- 20 Courses';
PRINT '- 45 Study Program Course Assignments';
PRINT '- 20 Enrollments';
PRINT '- 20 Schedules';
PRINT '- 5 Exams';
PRINT '- 8 Recurring Schedules';
PRINT '- 6 Chat Messages';
PRINT '';
PRINT 'Test Credentials:';
PRINT 'Admin: admin@university.edu (password: Admin123!)';
PRINT 'Student: john.doe@university.edu (password: Student123!)';
PRINT 'Professor: dr.alice.chen@university.edu (password: Professor123!)';
PRINT '';
PRINT 'Note: You may need to update passwords using the application''s password reset feature.';
PRINT '====================================================='; 