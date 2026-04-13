-- Migration: Add authentication columns to Users_master
-- Run this once against the PS_UserData database if these columns are missing.
-- All columns are nullable to avoid breaking existing rows.

USE PS_UserData;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users_master') AND name = 'email'
)
BEGIN
    ALTER TABLE dbo.Users_master ADD email NVARCHAR(255) NULL;
    PRINT 'Column email added.';
END
ELSE
    PRINT 'Column email already exists.';
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users_master') AND name = 'securityquestion'
)
BEGIN
    ALTER TABLE dbo.Users_master ADD securityquestion NVARCHAR(255) NULL;
    PRINT 'Column securityquestion added.';
END
ELSE
    PRINT 'Column securityquestion already exists.';
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users_master') AND name = 'securityanswer'
)
BEGIN
    ALTER TABLE dbo.Users_master ADD securityanswer NVARCHAR(255) NULL;
    PRINT 'Column securityanswer added.';
END
ELSE
    PRINT 'Column securityanswer already exists.';
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users_master') AND name = 'resettoken'
)
BEGIN
    ALTER TABLE dbo.Users_master ADD resettoken NVARCHAR(255) NULL;
    PRINT 'Column resettoken added.';
END
ELSE
    PRINT 'Column resettoken already exists.';
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users_master') AND name = 'resettokenexpiresat'
)
BEGIN
    ALTER TABLE dbo.Users_master ADD resettokenexpiresat DATETIME NULL;
    PRINT 'Column resettokenexpiresat added.';
END
ELSE
    PRINT 'Column resettokenexpiresat already exists.';
GO
