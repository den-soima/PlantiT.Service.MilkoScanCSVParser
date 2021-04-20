USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanLastSampleDateGet];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanLastSampleDateGet]
  @tAnalysisTime  DATETIME = '0000-00-00 00:00:00' OUTPUT 
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanLastSampleDateGet

   PURPOSE:    Get last analysis date

   COMMENTS:   
 
   CHANGES:    20.04.2021 DSO Created

****************************************************************************/

BEGIN

    SELECT @tAnalysisTime = MAX(tAnalysisTime) FROM tbl_MS_MilkoScanDataSample 

END
