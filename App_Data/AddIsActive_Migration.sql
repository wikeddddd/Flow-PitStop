-- Migration: Add soft-delete support to Students table
-- Run this once against PitStopStudent.mdf in SQL Server Object Explorer
-- (View -> SQL Server Object Explorer -> expand localdb -> PitStopStudent -> New Query)

-- 1. Add IsActive column (defaults to 1 = active for all existing rows)
ALTER TABLE Students
    ADD IsActive BIT NOT NULL DEFAULT 1;

-- 2. Verify
SELECT Id, username, IsActive FROM Students;
