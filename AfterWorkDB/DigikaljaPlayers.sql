CREATE TABLE [dbo].[DigikaljaPlayers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] NVARCHAR(450) NOT NULL,
	[DigikaljaId] INT NOT NULL,
	[Points] INT,
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	FOREIGN KEY ([DigikaljaId]) REFERENCES [dbo].[Digikalja] ([Id]),
)
