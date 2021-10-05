CREATE TABLE [dbo].[Games] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ImgURL]      NVARCHAR (MAX) NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [MinPlayers]  INT            NULL,
    [MaxPlayers]  INT            NULL,
    [AltImgText]  nvarchar (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

