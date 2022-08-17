BEGIN TRANSACTION;  

BEGIN TRY  
    -- Generate a divide-by-zero error.  
    CREATE TABLE #Subjects(
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(200)
    );

    CREATE TABLE #CapWeeks(
        Id INT IDENTITY PRIMARY KEY,
        CapWeekDate DATETIME2,
        SubjectId INT        
    );

    CREATE TABLE #CapDays(
        Id INT IDENTITY PRIMARY KEY,
        DayOfWeek DATETIME2,
        CapWeekId INT        
    );
    -- 1,2,..15
    CREATE TABLE #CapTimes(
        Id INT IDENTITY PRIMARY KEY,
        TimeOrder INT,
        CapDayId INT        
    );

    INSERT INTO #Subjects(Name)
    VALUES('Phan tich thiet ke huong doi tuong 2021')
    ,('Phan tich thiet ke huong doi tuong 2022')

    --auto insert weeks, day after getting subjects
    INSERT INTO #CapWeeks(CapWeekDate, SubjectId)
    VALUES('2021-11-29', 1)
    ,('2021-12-06', 1)
    ,('2021-12-13', 1)

    INSERT INTO #CapDays(DayOfWeek, CapWeekId)
    VALUES('2021-11-29', 1),
    ('2021-11-30', 1),
    ('2021-12-01', 1),
    ('2021-12-02', 1),
    ('2021-12-03', 1),
    ('2021-12-04', 1)
    ,('2021-12-05', 1)
    ,('2021-12-06', 1)
    ,('2021-12-07', 1)

    INSERT INTO #CapTimes(TimeOrder, CapDayId)
    VALUES(2, 1),(3, 1),(4, 1),(2, 5),(3, 5),(5, 5),(6, 5)

    SELECT s.Name, d.DayOfWeek, t.TimeOrder FROM 
    #Subjects s INNER JOIN
    #CapWeeks w ON s.Id = w.Id
    INNER JOIN #CapDays d ON w.Id = d.CapWeekId
    INNER JOIN #CapTimes t ON d.Id = t.CapDayId
    ORDER BY s.Name, d.DayOfWeek, t.TimeOrder


    DROP TABLE #CapTimes;
    DROP TABLE #CapDays;
    DROP TABLE #CapWeeks;
    DROP TABLE #Subjects;
END TRY  
BEGIN CATCH  
    SELECT   
        ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_LINE() AS ErrorLine  
        ,ERROR_MESSAGE() AS ErrorMessage;  

    IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
END CATCH;  

IF @@TRANCOUNT > 0  
    COMMIT TRANSACTION;  
GO  