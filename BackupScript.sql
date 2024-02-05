/*Backup script.*/
USE [master];
GO

BACKUP DATABASE [HemoTrack]
TO DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\HemoTrack.bak'
WITH NOFORMAT, NOINIT,
NAME = N'HemoTrack-Full Database backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10;
GO

/*Drop database*/
EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'HemoTrack'
GO

USE [master];
GO

DROP DATABASE [HemoTrack];
GO

/*Restore database*/
USE [master];
GO

RESTORE DATABASE [HemoTrack]
FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\HemoTrack.bak'
WITH REPLACE

USE [master]
GO
ALTER DATABASE [HemoTrack]
SET SINGLE_USER