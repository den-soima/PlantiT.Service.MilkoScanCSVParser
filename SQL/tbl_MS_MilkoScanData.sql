USE [dbExchange]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * from sys.tables WHERE name='tbl_MS_MilkoScanData')

CREATE TABLE [dbo].[tbl_MS_MilkoScanData](
      [nKey]          INT IDENTITY(1,1) NOT NULL
    , [szFileName]    NVARCHAR(255)     NOT NULL
    , [szFileBody]    NVARCHAR(MAX)     NOT NULL
    , [tFileCreated]  DATETIME          NOT NULL
    , [tFileModified] DATETIME          NOT NULL
    , [tCreated]      datetime          NOT NULL
    CONSTRAINT [PK_dbo.tbl_MS_MilkoScanData] PRIMARY KEY CLUSTERED ([nKey] ASC))
GO


