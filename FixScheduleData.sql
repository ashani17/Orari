-- Comprehensive Fix for Schedule Data
-- This script fixes all issues: dates, groups, and study program relationships

-- =====================================================
-- 1. DIAGNOSE CURRENT STATE
-- =====================================================

-- Check current dates
SELECT '=== CURRENT DATES ===' as Info;
SELECT DISTINCT Date FROM Schedules ORDER BY Date;

-- Check study programs
SELECT '=== STUDY PROGRAMS ===' as Info;
SELECT SPId, SPName FROM StudyPrograms ORDER BY SPName;

-- Check if groups exist
SELECT '=== GROUPS STATUS ===' as Info;
SELECT COUNT(*) as SchedulesWithGroups FROM Schedules WHERE [Group] IS NOT NULL;
SELECT COUNT(*) as SchedulesWithoutGroups FROM Schedules WHERE [Group] IS NULL;

-- =====================================================
-- 2. UPDATE DATES TO CURRENT WEEK
-- =====================================================

-- Get current week dates (Monday to Friday)
DECLARE @CurrentDate DATE = GETDATE();
DECLARE @MondayOfWeek DATE = DATEADD(day, -(DATEPART(weekday, @CurrentDate) - 2), @CurrentDate);
DECLARE @TuesdayOfWeek DATE = DATEADD(day, 1, @MondayOfWeek);
DECLARE @WednesdayOfWeek DATE = DATEADD(day, 2, @MondayOfWeek);
DECLARE @ThursdayOfWeek DATE = DATEADD(day, 3, @MondayOfWeek);
DECLARE @FridayOfWeek DATE = DATEADD(day, 4, @MondayOfWeek);

SELECT '=== UPDATING DATES TO CURRENT WEEK ===' as Info;
SELECT @MondayOfWeek as Monday, @TuesdayOfWeek as Tuesday, @WednesdayOfWeek as Wednesday, @ThursdayOfWeek as Thursday, @FridayOfWeek as Friday;

-- Update dates
UPDATE Schedules SET Date = @MondayOfWeek WHERE Date = '2024-01-15';
UPDATE Schedules SET Date = @TuesdayOfWeek WHERE Date = '2024-01-16';
UPDATE Schedules SET Date = @WednesdayOfWeek WHERE Date = '2024-01-17';
UPDATE Schedules SET Date = @ThursdayOfWeek WHERE Date = '2024-01-18';
UPDATE Schedules SET Date = @FridayOfWeek WHERE Date = '2024-01-19';

-- =====================================================
-- 3. ADD GROUPS TO SCHEDULES
-- =====================================================

SELECT '=== ADDING GROUPS ===' as Info;

-- Assign groups based on course IDs (you can adjust this logic)
UPDATE Schedules 
SET [Group] = 'A1'
WHERE CId IN (1, 2, 3, 4, 5) AND [Group] IS NULL;

UPDATE Schedules 
SET [Group] = 'A2'
WHERE CId IN (6, 7, 8, 9, 10) AND [Group] IS NULL;

UPDATE Schedules 
SET [Group] = 'B1'
WHERE CId IN (11, 12, 13, 14, 15) AND [Group] IS NULL;

UPDATE Schedules 
SET [Group] = 'B2'
WHERE CId IN (16, 17, 18, 19, 20) AND [Group] IS NULL;

-- Assign remaining schedules to groups randomly
UPDATE Schedules 
SET [Group] = CASE 
    WHEN SId % 4 = 0 THEN 'A1'
    WHEN SId % 4 = 1 THEN 'A2'
    WHEN SId % 4 = 2 THEN 'B1'
    WHEN SId % 4 = 3 THEN 'B2'
END
WHERE [Group] IS NULL;

-- =====================================================
-- 4. ENSURE STUDY PROGRAM RELATIONSHIPS
-- =====================================================

SELECT '=== CHECKING STUDY PROGRAM RELATIONSHIPS ===' as Info;

-- Check if courses are linked to study programs
SELECT c.CId, c.CName, COUNT(spc.SPId) as StudyProgramCount
FROM Courses c
LEFT JOIN StudyProgramCourse spc ON c.CId = spc.CId
GROUP BY c.CId, c.CName
ORDER BY c.CName;

-- If courses are not linked, we need to link them
-- This assumes you have at least one study program
-- You may need to adjust the SPId based on your actual study program

-- =====================================================
-- 5. VERIFICATION
-- =====================================================

SELECT '=== VERIFICATION ===' as Info;

-- Check updated dates
SELECT 'Updated dates:' as Info;
SELECT DISTINCT Date FROM Schedules ORDER BY Date;

-- Check groups
SELECT 'Groups distribution:' as Info;
SELECT [Group], COUNT(*) as ScheduleCount
FROM Schedules 
WHERE [Group] IS NOT NULL
GROUP BY [Group]
ORDER BY [Group];

-- Check schedules with study program info
SELECT 'Sample schedules with study program info:' as Info;
SELECT TOP 10 s.SId, s.Date, s.StartTime, s.EndTime, 
       c.CName as CourseName,
       sp.SPName as StudyProgramName,
       s.[Group],
       p.FirstName + ' ' + p.LastName as ProfessorName,
       r.RName as RoomName
FROM Schedules s
JOIN Courses c ON s.CId = c.CId
LEFT JOIN StudyProgramCourse spc ON c.CId = spc.CId
LEFT JOIN StudyPrograms sp ON spc.SPId = sp.SPId
LEFT JOIN [User] p ON s.ProfessorId = p.Id
LEFT JOIN Rooms r ON s.RId = r.RId
ORDER BY s.Date, s.StartTime;

-- Check total schedules
SELECT 'Total schedules:' as Info;
SELECT COUNT(*) as TotalSchedules FROM Schedules; 