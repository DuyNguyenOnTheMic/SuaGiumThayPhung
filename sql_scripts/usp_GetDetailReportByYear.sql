DROP PROCEDURE [dbo].[usp_GetDetailReportByYear]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_GetDetailReportByYear
	@startYear int, @endYear int
AS
BEGIN
	SELECT SUM(SoTietDaXep) AS TotalHours, TeacherId 
		into #tmp
	FROM classes c inner join semesters s
	ON c.SemesterId = s.Id
		WHERE s.StartYear = @startYear AND s.EndYear = @endYear AND teacherId IS NOT NULL
		GROUP BY teacherId

	SELECT A.TeacherId UserId, IsNull(B.FullName, au.Email) As FullName, A.TotalHours FROM
			#tmp AS A
		INNER JOIN AspNetUsers au ON A.TeacherId = au.Id
		LEFT JOIN UserProfiles B ON au.Id = B.Id

	Drop table #tmp
END
GO
-- --Test

 --DECLARE @startYear int, @endYear int
 --SET @startYear = 2022
 --SET @endYear = 2023

 ---- TODO: Set parameter values here.
 --EXECUTE [dbo].[usp_GetDetailReportByYear] @startYear, @endYear
 --GO
 /**/
