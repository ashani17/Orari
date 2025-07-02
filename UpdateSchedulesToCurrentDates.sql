-- Update Schedule Dates to Current Week
-- This script updates all schedule dates to the current week so filtering works properly

-- First, let's see the current dates
SELECT 'Current dates in database:' as Info;
SELECT DISTINCT Date FROM Schedules ORDER BY Date;

-- Get current week dates (Monday to Friday)
DECLARE @CurrentDate DATE = GETDATE();
DECLARE @MondayOfWeek DATE = DATEADD(day, -(DATEPART(weekday, @CurrentDate) - 2), @CurrentDate);
DECLARE @TuesdayOfWeek DATE = DATEADD(day, 1, @MondayOfWeek);
DECLARE @WednesdayOfWeek DATE = DATEADD(day, 2, @MondayOfWeek);
DECLARE @ThursdayOfWeek DATE = DATEADD(day, 3, @MondayOfWeek);
DECLARE @FridayOfWeek DATE = DATEADD(day, 4, @MondayOfWeek);

-- Show what the new dates will be
SELECT 'New dates will be:' as Info;
SELECT @MondayOfWeek as Monday, @TuesdayOfWeek as Tuesday, @WednesdayOfWeek as Wednesday, @ThursdayOfWeek as Thursday, @FridayOfWeek as Friday;

-- Update Monday schedules (2024-01-15 -> current Monday)
UPDATE Schedules 
SET Date = @MondayOfWeek
WHERE Date = '2024-01-15';

-- Update Tuesday schedules (2024-01-16 -> current Tuesday)
UPDATE Schedules 
SET Date = @TuesdayOfWeek
WHERE Date = '2024-01-16';

-- Update Wednesday schedules (2024-01-17 -> current Wednesday)
UPDATE Schedules 
SET Date = @WednesdayOfWeek
WHERE Date = '2024-01-17';

-- Update Thursday schedules (2024-01-18 -> current Thursday)
UPDATE Schedules 
SET Date = @ThursdayOfWeek
WHERE Date = '2024-01-18';

-- Update Friday schedules (2024-01-19 -> current Friday)
UPDATE Schedules 
SET Date = @FridayOfWeek
WHERE Date = '2024-01-19';

-- Verify the updates
SELECT 'Updated dates:' as Info;
SELECT DISTINCT Date FROM Schedules ORDER BY Date;

-- Show schedules with new dates
SELECT SId, Date, StartTime, EndTime, CId, [Group]
FROM Schedules 
ORDER BY Date, StartTime; 