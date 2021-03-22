USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanDataDuplicateCheck];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanDataDuplicateCheck]
  @tAnalysisTime    DATETIME
, @bIsDuplicate     BIT = 0 OUTPUT 
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanDataDuplicateCheck

   PURPOSE:    Check that new sample is not duplicate

   COMMENTS:   
 
   CHANGES:    20.03.2021 DSO Created

****************************************************************************/

BEGIN

    IF EXISTS(SELECT nKey FROM tbl_MS_MilkoScanDataSample WHERE tAnalysisTime = @tAnalysisTime)
        SET @bIsDuplicate = 1
    ELSE
        SET @bIsDuplicate = 0      

END
