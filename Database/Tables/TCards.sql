/****** Object:  Table [dbo].[TCards]    Script Date: 06/03/2025 14:08:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TCards]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TCards](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL,
	[Balance] [money] NOT NULL,
	[Limit] [money] NOT NULL,
	[Active] [bit] NOT NULL,
	[Elegible] [bit] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_TCards] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_TCards]    Script Date: 06/03/2025 14:08:41 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TCards]') AND name = N'IX_TCards')
CREATE UNIQUE NONCLUSTERED INDEX [IX_TCards] ON [dbo].[TCards]
(
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TCards_Balance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TCards] ADD  CONSTRAINT [DF_TCards_Balance]  DEFAULT ((0)) FOR [Balance]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TCards_Limit]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TCards] ADD  CONSTRAINT [DF_TCards_Limit]  DEFAULT ((0)) FOR [Limit]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TCards_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TCards] ADD  CONSTRAINT [DF_TCards_Active]  DEFAULT ((1)) FOR [Active]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TCards_Elegible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TCards] ADD  CONSTRAINT [DF_TCards_Elegible]  DEFAULT ((1)) FOR [Elegible]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TCards_Status]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TCards] ADD  CONSTRAINT [DF_TCards_Status]  DEFAULT ((0)) FOR [Status]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TCards_TCards]') AND parent_object_id = OBJECT_ID(N'[dbo].[TCards]'))
ALTER TABLE [dbo].[TCards]  WITH CHECK ADD  CONSTRAINT [FK_TCards_TCards] FOREIGN KEY([ID])
REFERENCES [dbo].[TCards] ([ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TCards_TCards]') AND parent_object_id = OBJECT_ID(N'[dbo].[TCards]'))
ALTER TABLE [dbo].[TCards] CHECK CONSTRAINT [FK_TCards_TCards]
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'TCards', N'COLUMN',N'Status'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - Blocked 1 - Authorized 2 - Suspicious 3 - Canceled' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TCards', @level2type=N'COLUMN',@level2name=N'Status'
GO


