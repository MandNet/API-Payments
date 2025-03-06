/****** Object:  Table [dbo].[TTransactions]    Script Date: 06/03/2025 14:14:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TTransactions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TTransactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Card_Id] [int] NOT NULL,
	[AuthorizationCode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL,
	[Value] [money] NOT NULL,
	[Fee_Id] [int] NOT NULL,
	[Total] [money] NOT NULL,
	[Request_Id] [int] NOT NULL,
 CONSTRAINT [PK_TTransactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Index [IX_TTransactions]    Script Date: 06/03/2025 14:14:27 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TTransactions]') AND name = N'IX_TTransactions')
CREATE NONCLUSTERED INDEX [IX_TTransactions] ON [dbo].[TTransactions]
(
	[Card_Id] ASC,
	[Date] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TTransactions_Date]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TTransactions] ADD  CONSTRAINT [DF_TTransactions_Date]  DEFAULT (getdate()) FOR [Date]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Table_1_Authorization]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TTransactions] ADD  CONSTRAINT [DF_Table_1_Authorization]  DEFAULT (newid()) FOR [AuthorizationCode]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TTransactions_Value]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TTransactions] ADD  CONSTRAINT [DF_TTransactions_Value]  DEFAULT ((0)) FOR [Value]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TTransactions_Fee]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TTransactions] ADD  CONSTRAINT [DF_TTransactions_Fee]  DEFAULT ((0)) FOR [Fee_Id]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TTransactions_Total]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TTransactions] ADD  CONSTRAINT [DF_TTransactions_Total]  DEFAULT ((0)) FOR [Total]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TTransactions_TCards]') AND parent_object_id = OBJECT_ID(N'[dbo].[TTransactions]'))
ALTER TABLE [dbo].[TTransactions]  WITH CHECK ADD  CONSTRAINT [FK_TTransactions_TCards] FOREIGN KEY([Card_Id])
REFERENCES [dbo].[TCards] ([ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TTransactions_TCards]') AND parent_object_id = OBJECT_ID(N'[dbo].[TTransactions]'))
ALTER TABLE [dbo].[TTransactions] CHECK CONSTRAINT [FK_TTransactions_TCards]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TTransactions_TFees]') AND parent_object_id = OBJECT_ID(N'[dbo].[TTransactions]'))
ALTER TABLE [dbo].[TTransactions]  WITH CHECK ADD  CONSTRAINT [FK_TTransactions_TFees] FOREIGN KEY([Fee_Id])
REFERENCES [dbo].[TFees] ([ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TTransactions_TFees]') AND parent_object_id = OBJECT_ID(N'[dbo].[TTransactions]'))
ALTER TABLE [dbo].[TTransactions] CHECK CONSTRAINT [FK_TTransactions_TFees]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TTransactions_TRequests]') AND parent_object_id = OBJECT_ID(N'[dbo].[TTransactions]'))
ALTER TABLE [dbo].[TTransactions]  WITH CHECK ADD  CONSTRAINT [FK_TTransactions_TRequests] FOREIGN KEY([Request_Id])
REFERENCES [dbo].[TRequests] ([ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TTransactions_TRequests]') AND parent_object_id = OBJECT_ID(N'[dbo].[TTransactions]'))
ALTER TABLE [dbo].[TTransactions] CHECK CONSTRAINT [FK_TTransactions_TRequests]
GO


