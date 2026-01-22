-- =====================================================
-- FUNCTION: handle_updated_at
-- Description: Trigger function to auto-update updated_at timestamp
-- Usage: Applied to ALL tables with updated_at column
-- =====================================================

CREATE OR REPLACE FUNCTION public.handle_updated_at() 
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

COMMENT ON FUNCTION public.handle_updated_at() IS 'Auto-updates updated_at timestamp on row modification';
