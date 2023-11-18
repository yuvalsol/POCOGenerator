-- set @database_name = database();

select
    t.table_name as Table_Name,
    c.column_name as Column_Name
from information_schema.tables t
inner join information_schema.columns c
    on t.table_catalog = c.table_catalog
    and t.table_schema = c.table_schema
    and t.table_name = c.table_name
where t.table_schema = @database_name
and t.table_type = 'BASE TABLE'
and (locate('VIRTUAL GENERATED', c.extra) > 0 or locate('VIRTUAL STORED', c.extra) > 0)
order by t.table_name, c.column_name;
