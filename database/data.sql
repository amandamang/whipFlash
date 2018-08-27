USE [FlashCards]
GO

SET IDENTITY_INSERT [dbo].[Role] ON 
INSERT [dbo].[Role] ([Id], [Name]) VALUES (1, N'Standard')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (2, N'Admin')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO

SET IDENTITY_INSERT [dbo].[User] ON 
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (1, N'Jimi', N'Tempfli', N'jimitempfli', N'1234', N'5678', 1)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [UserName], [Password], [Salt], [RoleId]) VALUES (2, N'Jimi', N'Tempfli', N'jimitempfliadmin', N'1234', N'5678', 1)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
