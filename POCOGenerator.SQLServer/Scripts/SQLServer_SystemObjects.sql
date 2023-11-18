select 
    [Type] = al.type,
    [Schema] = ss.name,
    [Name] = al.name
from sys.all_objects al
inner join sys.schemas ss on al.schema_id = ss.schema_id
where al.is_ms_shipped = 1
