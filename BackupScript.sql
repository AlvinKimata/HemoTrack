USE [master];
GO

BACKUP DATABASE [HemoTrack]
TO DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\HemoTrack.bak'
WITH NOFORMAT, NOINIT,
NAME = N'HemoTrack-Full Database backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10;
GO