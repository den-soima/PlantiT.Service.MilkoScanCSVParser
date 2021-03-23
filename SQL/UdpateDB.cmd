@rem *****************************************************
@rem          Update script - Create tbl/sp
@rem *****************************************************

tree /F

@echo off

@rem *** Parameters ***
@rem
SET Server=.
SET Instanse=PLANTIT
SET Database=dbExchange
SET Debug=0
SET RUN=sqlcmd -v DebugCmd=%Debug% -S %Server%\%Instanse% -d %Database% -i
@rem   
%RUN% tbl_MS_MilkoScanData.sql
%RUN% tbl_MS_MilkoScanDataSample.sql
%RUN% sp_MS_MilkoScanDataInsert.sql
%RUN% sp_MS_MilkoScanDataSampleInsert.sql
%RUN% sp_MS_MilkoScanDataDuplicateCheck.sql
@rem
echo.
@rem
@echo ---                          ---
@echo        Update Finished!!!
@echo ---                          ---

pause