DROP PROCEDURE [dbo].[usp_RemoveUser]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_RemoveUser
	@userId varchar(50)
AS
BEGIN
	DECLARE @totalRowAffected int = 0;

	BEGIN TRANSACTION;  

	BEGIN TRY  
		--delete user roles
		DELETE ur
		FROM AspNetUserRoles ur WHERE UserId = @userId;
		SET @totalRowAffected = @totalRowAffected + @@ROWCOUNT
		--delete user profiles
		DELETE ur
		FROM UserProfiles ur WHERE Id = @userId;
		SET @totalRowAffected = @totalRowAffected + @@ROWCOUNT
		--delete users
		DELETE ur
		FROM AspNetusers ur WHERE Id = @userId;
		SET @totalRowAffected = @totalRowAffected + @@ROWCOUNT
		select @totalRowAffected;
	END TRY  
	BEGIN CATCH  		
		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		SELECT -1 As TotalRowsAffected
	END CATCH;  

	IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;    
	
END
-- --Test

 --DECLARE @userId varchar(50)
 --SET @userId =  '2c6a1bd9-0a76-4aef-8a50-afec037984d3'
 ---- TODO: Set parameter values here.
 --EXECUTE [dbo].[usp_RemoveUser] @userId
 --GO
 /**/
