@rem *****************************************************
@rem                 Configure Service
@rem *****************************************************

tree /F

@echo off

@rem *** Parameters ***
@rem
SET Name="Plant iT MilkoScan Data Parser"
SET Path="C:\PROLEIT\Project\Services\PlantiT.Service.MilkoScanCSVParser\publish\PlantiT.Service.MilkoScanCSVParser.exe"
@rem   

sc create %Name% binPath=%Path%
sc start %Name%

@rem
echo.
@rem
@echo ---                          ---
@echo        Service Started!!!
@echo ---                          ---

pause


