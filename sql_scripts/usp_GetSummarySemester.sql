DROP PROCEDURE [dbo].[usp_GetSummarySemester]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_GetSummarySemester
AS
BEGIN
	--get all semesters
	CREATE TABLE #result(
		[SemesterId] [int],
		[Name] [nvarchar](max),
		[StartYear] [int],
		[EndYear] [int],
		[ListWeek] [nvarchar](max),
		[StartTime] [datetime] NOT NULL,
		[TotalSubject] INT,
		TotalAssignedClass INT
	)
	INSERT INTO #result(SemesterId, Name, StartYear, EndYear, ListWeek, StartTime, TotalSubject, TotalAssignedClass)
	SELECT Id, Name, StartYear, EndYear, ListWeek, StartTime, 0, 0 
		FROM Semesters
	--get all assigned class
	;WITH assigned_class(
		SemesterId, totalAssignedClass
	)
	AS (
		SELECT SemesterId, COUNT(1) FROM Classes
			WHERE TeacherId IS NOT NULL
			GROUP BY SemesterId
	)
	UPDATE tmp
	SET tmp.TotalAssignedClass = b.totalAssignedClass
	FROM #result tmp
	INNER JOIN assigned_class b ON tmp.SemesterId = b.SemesterId
	--get all subjects
	;WITH total_subject(
		SemesterId, TotalSubject
	)
	AS (
		SELECT SemesterId, COUNT(1) AS TotalSubject
		 FROM (
			SELECT SemesterId, SubjectId FROM Classes
				GROUP BY SemesterId, SubjectId) A
			GROUP BY A.SemesterId
	)
	UPDATE tmp
	SET tmp.TotalSubject = b.TotalSubject
	FROM #result tmp
	INNER JOIN total_subject b ON tmp.SemesterId = b.SemesterId

	----RETURN result
	SELECT  *
		FROM #result

	DROP TABLE #result;	

END
GO
-- --Test

 ---- TODO: Set parameter values here.
 --EXECUTE [dbo].[usp_GetSummarySemester] 
 --GO
 /**/
