select 
    tvp_schema = ss.name, 
    tvp_name = stt.name, 
    stt.type_table_object_id 
from sys.table_types stt 
inner join sys.schemas ss on stt.schema_id = ss.schema_id
