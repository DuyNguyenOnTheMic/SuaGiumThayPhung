DROP PROCEDURE [dbo].[usp_GetYearlySummaryReport]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_GetYearlySummaryReport
AS
BEGIN
	--CREATE temp table to cache data
	Select s.StartYear, s.EndYear, c.TeacherId, SUM(c.SoTiet) SoTiet
		INTO #tempTable
		from classes c INNER JOIN Semesters s ON c.SemesterId = s.Id
		WHERE TeacherId is not null
		GROUP BY s.StartYear, s.EndYear, TeacherId

	--SELECT * FROM #tempTable	

	SELECT A.StartYear, A.EndYear, COUNT(TeacherId) AS TotalTeacher
		INTO #tempTeachers
		FROM (
		Select StartYear, EndYear, TeacherId from #tempTable
		--group by SemesterId, TeacherId
	) AS A
	GROUP BY A.StartYear, A.EndYear;

	Select StartYear, EndYear, SUM(SoTiet) AS SoTiet
		INTO #tempTietHoc
		from #tempTable
		group by StartYear, EndYear

	----RETURN result
	SELECT  1 AS test, t.StartYear AS StartYear, t.EndYear AS EndYear, t.TotalTeacher AS TotalTeacher, th.SoTiet AS TotalTietHoc, th.SoTiet AS TotalHour
		FROM #tempTeachers t INNER JOIN #tempTietHoc th ON t.StartYear = th.StartYear AND t.EndYear = th.EndYear

	DROP TABLE #tempTable;	
	DROP TABLE #tempTeachers;
	DROP TABLE #tempTietHoc;
END
GO
-- --Test

 ---- TODO: Set parameter values here.
 --EXECUTE [dbo].[usp_GetYearlySummaryReport] 
 --GO
 /**/
