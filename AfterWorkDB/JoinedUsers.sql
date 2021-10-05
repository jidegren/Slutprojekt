CREATE TABLE [dbo].[JoinedUsers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] NVARCHAR(450) NOT NULL,
	[AWSessionId] INT NOT NULL,
	[Points] INT,
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	FOREIGN KEY ([AWSessionId]) REFERENCES [dbo].[AWSessions] ([Id]),
)
