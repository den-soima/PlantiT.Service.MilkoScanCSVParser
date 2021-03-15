USE [MES]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * from sys.tables WHERE name='tbl_MS_MilkoScanDataSample')
BEGIN

    CREATE TABLE [dbo].[tbl_MS_MilkoScanDataSample](
          [nKey]                     INT IDENTITY(1,1) NOT NULL
        , [nMilkoScanDataLink]       INT               NOT NULL
        , [tAnalysisTime]            DATETIME          NOT NULL 
        , [szProductName]            NVARCHAR(128)     NOT NULL
        , [szProductCode]            NVARCHAR(32)      NOT NULL
        , [szSampleType]             NVARCHAR(32)      NOT NULL 
        , [szSampleNumber]           NVARCHAR(32)      NOT NULL 
        , [szSampleComment]          NVARCHAR(256)     NULL 
        , [szInstrumentName]         NVARCHAR(64)      NOT NULL 
        , [szInstrumentSerialNumber] NVARCHAR(32)      NOT NULL 
        , [rFat]                     REAL              NOT NULL 
        , [rRefFat]                  REAL              NULL      
        , [rWhey]                    REAL              NOT NULL 
        , [rRefWhey]                 REAL              NULL
        , [rDryParticles]            REAL              NOT NULL 
        , [rRefDryParticles]         REAL              NULL 
        , [rDryFatFreeParticles]     REAL              NOT NULL 
        , [rRefDryFatFreeParticles]  REAL              NULL 
        , [rFreezingPoint]           REAL              NOT NULL 
        , [rRefFreezingPoint]        REAL              NULL
        , [rLactose]                 REAL              NOT NULL 
        , [rRefLactose]              REAL              NULL 
        CONSTRAINT [PK_dbo.tbl_MS_MilkoScanDataSample] PRIMARY KEY CLUSTERED ([nKey] ASC))


    ALTER TABLE [dbo].[tbl_MS_MilkoScanDataSample]  WITH CHECK 
        ADD  CONSTRAINT [FK_dbo.tbl_MS_MilkoScanDataSample.tbl_MS_MilkoScanData_nMilkoScanDataLink] 
            FOREIGN KEY([nMilkoScanDataLink])
        REFERENCES [dbo].[tbl_MS_MilkoScanData] ([nKey])
        ON DELETE CASCADE
    
END
GO


