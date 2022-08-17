1. Setup migration: Enable-Migrations -ProjectName CAPTimeTableIT.Infrastructure -StartUpProjectName CAPTimeTableIT -Verbose
2. Add a new migration: Add-Migration InitialDatabase -ProjectName CAPTimeTableIT.Infrastructure -StartUpProjectName CAPTimeTableIT -Verbose
3. Update Database: Update-Database -ProjectName CAPTimeTableIT.Infrastructure -StartUpProjectName CAPTimeTableIT -Verbose
3.1 Generate a script: Update-Database -script -ProjectName CAPTimeTableIT.Infrastructure -StartUpProjectName CAPTimeTableIT -Verbose