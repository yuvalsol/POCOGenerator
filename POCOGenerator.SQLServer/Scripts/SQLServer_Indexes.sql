select
	Is_Table_Index = cast(1 as bit),
	Is_View_Index = cast(0 as bit),
	Name = i.name,
	[Schema] = s.name,
	Table_Name = t.name,
	Is_Unique = isnull(i.is_unique,0),
	Is_Clustered = cast((case when i.type = 1 then 1 else 0 end) as bit),
	Ordinal = ic.key_ordinal,
	Column_Name = c.name,
	Is_Descending = isnull(ic.is_descending_key,0)
from sys.indexes i
inner join sys.index_columns ic on  i.object_id = ic.object_id and i.index_id = ic.index_id
inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
inner join sys.tables t on i.object_id = t.object_id
inner join sys.schemas s on t.schema_id = s.schema_id
where i.type in (1,2) -- clustered, nonclustered
and i.is_primary_key = 0
and i.is_unique_constraint = 0
and i.is_disabled = 0
and i.is_hypothetical = 0
and t.is_ms_shipped = 0
and ic.is_included_column = 0

union all

select
	Is_Table_Index = cast(0 as bit),
	Is_View_Index = cast(1 as bit),
	Name = i.name,
	[Schema] = s.name,
	Table_Name = v.name,
	Is_Unique = isnull(i.is_unique,0),
	Is_Clustered = cast((case when i.type = 1 then 1 else 0 end) as bit),
	Ordinal = ic.key_ordinal,
	Column_Name = c.name,
	Is_Descending = isnull(ic.is_descending_key,0)
from sys.indexes i
inner join sys.index_columns ic on  i.object_id = ic.object_id and i.index_id = ic.index_id
inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
inner join sys.views v on i.object_id = v.object_id
inner join sys.schemas s on v.schema_id = s.schema_id
where i.type in (1,2) -- clustered, nonclustered
and i.is_primary_key = 0
and i.is_unique_constraint = 0
and i.is_disabled = 0
and i.is_hypothetical = 0
and v.is_ms_shipped = 0
and ic.is_included_column = 0

order by Name, [Schema], Table_Name, Ordinal
