INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name]
           ,[Discriminator]
           ,[ApplicationUser_Id])
     VALUES
           ('35b57c6e-dacc-4be1-ad61-a7f678b9854d',N'Giảng viên','ApplicationRole', NULL),
		   ('7a23a79a-937e-4da6-9196-d0261d5a9a44',N'Chưa Phân Quyền','ApplicationRole', NULL),
		   ('93204e9f-bb17-4133-ad69-5598475f407c',N'BCN Khoa','ApplicationRole', NULL),
		   ('ab165e8b-7073-42e4-ac71-a3a817db8ced',N'Admin','ApplicationRole', NULL),
		   ('ea2d1f58-610d-45bb-be52-094a4f2b6714',N'Bộ môn','ApplicationRole', NULL)
GO

