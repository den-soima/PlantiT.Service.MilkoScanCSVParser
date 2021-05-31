USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanRelevantSampleDateGet];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanRelevantSampleDateGet]
  @tAnalysisTime  DATETIME = '0000-00-00 00:00:00' OUTPUT 
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanRelevantSampleDateGet

   PURPOSE:    Get relevant analysis date

   COMMENTS:   Only analysis with date higher then result value will be added
 
   CHANGES:    24.05.2021 DSO Created

****************************************************************************/

BEGIN

    SELECT @tAnalysisTime = MAX(tAnalysisTime) - 1 FROM tbl_MS_MilkoScanDataSample 

END
