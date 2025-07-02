-- Quick Diagnostic for "Informatike" Issue
-- Run this in your database to see what's happening

-- 1. Check what study programs exist
SELECT '=== STUDY PROGRAMS ===' as Info;
SELECT SPId, SPName FROM StudyPrograms ORDER BY SPName;

-- 2. Check for "Informatike" with different spellings
SELECT '=== SEARCHING FOR INFORMATIKE ===' as Info;
SELECT SPId, SPName 
FROM StudyPrograms 
WHERE SPName LIKE '%informatike%' 
   OR SPName LIKE '%Informatike%'
   OR SPName LIKE '%INFORMATIKE%'
   OR SPName LIKE '%informatik%'
   OR SPName LIKE '%Informatik%'
   OR SPName LIKE '%computer%'
   OR SPName LIKE '%Computer%';

-- 3. Check if there are any schedules at all
SELECT '=== TOTAL SCHEDULES ===' as Info;
SELECT COUNT(*) as TotalSchedules FROM Schedules;

-- 4. Check if courses are linked to study programs
SELECT '=== COURSES WITH STUDY PROGRAMS ===' as Info;
SELECT c.CId, c.CName, sp.SPName
FROM Courses c
JOIN StudyProgramCourse spc ON c.CId = spc.CId
JOIN StudyPrograms sp ON spc.SPId = sp.SPId
ORDER BY sp.SPName, c.CName;

-- 5. Check courses that have schedules
SELECT '=== COURSES WITH SCHEDULES ===' as Info;
SELECT DISTINCT c.CId, c.CName
FROM Schedules s
JOIN Courses c ON s.CId = c.CId
ORDER BY c.CName;

-- 6. Check schedules with study program info (this is what the API should return)
SELECT '=== SCHEDULES WITH STUDY PROGRAM INFO ===' as Info;
SELECT s.SId, s.Date, s.StartTime, s.EndTime, 
       c.CName as CourseName,
       sp.SPName as StudyProgramName,
       s.[Group]
FROM Schedules s
JOIN Courses c ON s.CId = c.CId
LEFT JOIN StudyProgramCourse spc ON c.CId = spc.CId
LEFT JOIN StudyPrograms sp ON spc.SPId = sp.SPId
ORDER BY sp.SPName, s.Date, s.StartTime; 