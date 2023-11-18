select 
    sc.*, 
    data_type = st.name 
from sys.columns sc 
inner join sys.types st on sc.system_type_id = st.system_type_id and sc.user_type_id = st.user_type_id
where sc.object_id = @tvp_id
