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
    , @szProductCode             NVARCHAR(32) 
    , @szSampleType              NVARCHAR(32) 
    , @szSampleNumber            NVARCHAR(32) 
    , @szSampleComment           NVARCHAR(256)
    , @szInstrumentName          NVARCHAR(64) 
    , @szInstrumentSerialNumber  NVARCHAR(32) 
    , @rFat                      REAL         
    , @rRefFat                   REAL          
    , @rWhey                     REAL         
    , @rRefWhey                  REAL         
    , @rDryParticles             REAL         
    , @rRefDryParticles          REAL         
    , @rDryFatFreeParticles      REAL         
    , @rRefDryFatFreeParticles   REAL         
    , @rFreezingPoint            REAL         
    , @rRefFreezingPoint         REAL         
    , @rLactose                  REAL         
    , @rRefLactose               REAL         
    , @nKey                      INT = 0 OUTPUT 
AS

/****************************************************************************
   FUNCTION:   sp_MS_MilkoScanDataSampleInsert

   PURPOSE:    Insert data to table tbl_MS_MilkoScanDataSample

   COMMENTS:   
 
   CHANGES:    15.02.2021 DSO Create

****************************************************************************/

BEGIN

    DECLARE @CurrentDate DATETIME = GETDATE();

    BEGIN TRANSACTION

        BEGIN TRY
            INSERT INTO tbl_MS_MilkoScanDataSample ( nMilkoScanDataLink     
                                                   , tAnalysisTime        
                                                   , szProductName          
                                                   , szProductCode          
                                                   , szSampleType           
                                                   , szSampleNumber        
                                                   , szSampleComment        
                                                   , szInstrumentName       
                                                   , szInstrumentSerialNumber
                                                   , rFat                   
                                                   , rRefFat                
                                                   , rWhey                  
                                                   , rRefWhey              
                                                   , rDryParticles         
                                                   , rRefDryParticles
                                                   , rDryFatFreeParticles   
                                                   , rRefDryFatFreeParticles
                                                   , rFreezingPoint      
                                                   , rRefFreezingPoint      
                                                   , rLactose               
                                                   , rRefLactose)            
            VALUES( @nMilkoScanDataLink      
                  , @tAnalysisTime           
                  , @szProductName           
                  , @szProductCode           
                  , @szSampleType            
                  , @szSampleNumber          
                  , @szSampleComment         
                  , @szInstrumentName        
                  , @szInstrumentSerialNumber
                  , @rFat                    
                  , @rRefFat                 
                  , @rWhey                   
                  , @rRefWhey                
                  , @rDryParticles           
                  , @rRefDryParticles        
                  , @rDryFatFreeParticles    
                  , @rRefDryFatFreeParticles 
                  , @rFreezingPoint          
                  , @rRefFreezingPoint       
                  , @rLactose                
                  , @rRefLactose)             

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
