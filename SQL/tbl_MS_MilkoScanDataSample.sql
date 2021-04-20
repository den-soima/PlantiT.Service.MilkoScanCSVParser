USE [dbExchange]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * from sys.tables WHERE name='tbl_MS_MilkoScanDataSample')
    DROP TABLE [dbo].[tbl_MS_MilkoScanDataSample]

CREATE TABLE [dbo].[tbl_MS_MilkoScanDataSample](
      [nKey]                     INT IDENTITY(1,1) NOT NULL
    , [nMilkoScanDataLink]       INT               NOT NULL
    , [tAnalysisTime]            DATETIME          NOT NULL 
    , [szProductName]            NVARCHAR(128)     NOT NULL
    , [szSampleId]               NVARCHAR(10)      NOT NULL
    , [szDate]                   NVARCHAR(8)       NOT NULL 
    , [szTime]                   NVARCHAR(8)       NOT NULL 
    , [szSampleStatus]           NVARCHAR(128)     NULL 
    , [nSampleNumber]            INT               NOT NULL 
    , [rWhey]                    REAL              NOT NULL
    , [rFat]                     REAL              NOT NULL
    , [rLactose]                 REAL              NOT NULL
    , [rDryParticles]            REAL              NOT NULL 
    , [rDryParticlesFatFree]     REAL              NOT NULL 
    , [rFreezingPoint]           REAL              NOT NULL 
    , [szInstrumentStatus]       NVARCHAR(32)      NOT NULL
    CONSTRAINT [PK_dbo.tbl_MS_MilkoScanDataSample] PRIMARY KEY CLUSTERED ([nKey] ASC))
    
ALTER TABLE [dbo].[tbl_MS_MilkoScanDataSample]  WITH CHECK 
    ADD  CONSTRAINT [FK_dbo.tbl_MS_MilkoScanDataSample.tbl_MS_MilkoScanData_nMilkoScanDataLink] 
        FOREIGN KEY([nMilkoScanDataLink])
    REFERENCES [dbo].[tbl_MS_MilkoScanData] ([nKey])
    ON DELETE CASCADE

CREATE INDEX IX_nSampleNumber ON tbl_MS_MilkoScanDataSample (nSampleNumber);

GO


