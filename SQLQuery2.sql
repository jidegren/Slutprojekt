delete from Games

INSERT INTO [dbo].[Games] ([ImgURL], [Name], [Description], [MinPlayers], [MaxPlayers], [AltImgText])
VALUES 
(N'Img/games/game1.jpg', N'DigiKalja', N'Bäst på att bluffa vinner!', NULL, NULL, N'En bild på ett spel'),
(N'Img/games/game2.jpg', N'Musikquiz', N'Visa dina musik-skills!', NULL, NULL, N'En bild på ett spel'),
(N'Img/games/game3.jpg', N'Det tredje spelet', N'Ett skojigt spel för After worken!', NULL, NULL, N'En bild på ett spel')
