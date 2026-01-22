-- =====================================================
-- FUNCTION: public.immutable_unaccent
-- Description: Wrapper for unaccent() marked as IMMUTABLE for use in generated columns
-- =====================================================

CREATE OR REPLACE FUNCTION public.immutable_unaccent(text) 
RETURNS text AS $$
  SELECT unaccent($1);
$$ LANGUAGE SQL IMMUTABLE PARALLEL SAFE STRICT;

COMMENT ON FUNCTION public.immutable_unaccent(text) IS 'Immutable wrapper for unaccent() - safe for generated columns and indexes';
