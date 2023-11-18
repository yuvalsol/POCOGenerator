-- set @database_name = database();

select
    true as Is_Table_Index,
    false as Is_View_Index,
    s.index_name as Name,
    s.table_name as Table_Name,
    (s.non_unique = 0) as Is_Unique,
    (s.index_name = 'PRIMARY') as Is_Clustered,
    s.seq_in_index as Ordinal,
    s.column_name as Column_Name,
    (s.collation is not null and s.collation = 'D') as Is_Descending
from information_schema.statistics s
where s.table_schema = @database_name
order by Name, Table_Name, Ordinal;
