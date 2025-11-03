-- Drop foreign key constraints
DECLARE @sql NVARCHAR(MAX) = '';
SELECT @sql += 'ALTER TABLE [' + SCHEMA_NAME(t.schema_id) + '].[' + t.name + '] DROP CONSTRAINT [' + fk.name + '];' + CHAR(13)
FROM sys.foreign_keys fk
JOIN sys.tables t ON fk.parent_object_id = t.object_id;
EXEC sp_executesql @sql;

-- Drop triggers (not tied to tables)
SET @sql = '';
SELECT @sql += 'DROP TRIGGER [' + OBJECT_SCHEMA_NAME(t.object_id) + '].[' + t.name + '];' + CHAR(13)
FROM sys.triggers t
WHERE t.parent_id = 0;
EXEC sp_executesql @sql;

-- Drop stored procedures
SET @sql = '';
SELECT @sql += 'DROP PROCEDURE [' + s.name + '].[' + p.name + '];' + CHAR(13)
FROM sys.procedures p
JOIN sys.schemas s ON p.schema_id = s.schema_id;
EXEC sp_executesql @sql;

-- Drop views
SET @sql = '';
SELECT @sql += 'DROP VIEW [' + s.name + '].[' + v.name + '];' + CHAR(13)
FROM sys.views v
JOIN sys.schemas s ON v.schema_id = s.schema_id;
EXEC sp_executesql @sql;

-- Drop functions
SET @sql = '';
SELECT @sql += 'DROP FUNCTION [' + s.name + '].[' + f.name + '];' + CHAR(13)
FROM sys.objects f
JOIN sys.schemas s ON f.schema_id = s.schema_id
WHERE f.type IN ('FN', 'IF', 'TF');
EXEC sp_executesql @sql;

-- Drop tables
SET @sql = '';
SELECT @sql += 'DROP TABLE [' + s.name + '].[' + t.name + '];' + CHAR(13)
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id;
EXEC sp_executesql @sql;
