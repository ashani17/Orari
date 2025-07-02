-- Add Groups to Existing Schedules
-- This script assigns groups A1, A2, B1, B2 to existing schedules

-- First, let's see what study programs exist
SELECT SPId, SPName FROM StudyPrograms;

-- Update schedules to include groups
-- Assign groups based on course and study program
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

-- Alternative: Assign groups randomly to remaining schedules
UPDATE Schedules 
SET [Group] = CASE 
    WHEN SId % 4 = 0 THEN 'A1'
    WHEN SId % 4 = 1 THEN 'A2'
    WHEN SId % 4 = 2 THEN 'B1'
    WHEN SId % 4 = 3 THEN 'B2'
END
WHERE [Group] IS NULL;

-- Verify the updates
SELECT SId, CId, [Group], Date, StartTime, EndTime 
FROM Schedules 
WHERE [Group] IS NOT NULL
ORDER BY [Group], Date, StartTime;

-- Show count by group
SELECT [Group], COUNT(*) as ScheduleCount
FROM Schedules 
WHERE [Group] IS NOT NULL
GROUP BY [Group]
ORDER BY [Group]; 