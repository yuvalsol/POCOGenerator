set SQL_SAFE_UPDATES = 0;

-- set @database_name = database();

create temporary table TableColumns (
    Table_Id int,
    Table_Name varchar(64),
    Column_Id int,
    Column_Name varchar(64),
    Is_Identity bit,
    Is_Computed bit
);

insert into TableColumns
select
    t.Table_Id,
    t.table_name as Table_Name,
    c.ordinal_position as Column_Id,
    c.column_name as Column_Name,
    if(locate('auto_increment', c.extra) > 0, 1, 0) as Is_Identity,
    if(locate('VIRTUAL GENERATED', c.extra) > 0 or locate('VIRTUAL STORED', c.extra) > 0, 1, 0) as Is_Computed
from (
    select
        t.table_catalog,
        t.table_schema,
        t.table_name,
        @'Table_Id' := @'Table_Id' + 1 as Table_Id
    from information_schema.tables t,
    (select @'Table_Id' := 0) ti
    where t.table_schema = @database_name
    and t.table_type = 'BASE TABLE'
) t
inner join information_schema.columns c
    on t.table_catalog = c.table_catalog
    and t.table_schema = c.table_schema
    and t.table_name = c.table_name
order by t.table_name, c.ordinal_position;

create temporary table PrimaryKeys (
    Id int,
    Name varchar(64),
    Table_Id int,
    Table_Name varchar(64),
    Ordinal bigint(10),
    Column_Id int,
    Column_Name varchar(64),
    Is_Descending bit,
    Is_Identity bit,
    Is_Computed bit
);

insert into PrimaryKeys
select
    @'Id1' := @'Id1' + 1 as Id,
    tc.constraint_name as Name,
    c.Table_Id,
    c.Table_Name,
    kcu.ordinal_position as Ordinal,
    c.Column_Id,
    c.Column_Name,
    0 as Is_Descending,
    c.Is_Identity,
    c.Is_Computed
from information_schema.table_constraints tc
inner join information_schema.key_column_usage kcu
    on tc.constraint_catalog = kcu.constraint_catalog
    and tc.constraint_schema = kcu.constraint_schema
    and tc.constraint_name = kcu.constraint_name
    and tc.table_schema = kcu.table_schema
    and tc.table_name = kcu.table_name
inner join TableColumns c
    on kcu.table_name = c.table_name
    and kcu.column_name = c.column_name,
(select @'Id1' := 0) ci
where tc.table_schema = @database_name
and tc.constraint_type = 'PRIMARY KEY'
order by tc.constraint_name, c.Table_Name, kcu.ordinal_position;

create temporary table UniqueKeys (
    Id int,
    Name varchar(64),
    Table_Id int,
    Table_Name varchar(64),
    Ordinal bigint(10),
    Column_Id int,
    Column_Name varchar(64),
    Is_Descending bit,
    Is_Identity bit,
    Is_Computed bit
);

insert into UniqueKeys
select
    @'Id2' := @'Id2' + 1 as Id,
    tc.constraint_name as Name,
    c.Table_Id,
    c.Table_Name,
    kcu.ordinal_position as Ordinal,
    c.Column_Id,
    c.Column_Name,
    0 as Is_Descending,
    c.Is_Identity,
    c.Is_Computed
from information_schema.table_constraints tc
inner join information_schema.key_column_usage kcu
    on tc.constraint_catalog = kcu.constraint_catalog
    and tc.constraint_schema = kcu.constraint_schema
    and tc.constraint_name = kcu.constraint_name
    and tc.table_schema = kcu.table_schema
    and tc.table_name = kcu.table_name
inner join TableColumns c
    on kcu.table_name = c.table_name
    and kcu.column_name = c.column_name,
(select @'Id2' := 0) ci
where tc.table_schema = @database_name
and tc.constraint_type = 'UNIQUE'
order by tc.constraint_name, c.Table_Name, kcu.ordinal_position;

create temporary table ForeignKeys (
    Id int,
    Name varchar(64),
    Is_One_To_One bit,
    Is_One_To_Many bit,
    Is_Many_To_Many bit,
    Is_Many_To_Many_Complete bit,
    Is_Cascade_Delete bit,
    Is_Cascade_Update bit,
    Foreign_Table_Id int,
    Foreign_Table varchar(64),
    Foreign_Column_Id int,
    Foreign_Column varchar(64),
    Is_Foreign_Column_PK bit,
    Primary_Table_Id int,
    Primary_Table varchar(64),
    Primary_Column_Id int,
    Primary_Column varchar(64),
    Is_Primary_Column_PK bit,
    Ordinal int
);

insert into ForeignKeys
select
    @'Id3' := @'Id3' + 1 as Id,
    tc.constraint_name as Name,
    0 as Is_One_To_One,
    1 as Is_One_To_Many,
    0 as Is_Many_To_Many,
    0 as Is_Many_To_Many_Complete,
    if(locate('CASCADE', rc.delete_rule) > 0, 1, 0) as Is_Cascade_Delete,
    if(locate('CASCADE', rc.update_rule) > 0, 1, 0) as Is_Cascade_Update,
    0 as Foreign_Table_Id,
    kcu.table_name as Foreign_Table,
    0 as Foreign_Column_Id,
    kcu.Column_Name as Foreign_Column,
    0 as Is_Foreign_Column_PK,
    0 as Primary_Table_Id,
    kcu.referenced_table_name as Primary_Table,
    0 as Primary_Column_Id,
    kcu.referenced_column_name as Primary_Column,
    0 as Is_Primary_Column_PK,
    kcu.ordinal_position as Ordinal
from information_schema.table_constraints tc
inner join information_schema.key_column_usage kcu
    on tc.constraint_catalog = kcu.constraint_catalog
    and tc.constraint_schema = kcu.constraint_schema
    and tc.constraint_name = kcu.constraint_name
    and tc.table_schema = kcu.table_schema
    and tc.table_name = kcu.table_name
inner join information_schema.referential_constraints rc
    on tc.constraint_catalog = rc.constraint_catalog
    and tc.constraint_schema = rc.constraint_schema
    and tc.constraint_name = rc.constraint_name
    and kcu.table_name = rc.table_name
    and kcu.referenced_table_name = rc.referenced_table_name,
(select @'Id3' := 0) ci
where tc.table_schema = @database_name
and tc.constraint_type = 'FOREIGN KEY'
order by tc.constraint_name, kcu.table_name, kcu.ordinal_position;

update ForeignKeys fk
inner join TableColumns c
    on fk.Foreign_Table = c.table_name
    and fk.Foreign_Column = c.column_name
set fk.Foreign_Table_Id = c.Table_Id,
    fk.Foreign_Column_Id = c.Column_Id;

update ForeignKeys fk
inner join TableColumns c
    on fk.Primary_Table = c.table_name
    and fk.Primary_Column = c.column_name
set fk.Primary_Table_Id = c.Table_Id,
    fk.Primary_Column_Id = c.Column_Id;

update ForeignKeys fk
inner join PrimaryKeys pk
    on fk.Foreign_Table_Id = pk.Table_Id
    and fk.Foreign_Column_Id = pk.Column_Id
set fk.Is_Foreign_Column_PK = 1;

update ForeignKeys fk
inner join PrimaryKeys pk
    on fk.Primary_Table_Id = pk.Table_Id
    and fk.Primary_Column_Id = pk.Column_Id
set fk.Is_Primary_Column_PK = 1;

-- primary keys of the foreign table
create temporary table ForeignTable_PrimaryColumns (
    Id int,       -- ForeignKeys.Id
    Column_Id int -- PrimaryKeys.Column_Id
);

insert into ForeignTable_PrimaryColumns
select fk.Id, pk.Column_Id
from PrimaryKeys pk
inner join (
    select distinct Id, Foreign_Table_Id
    from ForeignKeys
    where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1
) fk on pk.Table_Id = fk.Foreign_Table_Id;

-- foreign column that are pk columns and reference pk columns
create temporary table ForeignColumns_ForeignPK_PrimaryPK (
    Id int,       -- ForeignKeys.Id
    Column_Id int -- ForeignKeys.Foreign_Column_Id
);

insert into ForeignColumns_ForeignPK_PrimaryPK
select Id, Foreign_Column_Id as Column_Id
from ForeignKeys
where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1;

-- one-to-one
update ForeignKeys
set Is_One_To_One = 1,
    Is_One_To_Many = 0,
    Is_Many_To_Many = 0,
    Is_Many_To_Many_Complete = 0
where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1
and Id not in (
    -- foreign table with a primary key column that is not included in the foreign key
    select Id
    from (
        -- primary keys of the foreign table
        select t1.Id, t1.Column_Id
        from ForeignTable_PrimaryColumns t1

        -- except
        -- foreign column that are pk columns and reference pk columns
        left outer join ForeignColumns_ForeignPK_PrimaryPK t2 on t1.Id = t2.Id and t1.Column_Id = t2.Column_Id
        where t2.Id is null and t2.Column_Id is null
    ) t
);

-- foreign key with foreign column that are pk columns and reference pk columns
create temporary table ForeignTables_ForeignPK_PrimaryPK (
    Foreign_Table_Id int, -- ForeignKeys.Foreign_Table_Id
    Primary_Table_Id int  -- ForeignKeys.Primary_Table_Id
);

insert into ForeignTables_ForeignPK_PrimaryPK
select distinct Foreign_Table_Id, Primary_Table_Id
from ForeignKeys
where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1;

-- many-to-many
create temporary table ManyToMany (
    Id int
);

-- candidates for many-to-many
insert into ManyToMany
select distinct fk.Id
from ForeignKeys fk
inner join (
    -- foreign table with more than one reference to another table
    select Foreign_Table_Id
    from ForeignTables_ForeignPK_PrimaryPK t -- foreign key with foreign column that are pk columns and reference pk columns
    group by Foreign_Table_Id
    having count(*)>1
) j on fk.Foreign_Table_Id = j.Foreign_Table_Id;

-- many-to-many foreign keys
create temporary table ManyToMany_ForeignKeys (
    Id int,
    Foreign_Table_Id int,
    Foreign_Column_Id int
);

insert into ManyToMany_ForeignKeys
select fk.Id, fk.Foreign_Table_Id, fk.Foreign_Column_Id
from ForeignKeys fk
inner join ManyToMany mtm on fk.Id = mtm.Id
where fk.Is_Foreign_Column_PK = 1 and fk.Is_Primary_Column_PK = 1;

-- primary keys of the many-to-many tables
create temporary table ForeignColumns (
    Id int,       -- ForeignKeys.Id
    Column_Id int -- PrimaryKeys.Column_Id
);

insert into ForeignColumns
select fk.Id, pk.Column_Id
from PrimaryKeys pk
inner join (
    select distinct Id, Foreign_Table_Id
    from ManyToMany_ForeignKeys
) fk on pk.Table_Id = fk.Foreign_Table_Id;

delete fc.*
from ForeignColumns fc
inner join (
    select mtm.Id, c.Foreign_Column_Id
    from ManyToMany mtm cross join (
        select Foreign_Column_Id
        from ManyToMany_ForeignKeys
    ) c
) c on fc.Id = c.Id and fc.Column_Id = c.Foreign_Column_Id;

-- not many-to-many
-- foreign table with a primary key column that is not included in the foreign key
delete mtm.*
from ManyToMany mtm
inner join ForeignColumns fc on mtm.Id = fc.Id;

-- many-to-many
update ForeignKeys fk
inner join ManyToMany mtm on fk.Id = mtm.Id
set fk.Is_One_To_One = 0,
    fk.Is_One_To_Many = 0,
    fk.Is_Many_To_Many = 1,
    fk.Is_Many_To_Many_Complete = 1;

-- many-to-many columns
create temporary table ManyToMany_Columns (
    Foreign_Table_Id int,
    Foreign_Column_Id int
);

insert into ManyToMany_Columns
select Foreign_Table_Id, Foreign_Column_Id
from ForeignKeys
where Is_Many_To_Many = 1;

-- many-to-many with extra columns
create temporary table ManyToMany_With_Extra_Columns (
    Foreign_Table_Id int
);

insert into ManyToMany_With_Extra_Columns
select Foreign_Table_Id
from (
    -- the columns of the many-to-many join table
    select
        mtm.Foreign_Table_Id,
        c.Column_Id
    from TableColumns c
    inner join (
        select distinct Foreign_Table_Id
        from ForeignKeys
        where Is_Many_To_Many = 1
    ) mtm on mtm.Foreign_Table_Id = c.Table_Id

    -- except
    -- the columns of the many-to-many foreign key
    left outer join ManyToMany_Columns mtmc on mtm.Foreign_Table_Id = mtmc.Foreign_Table_Id and c.Column_Id = mtmc.Foreign_Column_Id
    where mtmc.Foreign_Table_Id is null and mtmc.Foreign_Column_Id is null
) t;

-- many-to-many join table is not complete
-- there is at least one more column that is not part of the pk
update ForeignKeys fk
inner join ManyToMany_With_Extra_Columns mtmec on fk.Foreign_Table_Id = mtmec.Foreign_Table_Id
set fk.Is_Many_To_Many_Complete = 0;

select * from PrimaryKeys order by Name, Table_Name, Ordinal;
select * from UniqueKeys order by Name, Table_Name, Ordinal;
select * from ForeignKeys order by Name, Foreign_Table, Ordinal;

drop temporary table TableColumns;
drop temporary table PrimaryKeys;
drop temporary table UniqueKeys;
drop temporary table ForeignKeys;
drop temporary table ForeignTable_PrimaryColumns;
drop temporary table ForeignColumns_ForeignPK_PrimaryPK;
drop temporary table ForeignTables_ForeignPK_PrimaryPK;
drop temporary table ManyToMany;
drop temporary table ManyToMany_ForeignKeys;
drop temporary table ForeignColumns;
drop temporary table ManyToMany_Columns;
drop temporary table ManyToMany_With_Extra_Columns;
