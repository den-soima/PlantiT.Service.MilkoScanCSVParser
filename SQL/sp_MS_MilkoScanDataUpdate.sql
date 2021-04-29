USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanDataUpdate];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanDataUpdate]
    @nKey               INT
  , @bIsDuplicate       BIT
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanDataUpdate
   PURPOSE:    Update bIsDuplicate to table tbl_MS_MilkoScanData
   COMMENTS:   
 
   CHANGES:    21.04.2021 DSO Create
****************************************************************************/

BEGIN

    UPDATE tbl_MS_MilkoScanData
        SET bIsDuplicate = @bIsDuplicate
    WHERE nKey = @nKey
END