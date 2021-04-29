USE [dbExchange]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * from sys.tables WHERE name='tbl_MS_MilkoScanDataSample')
    DROP TABLE [dbo].[tbl_MS_MilkoScanDataSample]

IF EXISTS (SELECT * from sys.tables WHERE name='tbl_MS_MilkoScanData')
    DROP TABLE [dbo].[tbl_MS_MilkoScanData]

CREATE TABLE [dbo].[tbl_MS_MilkoScanData](
      [nKey]               INT IDENTITY(1,1) NOT NULL
    , [szFileName]         NVARCHAR(255)     NOT NULL
    , [szFileBody]         NVARCHAR(MAX)     NOT NULL
    , [tFileCreated]       DATETIME          NOT NULL
    , [tFileModified]      DATETIME          NOT NULL
    , [bHasWrongStructure] BIT               NOT NULL
    , [bIsDuplicate]       BIT               NOT NULL
    , [tCreated]           DATETIME          NOT NULL
    CONSTRAINT [PK_dbo.tbl_MS_MilkoScanData] PRIMARY KEY CLUSTERED ([nKey] ASC))
GO


