USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanDataInsert];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanDataInsert]
  @szFileName         NVARCHAR(255)
, @szFileBody         NVARCHAR(MAX)
, @tFileCreated       NVARCHAR(255)
, @tFileModified      NVARCHAR(255)
, @bHasWrongStructure BIT
, @bIsDuplicate       BIT
, @nKey               INT = 0 OUTPUT
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanDataInsert
   PURPOSE:    Insert data to table tbl_MS_MilkoScanData
   COMMENTS:   
 
   CHANGES:    15.03.2021 DSO Create
               20.03.2021 DSO Add @bHasWrongStructure/@bIsDuplicate
****************************************************************************/

BEGIN

    DECLARE @CurrentDate DATETIME = GETDATE();

    BEGIN TRANSACTION

        BEGIN TRY
            INSERT INTO tbl_MS_MilkoScanData ( szFileName
                                             , szFileBody
                                             , tFileCreated
                                             , tFileModified
                                             , bHasWrongStructure
                                             , bIsDuplicate
                                             , tCreated)
            VALUES( @szFileName
                  , @szFileBody
                  , @tFileCreated
                  , @tFileModified
                  , @bHasWrongStructure
                  , @bIsDuplicate
                  , @CurrentDate)

            COMMIT TRANSACTION
            SELECT @nKey = SCOPE_IDENTITY()

        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION
            SELECT
                ERROR_NUMBER() AS ErrorNumber
                 ,ERROR_SEVERITY() AS ErrorSeverity
                 ,ERROR_STATE() AS ErrorState
                 ,ERROR_PROCEDURE() AS ErrorProcedure
                 ,ERROR_LINE() AS ErrorLine
                 ,ERROR_MESSAGE() AS ErrorMessage;

        END CATCH

END