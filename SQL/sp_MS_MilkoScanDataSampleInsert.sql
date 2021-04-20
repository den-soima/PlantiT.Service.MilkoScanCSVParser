USE [dbExchange]
GO
-- Drop procedure if exist
DROP PROCEDURE IF EXISTS [dbo].[sp_MS_MilkoScanDataSampleInsert];
--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_MS_MilkoScanDataSampleInsert]
      @nMilkoScanDataLink        INT              
    , @tAnalysisTime             DATETIME           
    , @szProductName             NVARCHAR(128)    
    , @szSampleId                NVARCHAR(10)      
    , @szDate                    NVARCHAR(8)    
    , @szTime                    NVARCHAR(8)     
    , @szSampleStatus            NVARCHAR(128)    
    , @nSampleNumber             INT               
    , @rWhey                     REAL       
    , @rFat                      REAL           
    , @rLactose                  REAL            
    , @rDryParticles             REAL            
    , @rDryParticlesFatFree      REAL         
    , @rFreezingPoint            REAL           
    , @szInstrumentStatus        NVARCHAR(32)      
    , @nKey                      INT = 0 OUTPUT 
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanDataSampleInsert

   PURPOSE:    Insert data to table tbl_MS_MilkoScanDataSample

   COMMENTS:   
 
   CHANGES:    15.02.2021 DSO Create
               20.04.2021 DSO Modified - source file structure changed

****************************************************************************/

BEGIN

    DECLARE @CurrentDate DATETIME = GETDATE();

    BEGIN TRANSACTION

        BEGIN TRY
            INSERT INTO tbl_MS_MilkoScanDataSample ( nMilkoScanDataLink
                                                   , tAnalysisTime         
                                                   , szProductName    
                                                   , szSampleId            
                                                   , szDate                
                                                   , szTime                  
                                                   , szSampleStatus          
                                                   , nSampleNumber       
                                                   , rWhey                 
                                                   , rFat                  
                                                   , rLactose               
                                                   , rDryParticles           
                                                   , rDryParticlesFatFree  
                                                   , rFreezingPoint          
                                                   , szInstrumentStatus)                
            VALUES( @nMilkoScanDataLink      
                  , @tAnalysisTime         
                  , @szProductName    
                  , @szSampleId            
                  , @szDate                
                  , @szTime                  
                  , @szSampleStatus          
                  , @nSampleNumber       
                  , @rWhey                 
                  , @rFat                  
                  , @rLactose               
                  , @rDryParticles           
                  , @rDryParticlesFatFree  
                  , @rFreezingPoint          
                  , @szInstrumentStatus)            

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
