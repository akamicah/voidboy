DO $$
DECLARE row RECORD;
BEGIN
    FOR row IN SELECT table_name
        FROM information_schema.tables
        WHERE table_type='BASE TABLE'
        AND table_schema='public'
        AND table_name NOT IN ('migrations')
    LOOP
        EXECUTE format('TRUNCATE TABLE %I CASCADE;',row.table_name);
    END LOOP;
END;
$$;