DROP PROCEDURE [dbo].[usp_GetSemesterSummaryReport]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_GetSemesterSummaryReport
AS
BEGIN
	--CREATE temp table to cache data
	Select c.SemesterId, c.TeacherId, SUM(c.SoTiet) SoTiet
		INTO #tempTable
		from classes c INNER JOIN Semesters s ON c.SemesterId = s.Id
		WHERE --s.StartYear = @startYear AND s.EndYear = @endYear AND 
		TeacherId is not null
		GROUP BY c.SemesterId, TeacherId

	--SELECT * FROM #tempTable	

	SELECT A.SemesterId, COUNT(TeacherId) AS TotalTeacher
		INTO #tempTeachers
		FROM (
		Select SemesterId, TeacherId from #tempTable
		--group by SemesterId, TeacherId
	) AS A
	GROUP BY A.SemesterId;

	Select SemesterId, SUM(SoTiet) AS SoTiet
		INTO #tempTietHoc
		from #tempTable
		group by SemesterId

	----RETURN result
	SELECT t.SemesterId, s.Name, s.StartYear AS StartYear, s.EndYear AS EndYear, t.TotalTeacher AS TotalTeacher, th.SoTiet AS TotalTietHoc, th.SoTiet AS TotalHour
		FROM #tempTeachers t INNER JOIN #tempTietHoc th ON t.SemesterId = th.SemesterId
			INNER JOIN Semesters s ON t.SemesterId = s.Id

	DROP TABLE #tempTable;	
	DROP TABLE #tempTeachers;
	DROP TABLE #tempTietHoc;
END
GO
-- --Test
 -- TODO: Set parameter values here.
 --EXECUTE [dbo].[usp_GetSemesterSummaryReport] 
 --GO
 /**/
