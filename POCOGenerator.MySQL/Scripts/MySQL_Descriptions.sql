-- set @database_name = database();

-- table
select 
	'Table' as Type,
    table_name as Name,
	null as Minor_Name,
    table_comment as Description
from information_schema.tables
where table_schema = @database_name
and table_type = 'BASE TABLE'
and table_comment is not null
and table_comment <> ''

union all

-- view
select 
	'View' as Type,
    table_name as Name,
	null as Minor_Name,
    table_comment as Description
from information_schema.tables
where table_schema = @database_name
and table_type = 'VIEW'
and table_comment is not null
and table_comment <> ''
and table_comment <> 'VIEW'

union all

-- table column
select 
	'TableColumn' as Type,
    t.table_name as Name,
	c.column_name as Minor_Name,
    c.column_comment as Description
from information_schema.tables t
inner join information_schema.columns c
	on t.table_catalog = c.table_catalog
	and t.table_schema = c.table_schema
	and t.table_name = c.table_name
where t.table_schema = @database_name
and t.table_type = 'BASE TABLE'
and c.column_comment is not null
and c.column_comment <> ''

union all

-- view column
select 
	'ViewColumn' as Type,
    t.table_name as Name,
	c.column_name as Minor_Name,
    c.column_comment as Description
from information_schema.tables t
inner join information_schema.columns c
	on t.table_catalog = c.table_catalog
	and t.table_schema = c.table_schema
	and t.table_name = c.table_name
where t.table_schema = @database_name
and t.table_type = 'VIEW'
and c.column_comment is not null
and c.column_comment <> ''

union all

-- index
select 
	'Index' as Type,
    t.table_name as Name,
	s.index_name as Minor_Name,
    s.index_comment as Description
from information_schema.tables t
inner join information_schema.statistics s
	on t.table_catalog = s.table_catalog
	and t.table_schema = s.table_schema
	and t.table_name = s.table_name
where t.table_schema = @database_name
and t.table_type = 'BASE TABLE'
and s.index_comment is not null
and s.index_comment <> ''

union all

-- stored procedure
select
	'Procedure' as Type,
	routine_name as Name,
	null as Minor_Name,
	cast(routine_comment as char(1024)) as Description
from information_schema.routines
where routine_schema = @database_name
and routine_type = 'PROCEDURE'
and routine_comment is not null
and routine_comment <> ''

order by Type, Name, Minor_Name;
