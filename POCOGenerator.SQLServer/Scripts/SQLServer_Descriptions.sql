-- database
select
	[Type] = 'Database',
	[Schema] = null,
	Name = db_name(),
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
where ep.class = 0
and ep.major_id = 0
and ep.minor_id = 0
and ep.name = N'MS_Description'

union all

-- schema
select
	[Type] = 'Schema',
	[Schema] = null,
	Name = s.name,
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
inner join sys.schemas s on ep.major_id = s.schema_id
where ep.class = 3
and ep.minor_id = 0
and ep.name = N'MS_Description'

union all

-- table
select
	[Type] = 'Table',
	[Schema] = s.name,
	Name = t.name,
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
inner join sys.tables t on ep.major_id = t.object_id
inner join sys.schemas s on t.schema_id = s.schema_id
where ep.class = 1
and ep.minor_id = 0
and ep.name = N'MS_Description'

union all

-- view
select
	[Type] = 'View',
	[Schema] = s.name,
	Name = v.name,
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
inner join sys.views v on ep.major_id = v.object_id
inner join sys.schemas s on v.schema_id = s.schema_id
where ep.class = 1
and ep.minor_id = 0
and ep.name = N'MS_Description'

union all

-- table column
select
	[Type] = 'TableColumn',
	[Schema] = s.name,
	Name = t.name,
	Minor_Name = c.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.tables t on ep.major_id = t.object_id
inner join sys.schemas s on t.schema_id = s.schema_id
inner join sys.columns c on ep.major_id = c.object_id and ep.minor_id = c.column_id
where ep.class = 1
and ep.name = N'MS_Description'

union all

-- view column
select
	[Type] = 'ViewColumn',
	[Schema] = s.name,
	Name = v.name,
	Minor_Name = c.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.views v on ep.major_id = v.object_id
inner join sys.schemas s on v.schema_id = s.schema_id
inner join sys.columns c on ep.major_id = c.object_id and ep.minor_id = c.column_id
where ep.class = 1
and ep.name = N'MS_Description'

union all

-- index (table)
select
	[Type] = 'Index',
	[Schema] = s.name,
	Name = t.name,
	Minor_Name = i.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.tables t on ep.major_id = t.object_id
inner join sys.schemas s on t.schema_id = s.schema_id
inner join sys.indexes i on ep.major_id = i.object_id and ep.minor_id = i.index_id
where ep.class = 7
and ep.name = N'MS_Description'

union all

-- index (view)
select
	[Type] = 'Index',
	[Schema] = s.name,
	Name = v.name,
	Minor_Name = i.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.views v on ep.major_id = v.object_id
inner join sys.schemas s on v.schema_id = s.schema_id
inner join sys.indexes i on ep.major_id = i.object_id and ep.minor_id = i.index_id
where ep.class = 7
and ep.name = N'MS_Description'

union all

-- stored procedure
select
	[Type] = 'Procedure',
	[Schema] = s.name,
	Name = sp.name,
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
inner join sys.procedures sp on ep.major_id = sp.object_id
inner join sys.schemas s on sp.schema_id = s.schema_id
where ep.class = 1
and ep.minor_id = 0
and ep.name = N'MS_Description'

union all

-- function
select
	[Type] = 'Function',
	[Schema] = s.name,
	Name = f.name,
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
inner join sys.objects f on ep.major_id = f.object_id
inner join sys.schemas s on f.schema_id = s.schema_id
where ep.class = 1
and ep.minor_id = 0
and ep.name = N'MS_Description'
and f.type = 'TF'

union all

-- stored procedure parameter
select
	[Type] = 'ProcedureParameter',
	[Schema] = s.name,
	Name = sp.name,
	Minor_Name = p.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.procedures sp on ep.major_id = sp.object_id
inner join sys.schemas s on sp.schema_id = s.schema_id
inner join sys.parameters p on ep.major_id = p.object_id and ep.minor_id = p.parameter_id
where ep.class = 2
and ep.name = N'MS_Description'

union all

-- function parameter
select
	[Type] = 'FunctionParameter',
	[Schema] = s.name,
	Name = f.name,
	Minor_Name = p.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.objects f on ep.major_id = f.object_id
inner join sys.schemas s on f.schema_id = s.schema_id
inner join sys.parameters p on ep.major_id = p.object_id and ep.minor_id = p.parameter_id
where ep.class = 2
and ep.name = N'MS_Description'
and f.type = 'TF'

union all

-- tvp
select
	[Type] = 'TVP',
	[Schema] = s.name,
	Name = tt.name,
	Minor_Name = null,
	Description = ep.value
from sys.extended_properties ep
inner join sys.table_types tt on ep.major_id = tt.user_type_id
inner join sys.schemas s on tt.schema_id = s.schema_id
where ep.class = 6
and ep.name = N'MS_Description'

union all

-- tvp column
select
	[Type] = 'TVPColumn',
	[Schema] = s.name,
	Name = tt.name,
	Minor_Name = c.name,
	Description = ep.value
from sys.extended_properties ep
inner join sys.table_types tt on ep.major_id = tt.user_type_id
inner join sys.schemas s on tt.schema_id = s.schema_id
inner join sys.columns c on tt.type_table_object_id = c.object_id and ep.minor_id = c.column_id
where ep.class = 8
and ep.name = N'MS_Description'

order by [Type], [Schema], Name, Minor_Name
