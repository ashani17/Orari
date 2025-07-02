-- Check Study Programs and Related Data
-- This script helps diagnose the "Informatike" filter issue

-- 1. Check all study programs
SELECT SPId, SPName, DId 
FROM StudyPrograms 
ORDER BY SPName;

-- 2. Check study program courses
SELECT spc.Id, spc.SPId, sp.SPName, spc.CId, c.CName, spc.Year, spc.AcademicYear
FROM StudyProgramCourse spc
JOIN StudyPrograms sp ON spc.SPId = sp.SPId
JOIN Courses c ON spc.CId = c.CId
ORDER BY sp.SPName, spc.Year, c.CName;

-- 3. Check schedules with study program info
SELECT s.SId, s.Date, s.StartTime, s.EndTime, 
       c.CName as CourseName,
       sp.SPName as StudyProgramName,
       spc.Year, spc.AcademicYear,
       s.[Group]
FROM Schedules s
JOIN Courses c ON s.CId = c.CId
JOIN StudyProgramCourse spc ON c.CId = spc.CId
JOIN StudyPrograms sp ON spc.SPId = sp.SPId
ORDER BY sp.SPName, s.Date, s.StartTime;

-- 4. Check if "Informatike" exists (case insensitive)
SELECT SPId, SPName 
FROM StudyPrograms 
WHERE SPName LIKE '%informatike%' OR SPName LIKE '%Informatike%';

-- 5. Show all unique study program names
SELECT DISTINCT SPName 
FROM StudyPrograms 
ORDER BY SPName;

-- 6. Check schedules count by study program
SELECT sp.SPName, COUNT(s.SId) as ScheduleCount
FROM StudyPrograms sp
LEFT JOIN StudyProgramCourse spc ON sp.SPId = spc.SPId
LEFT JOIN Schedules s ON spc.CId = s.CId
GROUP BY sp.SPName
ORDER BY sp.SPName; 