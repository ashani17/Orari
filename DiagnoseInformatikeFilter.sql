-- Diagnose "Informatike" Filter Issue
-- This script helps identify why filtering by "Informatike" doesn't work

-- 1. Check all study programs
SELECT '=== ALL STUDY PROGRAMS ===' as Info;
SELECT SPId, SPName, DId 
FROM StudyPrograms 
ORDER BY SPName;

-- 2. Check for "Informatike" specifically (case insensitive)
SELECT '=== SEARCHING FOR INFORMATIKE ===' as Info;
SELECT SPId, SPName 
FROM StudyPrograms 
WHERE SPName LIKE '%informatike%' 
   OR SPName LIKE '%Informatike%'
   OR SPName LIKE '%INFORMATIKE%'
   OR SPName LIKE '%informatik%'
   OR SPName LIKE '%Informatik%';

-- 3. Check study program courses relationship
SELECT '=== STUDY PROGRAM COURSES ===' as Info;
SELECT spc.Id, spc.SPId, sp.SPName, spc.CId, c.CName, spc.Year, spc.AcademicYear
FROM StudyProgramCourse spc
JOIN StudyPrograms sp ON spc.SPId = sp.SPId
JOIN Courses c ON spc.CId = c.CId
ORDER BY sp.SPName, spc.Year, c.CName;

-- 4. Check schedules with study program info (this is what the API returns)
SELECT '=== SCHEDULES WITH STUDY PROGRAM INFO ===' as Info;
SELECT s.SId, s.Date, s.StartTime, s.EndTime, 
       c.CName as CourseName,
       sp.SPName as StudyProgramName,
       spc.Year, spc.AcademicYear,
       s.[Group],
       p.FirstName + ' ' + p.LastName as ProfessorName,
       r.RName as RoomName
FROM Schedules s
JOIN Courses c ON s.CId = c.CId
JOIN StudyProgramCourse spc ON c.CId = spc.CId
JOIN StudyPrograms sp ON spc.SPId = sp.SPId
LEFT JOIN [User] p ON s.ProfessorId = p.Id
LEFT JOIN Rooms r ON s.RId = r.RId
ORDER BY sp.SPName, s.Date, s.StartTime;

-- 5. Check schedules count by study program
SELECT '=== SCHEDULE COUNT BY STUDY PROGRAM ===' as Info;
SELECT sp.SPName, COUNT(s.SId) as ScheduleCount
FROM StudyPrograms sp
LEFT JOIN StudyProgramCourse spc ON sp.SPId = spc.SPId
LEFT JOIN Schedules s ON spc.CId = s.CId
GROUP BY sp.SPName
ORDER BY sp.SPName;

-- 6. Check if there are any schedules at all
SELECT '=== TOTAL SCHEDULES ===' as Info;
SELECT COUNT(*) as TotalSchedules FROM Schedules;

-- 7. Check courses that have schedules
SELECT '=== COURSES WITH SCHEDULES ===' as Info;
SELECT DISTINCT c.CId, c.CName
FROM Schedules s
JOIN Courses c ON s.CId = c.CId
ORDER BY c.CName;

-- 8. Check if courses are linked to study programs
SELECT '=== COURSES LINKED TO STUDY PROGRAMS ===' as Info;
SELECT c.CId, c.CName, sp.SPName
FROM Courses c
JOIN StudyProgramCourse spc ON c.CId = spc.CId
JOIN StudyPrograms sp ON spc.SPId = sp.SPId
ORDER BY sp.SPName, c.CName;

-- 9. Check courses that are NOT linked to study programs
SELECT '=== COURSES NOT LINKED TO STUDY PROGRAMS ===' as Info;
SELECT c.CId, c.CName
FROM Courses c
LEFT JOIN StudyProgramCourse spc ON c.CId = spc.CId
WHERE spc.CId IS NULL
ORDER BY c.CName; 