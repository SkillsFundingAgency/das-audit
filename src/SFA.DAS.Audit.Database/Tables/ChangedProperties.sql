CREATE TABLE [dbo].[ChangedProperties](
	[PropertyName] [varchar](50) NOT NULL,
	[NewValue] [varchar](max) NULL,
	[MessageId] [uniqueidentifier] NOT NULL
)