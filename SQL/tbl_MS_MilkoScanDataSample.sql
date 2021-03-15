USE [MES]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * from sys.tables WHERE name='tbl_MS_MilkoScanDataSample')

CREATE TABLE [dbo].[tbl_MS_MilkoScanDataSample](
      [nKey]               INT IDENTITY(1,1) NOT NULL
    , [nMilkoScanDataLink] INT               NOT NULL
    , [tAnalysisTime]      DATETIME          NOT NULL 
    , [szProductName]
    , [szProductCode]
    , [szSampleType]
    , [szSampleNumber]
    , [szSampleComment]
    , [szInstrumentName]
    , [szInstrumentSerialNumber]
    , [rFat]
    , [rRefFat]
    , [rWhey]
    , [rRefWhey]
    , [rDryParticles]
    , [rRefDryParticles]
    , [rDryFatFreeParticles]
    , [rRefDryFatFreeParticles]
    , [rFreezingPoint]
    , [rRefFreezingPoint]
    , [rLactose]
    , [rRefLactose]         REAL
    CONSTRAINT [PK_dbo.tbl_MS_MilkoScanDataSample] PRIMARY KEY CLUSTERED ([nKey] ASC))
GO

ALTER TABLE [dbo].[tbl_MS_MilkoScanDataSample]  WITH CHECK 
    ADD  CONSTRAINT [FK_dbo.tbl_MS_MilkoScanDataSample.tbl_MS_MilkoScanData_nMilkoScanDataLink] 
        FOREIGN KEY([nMilkoScanDataLink])
    REFERENCES [dbo].[tbl_MS_MilkoScanData] ([nKey])
    ON DELETE CASCADE
GO


