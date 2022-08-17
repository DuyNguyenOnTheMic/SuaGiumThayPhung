DROP PROCEDURE [dbo].[usp_Class_Insert]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Phuong Anh
-- Create date: 2022-04-22
-- Description:	Insert multi-classes
-- =============================================
CREATE PROCEDURE usp_Class_Insert
	@classes [dbo].[ClassInsertedModel] READONLY
AS
BEGIN
	BEGIN TRANSACTION;  
	BEGIN TRY
		--SELECT  SemesterId, MaGocLHP, MaLHP, SoTC, LoaiHP, MaLop, TMSH, SoTietDaXep, PH, Thu, TietBD, SoTiet, 
		--TietHoc, Phong, PH_X, SucChua, 
		--SiSoTKB, Trong, TinhTrangLHP, TuanHoc2, ThuS, TietS, SoSVDK, TuanBD, TuanKT, MaNganh, TenNganh, Note, SubjectId, GETUTCDATE(), GETUTCDATE() FROM @classes

		Insert into dbo.Classes(SemesterId,MaGocLHP,MaLHP,SoTC,LoaiHP,MaLop,TMSH,SoTietDaXep,PH,Thu,TietBD,SoTiet,
		TietHoc,Phong,PH_X,SucChua,
		SiSoTKB,Trong,TinhTrangLHP,TuanHoc2,ThuS,TietS,SoSVDK,TuanBD,TuanKT,MaNganh,TenNganh,Note,SubjectId,CreatedDateTime, LastModifiedDateTime)
		SELECT  SemesterId, MaGocLHP, MaLHP, SoTC, LoaiHP, MaLop, TMSH, SoTietDaXep, PH, Thu, TietBD, SoTiet, 
		TietHoc, Phong, PH_X, SucChua, 
		SiSoTKB, Trong, TinhTrangLHP, TuanHoc2, ThuS, TietS, SoSVDK, TuanBD, TuanKT, MaNganh, TenNganh, Note, SubjectId, GETUTCDATE(), GETUTCDATE() FROM @classes
		SELECT @@ROWCOUNT
	END TRY  
	BEGIN CATCH  
		SELECT  
			ERROR_NUMBER() AS ErrorNumber  
			,ERROR_SEVERITY() AS ErrorSeverity  
			,ERROR_STATE() AS ErrorState  
			,ERROR_PROCEDURE() AS ErrorProcedure  
			,ERROR_LINE() AS ErrorLine  
			,ERROR_MESSAGE() AS ErrorMessage;  
		ROLLBACK TRANSACTION;
	END CATCH  
	IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
