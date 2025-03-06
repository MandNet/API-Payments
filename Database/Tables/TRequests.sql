/****** Object:  Table [dbo].[TRequests]    Script Date: 06/03/2025 14:13:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRequests]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TRequests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Card] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL,
	[Value] [money] NOT NULL,
	[Status] [int] NOT NULL,
	[Motive] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AI NULL,
	[ProcessorCode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AI NULL,
 CONSTRAINT [PK_TRequests] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_TRequests]    Script Date: 06/03/2025 14:13:42 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TRequests]') AND name = N'IX_TRequests')
CREATE NONCLUSTERED INDEX [IX_TRequests] ON [dbo].[TRequests]
(
	[Status] ASC,
	[ProcessorCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TRequests_Date]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TRequests] ADD  CONSTRAINT [DF_TRequests_Date]  DEFAULT (getdate()) FOR [Date]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TRequests_Value]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TRequests] ADD  CONSTRAINT [DF_TRequests_Value]  DEFAULT ((0)) FOR [Value]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TRequests_Status]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TRequests] ADD  CONSTRAINT [DF_TRequests_Status]  DEFAULT ((0)) FOR [Status]
END
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'TRequests', N'COLUMN',N'Status'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - Saved 1 - Being Processed 2 - Aproved 3 - Rejected' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TRequests', @level2type=N'COLUMN',@level2name=N'Status'
GO


