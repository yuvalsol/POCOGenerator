![POCO Generator UI](./Solution%20Items/Images/POCOGeneratorUI.jpg "POCO Generator UI")

# POCO Generator

POCO Generator traverses databases and generates POCOs from various data objects.
POCO Generator supports SQL Server and MySQL.

There are five types of database objects that POCOs are generated from:

- Tables
- Views
- Stored Procedures
- Table-valued Functions
- User-Defined Table Types (TVP)

POCO Generator also detects primary keys, foreign keys, unique keys, indexes and more.

POCO Generator requires .NET Framework 4.6.2 Runtime.

# Disclaimer

<span style="color: DarkRed;">**One person reported data loss after using this utility (Comments section in the original article on CodeProject [Potential Data Loss](https://www.codeproject.com/Articles/892233/POCO-Generator?msg=5619954 "Potential Data Loss")). Some tables were cleared of all their records but they were able to restore them from backup. This error is NOT resolved despite my efforts to replicate and solve it. Backup your database before using this utility or use it at your own risk.**</span>

# POCO Generator UI

The RDBMS tree lists all the databases on that instance and each database lists its data objects - tables, views, procedures, functions & TVPs.
The checkboxes on the tree are for picking specific database objects.
The upper right side of the window shows the current generated POCOs, based on what is selected on the tree.
The setting panels, at the bottom, manipulate how the POCOs are constructed and handle exporting them to files.

![Person Table and POCO](./Solution%20Items/Images/PersonTableAndPOCO.jpg "Person Table and POCO")

## POCO Settings

![POCO Settings](./Solution%20Items/Images/POCOSettings.jpg "POCO Settings")

These settings determine the structure of the POCO.

**Properties** - Data members are constructed as properties (getter & setter).

**Fields** - Data members are constructed as fields.

**Partial Class** - Add `partial` modifier to the class.

**Virtual Properties** - Add `virtual` modifier to properties.

**Override Properties** - Add `override` modifier to properties.

**Struct Types Nullable** - `struct` data members will be constructed as nullable (`int?`) even if they are not nullable in the database.

**Column Defaults** - Add data member initialization based on the column's default value in the database. Default value that can't be handled properly will be commented.

**Comments** - Add a comment, to data members, about the original database column type and whether the column is nullable.

**Comments Without null** - Add a comment, to data members, about the original database column type.

**using** - Add `using` statements at the beginning of all the POCOs. If a custom namespace is set (**Namespace** setting), the `using` statements are placed _outside_ the namespace declaration.

**using Inside Namespace** - If a custom namespace is set (**Namespace** setting), the `using` statements are placed _inside_ the namespace declaration.

**Namespace** - Wraps all the POCOs with a custom namespace.

**Inherit** - Add a comma-delimited list of inherit class and interfaces.

**New Line Between Members** - Add empty lines between POCO data members.

**Complex Types** - Reverse-engineer existing Entity Framework's complex types in the database. Code-first Entity Framework prefixes the column name with the name of the complex type. More in this article [Associations in EF Code First: Part 2 – Complex Types](https://weblogs.asp.net/manavi/associations-in-ef-4-1-code-first-part-2-complex-types "Associations in EF Code First: Part 2 – Complex Types"). A limitation of POCO Generator is it doesn't detect nested complex types (complex type in a complex type).

```sql
CREATE TABLE dbo.Customers (
    CustomerId int NOT NULL,
    CustomerName nvarchar(50) NOT NULL,
    ShippingAddress_Street  nvarchar(50) NOT NULL,
    ShippingAddress_City    nvarchar(50) NOT NULL,
    ShippingAddress_ZipCode nvarchar(50) NULL,
    BillingAddress_Street   nvarchar(50) NOT NULL,
    BillingAddress_City     nvarchar(50) NOT NULL,
    BillingAddress_ZipCode  nvarchar(50) NULL
)

CREATE TABLE dbo.Users (
    UserId int NOT NULL,
    UserName nvarchar(50) NOT NULL,
    Address_Street  nvarchar(50) NOT NULL,
    Address_City    nvarchar(50) NOT NULL,
    Address_ZipCode nvarchar(50) NULL
)
```

When Complex Types setting is enabled:

```cs
public class Customers
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public Address ShippingAddress { get; set; }
    public Address BillingAddress { get; set; }
}

public class Users
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
}
```

How POCO Generator detects and builds complex types:

1. For every table, select columns that
- have underscore in the column name
- are not part of the table's primary key
- are not part of unique key
- are not part of foreign key
- are not referenced by a foreign key from another table
- are not part of an index
- are not identity column
2. Split the column on the _first_ underscore. The prefix is the name of the complex type table. The suffix is the name of the complex type column.
3. Group the complex type columns by the name of the complex type table.
4. Consolidate complex types that may appear in several database tables. Two complex types are the same if they have the same complex type columns and no more. Two complex type columns are the same if they have
- same name
- same data type
- same precision
- same unsigned property
- same nullable property
- same computed property
- same column default

**Enum Type** - Determines how enum and set type columns are generated. This setting is applicable when the RDBMS, such as MySQL, supports enum types.

If it is set to `string`, the data member type will be `string` for both enum and set type columns.

If it is set to `enum ushort`, enum type column will be generated as enumeration of type `System.UInt16` (`ushort`) and set type column will be generated as bitwise enumeration of type `System.UInt64` (`ulong`).

If it is set to `enum int`, enum and set type column will be generated as enumeration of type `System.Int32` (`int`).

```sql
CREATE TABLE NumbersTable (
    Number ENUM('One', 'Two', 'Three')
    Numbers SET('One', 'Two', 'Three')
);
```

When Enum Type is set to `enum ushort`

```cs
                                [Flags]
public enum Number : ushort     public enum Numbers : ulong
{                               {
    One = 1,                        One = 1ul,
    Two = 2,                        Two = 1ul << 1,
    Three = 3                       Three = 1ul << 2
}                               }
```

When Enum Type is set to `enum int`

```cs
                                [Flags]
public enum Number : int        public enum Numbers : int
{                               {
    One = 1,                        One = 1,
    Two = 2,                        Two = 1 << 1,
    Three = 3                       Three = 1 << 2
}                               }
```

## Class Name Settings

![Class Name Settings](./Solution%20Items/Images/ClassNameSettings.jpg "Class Name Settings")

The name of the POCO class is set to the name of the data object, whether it is a valid C# class name or not. These settings modify the initial class name.

**Singular** - Change the class name from plural to singular. Applicable only for tables, views & TVPs.

**Include DB** - Add the database name.

**DB Separator** - Add a separator after the database name.

**Include Schema** - Add the schema name.

**Ignore dbo Schema** - If the schema name is `dbo`, don't add the schema name.

**Schema Separator** - Add a separator after the schema name.

**Words Separator** - Add a separator between words. Word are text between underscores or in a camel case.

The class name `EmployeeDepartmentHistory` has 3 words in it, `Employee`, `Department` & `History`.
The class name `Product_Category` has 2 words, `Product` & `Category`.

**CamelCase** - Change class name to camel case.

**UPPER CASE** - Change class name to upper case.

**lower case** - Change class name to lower case.

**Search**, **Replace** - Search and replace on the class name. Search is case-sensitive.

**Ignore Case** - Enable case-insensitive search.

**Fixed Name** - Set the name of the class to a fixed name.

**Prefix** - Add prefix text to the class name.

**Suffix** - Add suffix text to the class name.

## Navigation Properties Settings

![Navigation Properties Settings](./Solution%20Items/Images/NavigationPropertiesSettings.jpg "Navigation Properties Settings")

These settings enable navigation properties and determine how they are constructed.

**Navigation Properties** - Add navigation properties and constructor initialization, if necessary.

**Comments** - Add a comment about the underline foreign key of the navigation property.

**Virtual** - Add `virtual` modifier to the navigation properties.

**Override** - Add `override` modifier to the navigation properties.

**Many-to-Many Join Table** - In a Many-to-Many relationship, the join table is hidden by default. When this setting is enabled, the join table is forcefully rendered.

**List**, **ICollection**, **IEnumerable** - When a navigation property is a collection, this setting determine what the type of collection it is.

## EF Annotations Settings

![EF Annotations Settings](./Solution%20Items/Images/EFAnnotationsSettings.jpg "EF Annotations Settings")

These settings add Code-first Entity Framework attributes to POCO classes. More about EF annotations on this page [Code First Data Annotations](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations "Code First Data Annotations").

**EF** - Add EF main attributes.
- **Table** attribute to class declaration. `[Table("Production.Product")]`
- **Key** attribute to primary key properties. `[Key]`
- **Column** attribute to composite primary key properties with `Order` set to the order of the key in the composite primary key. `[Column(Order = 1)]`
- **MaxLength** attribute to `string` properties. `[MaxLength(50)]`
- **Timestamp** attribute to `timestamp` properties. `[Timestamp]`
- **DatabaseGenerated** attribute to Identity & Computed properties. `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]`

**Column** - Add `Column` attribute with the database column's name and type. `[Column(Name = "ProductID", TypeName = "int")]`

**Required** - Add `Required` attribute to properties that are not nullable. `[Required]`

**Required with ErrorMessage** - Same as **Required** and also add an error message. `[Required(ErrorMessage = "Product ID is required")]`

**ConcurrencyCheck** - Add `ConcurrencyCheck` attribute to `Timestamp` and `RowVersion` properties. `[ConcurrencyCheck]`

**StringLength** - Add `StringLength` attribute to `string` properties. This attribute has no bearing on the database, unlike `MaxLength`. It is used as a user input validation. `[StringLength(50)]`

**Display** - Add `Display` attribute. `[Display(Name = "Product ID")]`

**Description** - Add `Description` attribute to table and columns. Descriptions are retrieved from SQL Server `sys.extended_properties` table or MySQL `information_schema` table. `[Description("table description")]`

**ComplexType** - Add `ComplexType` attribute to complex types. `[ComplexType]`

**Index (EF6)** - Add `Index` attribute to data members that are part of an index. If the index is unique or clustered, the corresponding properties are set accordingly. `Index` attribute is applicable starting from EF6. `[Index("IX_ProductName", IsUnique = true)]`

**ForeignKey & InverseProperty** - Add `ForeignKey` and `InverseProperty` attributes to navigation properties. `[ForeignKey("ProductID")]` `[InverseProperty("Product")]`

## Export To Files Settings

![Export To Files Settings](./Solution%20Items/Images/ExportToFilesSettings.jpg "Export To Files Settings")

Save POCOs to files. These settings determine how the POCOs will be grouped to files and what the directory structure, where the files are saved, will be.

**Single File** - All the POCOs are saved to one file. The file name is the name of the database.

**Multiple Files - Single Folder** - Each POCO is saved to its own file. All the files are saved to the root folder.

**Multiple Files - Relative Folders** - Each POCO is saved to its own file. The files are saved to this directory structure

<pre>
Server <i>[(LocalDB)_MSSQLLocalDB]</i>
    Namespace <i>[Namespace setting if set]</i>
        Database <i>[AdventureWorks2014]</i>
            Tables
                Schema <i>[Person, Sales]</i>
                    *.cs <i>[Person.cs, Customer.cs]</i>
            Views
                Schema
                    *.cs
            Procedures
                Schema
                    *.cs
            Functions
                Schema
                    *.cs
            TVPs
                Schema
                    *.cs
</pre>

Each POCO is wrapped in the following namespace structure **Namespace.Database.Data Object.Schema**.

Example: If the Namespace setting is set to "CustomNamespace", the Person.Person table is wrapped in this namespace

```cs
namespace CustomNamespace.AdventureWorks2014.Tables.Person
{
    public class Person
    {
    }
}
```

Example: If the Namespace setting is not set, the Sales.Customer table is wrapped in this namespace

```cs
namespace AdventureWorks2014.Tables.Sales
{
    public class Customer
    {
    }
}
```

**File Name** - This setting determines the file names of the exported POCOs. It is applicable when exporting to multiple files. For RDBMS that doesn't support schema (MySQL), the options with **Schema** are not available.

## Type Mapping

The following lists the mapping from RDBMS data types to .NET data types.

More about SQL Server data type mappings on this page [SQL Server Data Type Mappings](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings "SQL Server Data Type Mappings").

|    SQL Server    |                   .NET                   |
|------------------|------------------------------------------|
| bigint           | long                                     |
| binary           | byte[]                                   |
| bit              | bool                                     |
| char             | string                                   |
| date             | DateTime                                 |
| datetime         | DateTime                                 |
| datetime2        | DateTime                                 |
| datetimeoffset   | DateTimeOffset                           |
| decimal          | decimal                                  |
| filestream       | byte[]                                   |
| float            | double                                   |
| geography        | Microsoft.SqlServer.Types.SqlGeography   |
| geometry         | Microsoft.SqlServer.Types.SqlGeometry    |
| hierarchyid      | Microsoft.SqlServer.Types.SqlHierarchyId |
| image            | byte[]                                   |
| int              | int                                      |
| money            | decimal                                  |
| nchar            | string                                   |
| ntext            | string                                   |
| numeric          | decimal                                  |
| nvarchar         | string                                   |
| real             | float                                    |
| rowversion       | byte[]                                   |
| smalldatetime    | DateTime                                 |
| smallint         | short                                    |
| smallmoney       | decimal                                  |
| sql_variant      | object                                   |
| text             | string                                   |
| time             | TimeSpan                                 |
| timestamp        | byte[]                                   |
| tinyint          | byte                                     |
| uniqueidentifier | Guid                                     |
| varbinary        | byte[]                                   |
| varchar          | string                                   |
| xml              | string                                   |
| else             | object                                   |

More about MySQL data type mappings on this page [Entity Framework Data Type Mapping](https://docs.devart.com/dotconnect/mysql/datatypemapping.html "Entity Framework Data Type Mapping") and data type mappings from SQL Server to MySQL on this page [Microsoft SQL Server Type Mapping](https://dev.mysql.com/doc/workbench/en/wb-migration-database-mssql-typemapping.html "Microsoft SQL Server Type Mapping").

|                   MySQL                    |              .NET              |
|--------------------------------------------|--------------------------------|
| bigint                                     | long                           |
| bigint unsigned/serial                     | decimal                        |
| binary/char byte                           | byte[]                         |
| bit                                        | bool                           |
| blob                                       | byte[]                         |
| char/character                             | string                         |
| date                                       | DateTime                       |
| datetime                                   | DateTime                       |
| decimal/numeric/dec/fixed                  | decimal                        |
| double                                     | double                         |
| double unsigned                            | decimal                        |
| enum                                       | string                         |
| float                                      | float                          |
| float unsigned                             | decimal                        |
| geometry/geometrycollection/geomcollection | System.Data.Spatial.DbGeometry |
| int/integer                                | int                            |
| int/integer unsigned                       | long                           |
| json                                       | string                         |
| linestring/multilinestring                 | System.Data.Spatial.DbGeometry |
| longblob                                   | byte[]                         |
| longtext                                   | string                         |
| mediumblob                                 | byte[]                         |
| mediumint                                  | int                            |
| mediumint unsigned                         | int                            |
| mediumtext                                 | string                         |
| nchar/national char                        | string                         |
| nvarchar/national varchar                  | string                         |
| point/multipoint                           | System.Data.Spatial.DbGeometry |
| polygon/multipolygon                       | System.Data.Spatial.DbGeometry |
| real (REAL_AS_FLOAT off)                   | double                         |
| real (REAL_AS_FLOAT on)                    | float                          |
| set                                        | string                         |
| smallint                                   | short                          |
| smallint unsigned                          | int                            |
| text                                       | string                         |
| time                                       | TimeSpan                       |
| timestamp                                  | DateTime                       |
| tinyblob                                   | byte[]                         |
| tinyint(1)/bool/boolean                    | bool                           |
| tinyint                                    | sbyte                          |
| tinyint unsigned                           | byte                           |
| tinytext                                   | string                         |
| varbinary                                  | byte[]                         |
| varchar/character varying                  | string                         |
| year                                       | short                          |
| else                                       | object                         |

## Tables and Foreign Keys

The context menu of a table object shows several options for quickly selecting other tables that are referenced from or referencing to the selected table.

![Table Context Menu](./Solution%20Items/Images/TableContextMenu.jpg "Table Context Menu")

**Referenced From** select all other tables that are connected from the selected table by foreign keys.

**Referencing To** select all other tables that are connected to the selected table by foreign keys.

**Recursively Accessible From & To** select all other tables that are directly or indirectly connected from or connected to the selected table.

## Filter Settings

The context menu of a data group (Tables, Views...) shows the filter settings. The filter selects, or excludes, specific data objects, within that data group, by their name and schema.

![Filter Settings](./Solution%20Items/Images/FilterSettings.jpg "Filter Settings")

# Schemas

The process of retrieving the schema of data objects (tables, views...) is mainly done through `GetSchema()` methods from `DbConnection` class. The class `DbConnection`, which `SqlConnection` and `MySqlConnection` inherit from, has several `GetSchema()` methods which do exactly as their name suggests. They return the schema information from the specified data source. You can pass, to the `GetSchema()` method, the type of object that you're looking for and list of restrictions which are usually used to filter on database name, schema name and the name of the object. A full list of object types and restricts can be found on these pages [SQL Server Schema Collections
](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-schema-collections "SQL Server Schema Collections
") and [Schema Restrictions](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/schema-restrictions "Schema Restrictions").

## Tables and Views

The schema type for both tables and views is "`Tables`". For tables, set the table type restriction to "`BASE TABLE`". For views, set the table type restriction to "`VIEW`".

Tables:

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable allTables = connection.GetSchema("Tables",
        new string[] { database_name, null, null, "BASE TABLE" });
    DataTable specificTable = connection.GetSchema("Tables",
        new string[] { database_name, schema_name, table_name, "BASE TABLE" });
}
```

and Views:

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable allViews = connection.GetSchema("Tables",
        new string[] { database_name, null, null, "VIEW" });
    DataTable specificView = connection.GetSchema("Tables",
        new string[] { database_name, schema_name, view_name, "VIEW" });
}
```

For each table, we query again for its list of columns. The schema type is "`Columns`" for SQL Server and for MySQL.

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable routineParameters = connection.GetSchema("Columns",
        new string[] { database_name, schema_name, table_name, null });
}
```

And for views, the schema type is "`Columns`" for SQL Server and "`ViewColumns`" for MySQL.

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable routineParameters = connection.GetSchema("Columns",
        new string[] { database_name, schema_name, view_name, null });
}
```

## User-Defined Table Types (TVP)

TVP schema can't be retrieved through `GetSchema()` methods or at least not retrieved reliably. Getting a TVP schema requires querying on the SQL Server side (TVPs are not supported on MySQL). The first query gets all the TVPs on the database.

```sql
select
    tvp_schema = ss.name,
    tvp_name = stt.name,
    stt.type_table_object_id
from sys.table_types stt
inner join sys.schemas ss on stt.schema_id = ss.schema_id
```

For each TVP, we query again for its list of columns. The `@tvp_id` parameter is the `type_table_object_id` column from the previous query.

```sql
select
    sc.*,
    data_type = st.name
from sys.columns sc
inner join sys.types st
    on sc.system_type_id = st.system_type_id
    and sc.user_type_id = st.user_type_id
where sc.object_id = @tvp_id
```

## Stored Procedures and Table-valued Functions

The schema type for both stored procedures and table-valued functions is "`Procedures`". For stored procedures, set the routine type restriction to "`PROCEDURE`". For table-valued functions, set the routine type restriction to "`FUNCTION`". Table-valued functions are not supported on MySQL.

Stored Procedures:

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable allProcedures = connection.GetSchema("Procedures",
        new string[] { database_name, null, null, "PROCEDURE" });
    DataTable specificProcedure = connection.GetSchema("Procedures",
        new string[] { database_name, schema_name, procedure_name, "PROCEDURE" });
}
```

and Table-valued Functions:

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable allFunctions = connection.GetSchema("Procedures",
        new string[] { database_name, null, null, "FUNCTION" });
    DataTable specificFunction = connection.GetSchema("Procedures",
        new string[] { database_name, schema_name, function_name, "FUNCTION" });
}
```

For each routine, we query again for its list of parameters. The schema type is "`ProcedureParameters`" for SQL Server and "`Procedure Parameters`" for MySQL.

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    DataTable routineParameters = connection.GetSchema("ProcedureParameters",
        new string[] { database_name, routine_schema, routine_name, null });
}
```

At this point, we can filter out anything that is not a table-valued function, meaning removing scalar functions. A scalar function has a single return parameter which is the result of the function and that's how we find them.

Once we have the routine parameters, we build an empty `SqlParameter` (or `MySqlParameter` for MySQL) for each one. An empty parameter is a parameter with `DBNull.Value` set as its value. For a TVP parameter (for SQL Server), we build a parameter with `SqlDbType.Structured` type and an empty `DataTable` as its value.

This is a very abridged code snippet of how a `SqlParameter` is built.

```cs
SqlParameter sqlParameter = new SqlParameter();

// name
sqlParameter.ParameterName = parameter_name;

// empty value
sqlParameter.Value = DBNull.Value;

// type
switch (data_type)
{
    case "bigint": sqlParameter.SqlDbType = SqlDbType.BigInt; break;
    case "binary": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
    ....
    case "varchar": sqlParameter.SqlDbType = SqlDbType.VarChar; break;
    case "xml": sqlParameter.SqlDbType = SqlDbType.Xml; break;
}

// size for string type
// character_maximum_length comes from the parameter schema
if (data_type == "binary" || data_type == "varbinary" ||
    data_type == "char" || data_type == "nchar" ||
    data_type == "nvarchar" || data_type == "varchar")
{
    if (character_maximum_length == -1 || character_maximum_length > 0)
        sqlParameter.Size = character_maximum_length;
}

// direction
if (parameter_mode == "IN")
    sqlParameter.Direction = ParameterDirection.Input;
else if (parameter_mode == "INOUT")
    sqlParameter.Direction = ParameterDirection.InputOutput;
else if (parameter_mode == "OUT")
    sqlParameter.Direction = ParameterDirection.Output;
```

Now, we are ready to get the routine columns. We use `SqlDataReader.GetSchemaTable()` method to get the routine schema with `CommandBehavior.SchemaOnly` flag.

For stored procedures, the command type is set to `CommandType.StoredProcedure`.

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = new SqlCommand())
    {
        command.Connection = connection;
        command.CommandText = string.Format("[{0}].[{1}]", routine_schema, routine_name);
        command.CommandType = CommandType.StoredProcedure;

        // for each routine parameter, build it and add it to command.Parameters

        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
        {
            DataTable schemaTable = reader.GetSchemaTable();
        }
    }
}
```

For table-valued functions, we construct a query that selects all the columns from the function.

```cs
using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = new SqlCommand())
    {
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        command.CommandText =
            string.Format("select * from [{0}].[{1}](", routine_schema, routine_name);

        // for each routine parameter, build it and add it
        // to command.Parameters and add its name to command.CommandText

        command.CommandText += ")";

        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
        {
            DataTable schemaTable = reader.GetSchemaTable();
        }
    }
}
```

## Stored Procedures With Many Result Sets

There is no way to determine if a stored procedure returns more than one result set. During the process of retrieving the schema of a stored procedure, only the **first** result set is returned. There is no way to get to the schema of any result set after the first one.

## Primary Keys and Foreign Keys

Retrieving information about primary keys and foreign keys must be done by SQL queries. The `GetSchema()` methods are not detailed enough for that.

The following are abridged queries for SQL Server but the full SQL script retrieves information about primary keys, unique keys, foreign keys, one-to-many relationships, one-to-one relationships and many-to-many relationships. Script for SQL Server is  [SQLServer_Keys.sql](POCOGenerator.SQLServer/Scripts/SQLServer_Keys.sql) and script for MySQL is [MySQL_Keys.sql](POCOGenerator.MySQL/Scripts/MySQL_Keys.sql).

Query for primary keys on SQL Server. The table, that holds information about primary keys, is `sys.key_constraints`.

```sql
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
inner join sys.index_columns ic
    on t.object_id = ic.object_id
    and kc.unique_index_id = ic.index_id
    and kc.type = 'PK'
inner join sys.columns c
    on ic.object_id = c.object_id
    and ic.column_id = c.column_id
order by [Name], [Schema], Table_Name, Ordinal
```

Query for foreign keys on SQL Server. The tables, that hold information about foreign keys, are `sys.foreign_keys` and `sys.foreign_key_columns`.

```sql
select
    Id = fk.object_id,
    [Name] = fk.name,
    Is_Cascade_Delete = (case when fk.delete_referential_action = 1 then 1 else 0 end),
    Is_Cascade_Update = (case when fk.update_referential_action = 1 then 1 else 0 end),
    Foreign_Schema_Id = fs.schema_id,
    Foreign_Schema = fs.name,
    Foreign_Table_Id = ft.object_id,
    Foreign_Table = ft.name,
    Foreign_Column_Id = fc.parent_column_id,
    Foreign_Column = col_name(fc.parent_object_id, fc.parent_column_id),
    Primary_Schema_Id = ps.schema_id,
    Primary_Schema = ps.name,
    Primary_Table_Id = pt.object_id,
    Primary_Table = pt.name,
    Primary_Column_Id = fc.referenced_column_id,
    Primary_Column = col_name(fc.referenced_object_id, fc.referenced_column_id),
    Ordinal = fc.constraint_column_id
from sys.foreign_keys fk
inner join sys.tables ft on fk.parent_object_id = ft.object_id
inner join sys.schemas fs on ft.schema_id = fs.schema_id
inner join sys.tables pt on fk.referenced_object_id = pt.object_id
inner join sys.schemas ps on pt.schema_id = ps.schema_id
inner join sys.foreign_key_columns fc on fk.object_id = fc.constraint_object_id
where fk.type = 'F'
order by [Name], Foreign_Schema, Foreign_Table, Ordinal
```

## Navigation Properties

Navigation properties define the relationship between POCOs and are reflections of foreign keys between database tables. There are 3 types of relationships: One-to-Many, One-to-One, Many-to-Many. Further reading:
- [Relationships between Entities in Entity Framework 6](https://www.entityframeworktutorial.net/entityframework6/entity-relationships.aspx "Relationships between Entities in Entity Framework 6")
- [Configure One-to-Many Relationships in EF 6](https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx "Configure One-to-Many Relationships in EF 6")
- [Configure One-to-Zero-or-One Relationship in Entity Framework 6](https://www.entityframeworktutorial.net/code-first/configure-one-to-one-relationship-in-code-first.aspx "Configure One-to-Zero-or-One Relationship in Entity Framework 6")
- [Configure Many-to-Many Relationships in Code-First](https://www.entityframeworktutorial.net/code-first/configure-many-to-many-relationship-in-code-first.aspx "Configure Many-to-Many Relationships in Code-First")

### One-to-Many Relationship

A single foreign key, without any special constraints, is a database implementation of a One-to-Many relationship between two tables.

In this example, the foreign key is from `Product.ProductModelID` to `ProductModel.ProductModelID`. `ProductModelID` is the primary key of `ProductModel` table. This foreign key defines a One-to-Many relationship between `Product` and `ProductModel`. The `Product` POCO class has a singular navigation property to `ProductModel` and the `ProductModel` POCO class has a collection navigation property to `Product`.

```cs
public class Product
{
    public int ProductID { get; set; } // primary key
    public int? ProductModelID { get; set; } // foreign key

    public virtual ProductModel ProductModel { get; set; }
}

public class ProductModel
{
    public ProductModel()
    {
        this.Products = new HashSet<Product>();
    }

    public int ProductModelID { get; set; } // primary key

    public virtual ICollection<Product> Products { get; set; }
}
```

### One-to-One Relationship

A database implementation of One-to-One relationship is when the primary key of one table is also a foreign key to the primary key of another table. POCO Generator doesn't recognize unique key/unique index database implementation of One-to-One relationship. The SQL Server implementation of One-to-One relationship is technically One-to-Zero-or-One relationship.

In this example, the foreign key is from `Employee.BusinessEntityID` to `Person.BusinessEntityID`. `Person.BusinessEntityID` is the primary key of `Person` and `Employee.BusinessEntityID` is **both** the primary key of `Employee` and a foreign key to the primary key of `Person`.

```cs
public class Employee
{
    public int BusinessEntityID { get; set; } // primary key, foreign key

    public virtual Person Person { get; set; }
}

public class Person
{
    public int BusinessEntityID { get; set; } // primary key

    public virtual Employee Employee { get; set; }
}
```

### Many-to-Many Relationship

Many-to-Many relationship is when two or more entities have multiple references to all the other entities in the relationship. A database implementation of Many-to-Many relationship is a join table, or intuitively a table "in the middle", that is a construct of all the primary keys of all the tables that take part in the relationship. Every primary key in the join table is also a foreign key to the appropriate primary key in the other corresponding table. The tables in the Many-to-Many relationship don't reference each other directly but rather go through the join table, hence the table "in the middle".

If the join table has more columns than the foreign keys to the other primary keys, for example a create time column, then POCO Generator will treat this relationship as One-To-Many relationship between the join table and each of the other tables in the relationship. This will also take effect when the **Many-to-Many Join Table** setting is enabled.

In this example, a product can be in several warehouses and every warehouse stores several products. The join table is `WarehouseProducts`. All the columns of `WarehouseProducts` are primary keys and each column is a foreign key to `Product` primary key or `Warehouse` primary key appropriately.

```cs
public class Product
{
    public Product()
    {
        this.Warehouses = new HashSet<Warehouse>();
    }

    public int ProductID { get; set; } // primary key

    public virtual ICollection<Warehouse> Warehouses { get; set; }
}

// this poco is not generated. only for illustration
public class WarehouseProducts
{
    public int ProductID { get; set; } // primary key, foreign key
    public int WarehouseID { get; set; } // primary key, foreign key
}

public class Warehouse
{
    public Warehouse()
    {
        this.Products = new HashSet<Product>();
    }

    public int WarehouseID { get; set; } // primary key

    public virtual ICollection<Product> Products { get; set; }
}
```