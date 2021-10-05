CREATE TABLE [dbo].[Digikalja]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Code] nvarchar(max) not null,
	[AWSessionId] INT NOT NULL,
	[WordId] INT,
	FOREIGN KEY ([WordId]) REFERENCES [dbo].[Words] ([Id]),
	FOREIGN KEY ([AWSessionId]) REFERENCES [dbo].[AWSessions] ([Id]),
)
