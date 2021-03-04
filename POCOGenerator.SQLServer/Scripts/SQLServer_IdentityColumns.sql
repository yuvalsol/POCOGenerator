select 
	[Schema] = ss.name,
	Table_Name = object_name(c.object_id),
	Column_Name = c.name
from sys.columns c
inner join sys.sysobjects so on c.object_id = so.id
inner join sys.schemas ss on so.uid = ss.schema_id
where c.is_identity = 1
order by [Schema], Table_Name, Column_Name
