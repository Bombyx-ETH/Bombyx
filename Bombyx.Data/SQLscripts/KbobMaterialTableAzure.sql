DROP TABLE IF EXISTS [dbo].[KbobMaterial]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KbobMaterial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEnglish] [nchar](200) NULL,
	[NameGerman] [nchar](200) NULL,
	[NameFrench] [nchar](200) NULL,
	[IdKbob] [nchar](12) NULL,
	[IdDisposal] [nchar](10) NULL,
	[Disposal] [nchar](200) NULL,
	[Density] [decimal](10, 2) NULL,
	[DensityUnit] [nchar](10) NULL,
	[Ubp13Embodied] [decimal](18, 6) NULL,
	[Ubp13EoL] [decimal](18, 6) NULL,
	[TotalEmbodied] [decimal](18, 6) NULL,
	[TotalEoL] [decimal](18, 6) NULL,
	[RenewableEmbodied] [decimal](18, 6) NULL,
	[RenewableEoL] [decimal](18, 6) NULL,
	[NonRenewableEmbodied] [decimal](18, 6) NULL,
	[NonRenewableEoL] [decimal](18, 6) NULL,
	[GHGEmbodied] [decimal](18, 6) NULL,
	[GHGEoL] [decimal](18, 6) NULL
) ON [PRIMARY]
GO