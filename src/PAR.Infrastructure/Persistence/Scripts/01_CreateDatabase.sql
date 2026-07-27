-- ============================================================
-- PAR - Script de creación de base de datos
-- Ejecutar en orden: 01 → 02 → 03
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PARDB')
BEGIN
    CREATE DATABASE PARDB;
    PRINT 'Base de datos PARDB creada.';
END
ELSE
    PRINT 'La base de datos PARDB ya existe.';
GO

USE PARDB;
GO
