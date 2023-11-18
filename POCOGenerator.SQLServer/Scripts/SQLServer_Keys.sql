declare @PrimaryKeys table (
    Id int,
    [Name] nvarchar(128),
    [Schema_Id] int,
    [Schema] nvarchar(128),
    Table_Id int,
    Table_Name nvarchar(128),
    Ordinal tinyint,
    Column_Id int,
    Column_Name nvarchar(128),
    Is_Descending bit,
    Is_Identity bit,
    Is_Computed bit
)

insert into @PrimaryKeys
select
    Id = kc.object_id,
    [Name] = kc.name,
    [Schema_Id] = s.schema_id,
    [Schema] = s.name,
    Table_Id = t.object_id,
    Table_Name = t.name,
    Ordinal = ic.key_ordinal,
    Column_Id = c.column_id,
    Column_Name = c.name,
    Is_Descending = ic.is_descending_key,
    Is_Identity = c.is_identity,
    Is_Computed = c.is_computed
from sys.key_constraints kc
inner join sys.tables t on kc.parent_object_id = t.object_id
inner join sys.schemas s on t.schema_id = s.schema_id
inner join sys.index_columns ic on t.object_id = ic.object_id and kc.unique_index_id = ic.index_id and kc.type = 'PK'
inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
order by [Name], [Schema], Table_Name, Ordinal

declare @UniqueKeys table (
    Id int,
    [Name] nvarchar(128),
    [Schema_Id] int,
    [Schema] nvarchar(128),
    Table_Id int,
    Table_Name nvarchar(128),
    Ordinal tinyint,
    Column_Id int,
    Column_Name nvarchar(128),
    Is_Descending bit,
    Is_Identity bit,
    Is_Computed bit
)

insert into @UniqueKeys
select
    Id = kc.object_id,
    [Name] = kc.name,
    [Schema_Id] = s.schema_id,
    [Schema] = s.name,
    Table_Id = t.object_id,
    Table_Name = t.name,
    Ordinal = ic.key_ordinal,
    Column_Id = c.column_id,
    Column_Name = c.name,
    Is_Descending = ic.is_descending_key,
    Is_Identity = c.is_identity,
    Is_Computed = c.is_computed
from sys.key_constraints kc
inner join sys.tables t on kc.parent_object_id = t.object_id
inner join sys.schemas s on t.schema_id = s.schema_id
inner join sys.index_columns ic on t.object_id = ic.object_id and kc.unique_index_id = ic.index_id and kc.type = 'UQ'
inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
order by [Name], [Schema], Table_Name, Ordinal

declare @ForeignKeys table (
    Id int,
    [Name] nvarchar(128),
    Is_One_To_One bit,
    Is_One_To_Many bit,
    Is_Many_To_Many bit,
    Is_Many_To_Many_Complete bit,
    Is_Cascade_Delete bit,
    Is_Cascade_Update bit,
    Foreign_Schema_Id int,
    Foreign_Schema nvarchar(128),
    Foreign_Table_Id int,
    Foreign_Table nvarchar(128),
    Foreign_Column_Id int,
    Foreign_Column nvarchar(128),
    Is_Foreign_Column_PK bit,
    Primary_Schema_Id int,
    Primary_Schema nvarchar(128),
    Primary_Table_Id int,
    Primary_Table nvarchar(128),
    Primary_Column_Id int,
    Primary_Column nvarchar(128),
    Is_Primary_Column_PK bit,
    Ordinal int
)

insert into @ForeignKeys
select
    Id = fk.object_id,
    [Name] = fk.name,
    Is_One_To_One = 0,
    Is_One_To_Many = 1,
    Is_Many_To_Many = 0,
    Is_Many_To_Many_Complete = 0,
    Is_Cascade_Delete = (case when fk.delete_referential_action = 1 then 1 else 0 end),
    Is_Cascade_Update = (case when fk.update_referential_action = 1 then 1 else 0 end),
    Foreign_Schema_Id = fs.schema_id,
    Foreign_Schema = fs.name,
    Foreign_Table_Id = ft.object_id,
    Foreign_Table = ft.name,
    Foreign_Column_Id = fc.parent_column_id,
    Foreign_Column = col_name(fc.parent_object_id, fc.parent_column_id),
    Is_Foreign_Column_PK = 0,
    Primary_Schema_Id = ps.schema_id,
    Primary_Schema = ps.name,
    Primary_Table_Id = pt.object_id,
    Primary_Table = pt.name,
    Primary_Column_Id = fc.referenced_column_id,
    Primary_Column = col_name(fc.referenced_object_id, fc.referenced_column_id),
    Is_Primary_Column_PK = 0,
    Ordinal = fc.constraint_column_id
from sys.foreign_keys fk
inner join sys.tables ft on fk.parent_object_id = ft.object_id
inner join sys.schemas fs on ft.schema_id = fs.schema_id
inner join sys.tables pt on fk.referenced_object_id = pt.object_id
inner join sys.schemas ps on pt.schema_id = ps.schema_id
inner join sys.foreign_key_columns fc on fk.object_id = fc.constraint_object_id
where fk.type = 'F'
order by [Name], Foreign_Schema, Foreign_Table, Ordinal

update fk
set fk.Is_Foreign_Column_PK = 1
from @ForeignKeys fk
inner join @PrimaryKeys pk
    on fk.Foreign_Schema_Id = pk.[Schema_Id]
    and fk.Foreign_Table_Id = pk.Table_Id
    and fk.Foreign_Column_Id = pk.Column_Id

update fk
set fk.Is_Primary_Column_PK = 1
from @ForeignKeys fk
inner join @PrimaryKeys pk
    on fk.Primary_Schema_Id = pk.[Schema_Id]
    and fk.Primary_Table_Id = pk.Table_Id
    and fk.Primary_Column_Id = pk.Column_Id

-- one-to-one
update fk
set Is_One_To_One = 1,
    Is_One_To_Many = 0,
    Is_Many_To_Many = 0,
    Is_Many_To_Many_Complete = 0
from @ForeignKeys fk
where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1
and Id not in (
    -- foreign table with a primary key column that is not included in the foreign key
    select Id
    from (
        -- primary keys of the foreign table
        select fk.Id, pk.Column_Id
        from @PrimaryKeys pk
        inner join (
            select distinct Id, Foreign_Schema_Id, Foreign_Table_Id
            from @ForeignKeys
            where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1
        ) fk on pk.[Schema_Id] = fk.Foreign_Schema_Id and pk.Table_Id = fk.Foreign_Table_Id

        except

        -- foreign column that are pk columns and reference pk columns
        select Id, Column_Id = Foreign_Column_Id
        from @ForeignKeys
        where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1
    ) t
)

declare @ManyToMany table (
    Id int
)

-- candidates for many-to-many
insert into @ManyToMany
select distinct fk.Id
from @ForeignKeys fk
inner join (
    -- foreign table with more than one reference to another table
    select Foreign_Schema_Id, Foreign_Table_Id
    from (
        -- foreign key with foreign column that are pk columns and reference pk columns
        select distinct Foreign_Schema_Id, Foreign_Table_Id, Primary_Schema_Id, Primary_Table_Id
        from @ForeignKeys
        where Is_Foreign_Column_PK = 1 and Is_Primary_Column_PK = 1
    ) t
    group by Foreign_Schema_Id, Foreign_Table_Id
    having count(*)>1
) j on fk.Foreign_Schema_Id = j.Foreign_Schema_Id and fk.Foreign_Table_Id = j.Foreign_Table_Id

declare @ForeignColumns table (
    Id int,
    Column_Id int
)

-- primary keys of the many-to-many tables
insert into @ForeignColumns
select fk.Id, pk.Column_Id
from @PrimaryKeys pk
inner join (
    select distinct fk.Id, fk.Foreign_Schema_Id, fk.Foreign_Table_Id
    from @ForeignKeys fk
    inner join @ManyToMany mtm on fk.Id = mtm.Id
    where fk.Is_Foreign_Column_PK = 1 and fk.Is_Primary_Column_PK = 1
) fk on pk.[Schema_Id] = fk.Foreign_Schema_Id and pk.Table_Id = fk.Foreign_Table_Id

delete from fc
from @ForeignColumns fc
inner join (
    select mtm.Id, c.Foreign_Column_Id
    from @ManyToMany mtm cross join (
        select fk.Foreign_Column_Id
        from @ForeignKeys fk
        inner join @ManyToMany mtm on fk.Id = mtm.Id
        where fk.Is_Foreign_Column_PK = 1 and fk.Is_Primary_Column_PK = 1
    ) c
) c on fc.Id = c.Id and fc.Column_Id = c.Foreign_Column_Id

-- not many-to-many
-- foreign table with a primary key column that is not included in the foreign key
delete from mtm
from @ManyToMany mtm
inner join @ForeignColumns fc on mtm.Id = fc.Id

-- many-to-many
update fk
set fk.Is_One_To_One = 0,
    fk.Is_One_To_Many = 0,
    fk.Is_Many_To_Many = 1,
    fk.Is_Many_To_Many_Complete = 1
from @ForeignKeys fk
inner join @ManyToMany mtm on fk.Id = mtm.Id

-- many-to-many join table is not complete
-- there is at least one more column that is not part of the pk
update fk
set fk.Is_Many_To_Many_Complete = 0
from @ForeignKeys fk
inner join (
    -- the columns of the many-to-many join table
    select
        mtm.Foreign_Schema_Id,
        mtm.Foreign_Table_Id,
        Column_Id = c.column_id
    from sys.columns c
    inner join sys.sysobjects so on c.object_id = so.id
    inner join sys.schemas ss on so.uid = ss.schema_id
    inner join (
        select distinct Foreign_Schema_Id, Foreign_Table_Id
        from @ForeignKeys
        where Is_Many_To_Many = 1
    ) mtm on mtm.Foreign_Schema_Id = ss.schema_id and mtm.Foreign_Table_Id = c.object_id

    except

    -- the columns of the many-to-many foreign key
    select Foreign_Schema_Id, Foreign_Table_Id, Column_Id = Foreign_Column_Id
    from @ForeignKeys fk
    where Is_Many_To_Many = 1
) t on fk.Foreign_Schema_Id = t.Foreign_Schema_Id and fk.Foreign_Table_Id = t.Foreign_Table_Id

select * from @PrimaryKeys order by [Name], [Schema], Table_Name, Ordinal
select * from @UniqueKeys order by [Name], [Schema], Table_Name, Ordinal
select * from @ForeignKeys order by [Name], Foreign_Schema, Foreign_Table, Ordinal