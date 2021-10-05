CREATE TABLE [dbo].[UserInfo] (
    [ID] INT PRIMARY KEY IDENTITY NOT NULL,
    [UserID]      NVARCHAR (450) NOT NULL,
    [TotalPoints] INT            NULL,
    [GamesPlayed] INT            NULL,
    [ProfileImgPath] NVARCHAR(MAX) NULL,
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id]),
);


    

