USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanLastSamplesGet];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanLastSamplesGet]
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanLastSamplesGet

   PURPOSE:    Get last analysis date

   COMMENTS:   
 
   CHANGES:    20.04.2021 DSO Created

****************************************************************************/

BEGIN

    DECLARE @tLastAnalysisTime DATETIME
    
    SELECT @tLastAnalysisTime = MAX(tAnalysisTime) FROM tbl_MS_MilkoScanDataSample
    
    SELECT nSampleNumber FROM tbl_MS_MilkoScanDataSample 
    WHERE tAnalysisTime > @tLastAnalysisTime - 1

END
