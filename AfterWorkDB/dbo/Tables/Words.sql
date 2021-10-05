CREATE TABLE [dbo].[Words]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[Word] nvarchar(max) not null,
	[Description] nvarchar(max) not null
)
