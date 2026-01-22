-- =====================================================
-- INITIAL SETUP: Schemas & Extensions
-- Description: Creates all required schemas and extensions
-- Run Order: FIRST (before any table creation)
-- =====================================================

-- Create schemas
CREATE SCHEMA IF NOT EXISTS identity;
CREATE SCHEMA IF NOT EXISTS assets;
CREATE SCHEMA IF NOT EXISTS school;
CREATE SCHEMA IF NOT EXISTS content;
CREATE SCHEMA IF NOT EXISTS game;
CREATE SCHEMA IF NOT EXISTS audit;

-- Enable required extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";
CREATE EXTENSION IF NOT EXISTS "unaccent";

COMMENT ON SCHEMA assets IS 'Media library: uploaded files metadata (images, audio, videos, documents)';
COMMENT ON SCHEMA identity IS 'User authentication, tenants, and access control';
COMMENT ON SCHEMA school IS 'Students, guardians, classes, teachers, and school entities';
COMMENT ON SCHEMA content IS 'Educational content: modules, activities, and assets';
COMMENT ON SCHEMA game IS 'Gamification: progress tracking, badges, and achievements';
COMMENT ON SCHEMA audit IS 'System logs and audit trails';
