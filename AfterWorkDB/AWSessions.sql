CREATE TABLE [dbo].[AWSessions]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[AWName] nvarchar(max) not null,
	[Code] nvarchar(max) not null,	
	[CreatorId] nvarchar(450) not null,
    FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
)
