USE [Lernperiode_5]
GO

/****** Object:  Table [dbo].[Cookie_Clicker_Score]    Script Date: 02.05.2025 11:22:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cookie_Clicker_Score](
	[Cookies] [int] NOT NULL,
	[cookiesPerClick] [int] NOT NULL,
	[clickPowerCost] [int] NOT NULL,
	[autoClickers] [int] NOT NULL,
	[autoClickerCost] [int] NOT NULL,
	[clickPowerUpgrades] [int] NOT NULL,
	[rebirths] [int] NOT NULL,
	[rebirthCost] [int] NOT NULL
) ON [PRIMARY]
GO

