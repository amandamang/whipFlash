USE [FlashCards]
GO

SET IDENTITY_INSERT [dbo].[User] ON 
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (1, N'standard', N'user', N'standardUser', N'1234', N'5678', 1)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (2, N'admin', N'user', N'adminUser', N'1234', N'5678', 1)
SET IDENTITY_INSERT [dbo].[User] OFF
GO

