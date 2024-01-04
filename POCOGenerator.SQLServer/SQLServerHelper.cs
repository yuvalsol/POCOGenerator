using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using POCOGenerator.Db;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;
using POCOGenerator.SQLServer.DbObjects;

namespace POCOGenerator.SQLServer
{
    internal class SQLServerHelper : DbHelper
    {
        #region Constructor

        public SQLServerHelper(string connectionString)
            : base(connectionString, new SQLServerSupport())
        {
        }

        #endregion

        #region Connection, Command & Parameter

        protected override DbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        protected override IDbCommand GetCommand()
        {
            return new SqlCommand();
        }

        protected override IDataParameter GetParameter(IProcedureParameter parameter, IDatabase database)
        {
            SqlParameter sqlParameter = new SqlParameter()
            {
                ParameterName = parameter.ParameterName,
                Value = DBNull.Value
            };

            string dataType = (parameter.ParameterDataType ?? string.Empty).ToLower();

            // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
            switch (dataType)
            {
                case "bigint": sqlParameter.SqlDbType = SqlDbType.BigInt; break;
                case "binary": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
                case "bit": sqlParameter.SqlDbType = SqlDbType.Bit; break;
                case "char": sqlParameter.SqlDbType = SqlDbType.Char; break;
                case "date": sqlParameter.SqlDbType = SqlDbType.Date; break;
                case "datetime": sqlParameter.SqlDbType = SqlDbType.DateTime; break;
                case "datetime2": sqlParameter.SqlDbType = SqlDbType.DateTime2; break;
                case "datetimeoffset": sqlParameter.SqlDbType = SqlDbType.DateTimeOffset; break;
                case "decimal": sqlParameter.SqlDbType = SqlDbType.Decimal; break;
                case "filestream": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
                case "float": sqlParameter.SqlDbType = SqlDbType.Float; break;
                case "geography":
                    sqlParameter.SqlDbType = SqlDbType.Udt;
                    sqlParameter.UdtTypeName = "Geography";
                    break;
                case "geometry":
                    sqlParameter.SqlDbType = SqlDbType.Udt;
                    sqlParameter.UdtTypeName = "Geometry";
                    break;
                case "hierarchyid":
                    sqlParameter.SqlDbType = SqlDbType.Udt;
                    sqlParameter.UdtTypeName = "HierarchyId";
                    break;
                case "image": sqlParameter.SqlDbType = SqlDbType.Image; break;
                case "int": sqlParameter.SqlDbType = SqlDbType.Int; break;
                case "money": sqlParameter.SqlDbType = SqlDbType.Money; break;
                case "nchar": sqlParameter.SqlDbType = SqlDbType.NChar; break;
                case "ntext": sqlParameter.SqlDbType = SqlDbType.NText; break;
                case "numeric": sqlParameter.SqlDbType = SqlDbType.Decimal; break;
                case "nvarchar": sqlParameter.SqlDbType = SqlDbType.NVarChar; break;
                case "real": sqlParameter.SqlDbType = SqlDbType.Real; break;
                case "rowversion": sqlParameter.SqlDbType = SqlDbType.Timestamp; break;
                case "smalldatetime": sqlParameter.SqlDbType = SqlDbType.SmallDateTime; break;
                case "smallint": sqlParameter.SqlDbType = SqlDbType.SmallInt; break;
                case "smallmoney": sqlParameter.SqlDbType = SqlDbType.SmallMoney; break;
                case "sql_variant": sqlParameter.SqlDbType = SqlDbType.Variant; break;
                case "text": sqlParameter.SqlDbType = SqlDbType.Text; break;
                case "time": sqlParameter.SqlDbType = SqlDbType.Time; break;
                case "timestamp": sqlParameter.SqlDbType = SqlDbType.Timestamp; break;
                case "tinyint": sqlParameter.SqlDbType = SqlDbType.TinyInt; break;
                case "uniqueidentifier": sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier; break;
                case "varbinary": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
                case "varchar": sqlParameter.SqlDbType = SqlDbType.VarChar; break;
                case "xml": sqlParameter.SqlDbType = SqlDbType.Xml; break;
                default:
                    if (database.TVPs != null)
                    {
                        // could be more than one tvp with the same name but with different schema
                        // there's no way to differentiate between them
                        // beacuse the data type from the procedure parameter doesn't come with the schema name
                        ITVP tvp = database.TVPs.Where(t => string.Compare(t.Name, parameter.ParameterDataType, true) == 0).FirstOrDefault();
                        if (tvp != null)
                        {
                            sqlParameter.TypeName = parameter.ParameterDataType;
                            sqlParameter.SqlDbType = SqlDbType.Structured;
                            sqlParameter.Value = GetTVPDataTable(tvp);
                        }
                    }
                    break;
            }

            if (dataType == "binary" || dataType == "varbinary" || dataType == "char" || dataType == "nchar" || dataType == "nvarchar" || dataType == "varchar")
            {
                if (parameter.ParameterSize == -1 || parameter.ParameterSize > 0)
                    sqlParameter.Size = parameter.ParameterSize.Value;
            }

            sqlParameter.Direction = parameter.ParameterDirection;

            return sqlParameter;
        }

        #endregion

        #region Server

        protected override string GetServerVersion()
        {
            try
            {
                string connectionString = ConnectionString;
                using (DbConnection connection = GetConnection(connectionString))
                {
                    using (IDbCommand command = GetCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = GetScript(this.GetType(), "SQLServer_Version.sql");
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        return command.ExecuteScalar() as string;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Databases

        protected override IEnumerable<IDatabase> GetSchemaDatabases(DbConnection connection)
        {
            return connection.GetSchema("Databases").Cast<Database>();
        }

        protected override void RemoveSystemDatabases(IServer server)
        {
            if (server.Databases.HasAny())
                server.Databases = server.Databases.Where(t => t.ToString() != "master" && t.ToString() != "model" && t.ToString() != "msdb" && t.ToString() != "tempdb").ToList();
        }

        #endregion

        #region System Objects

        protected override List<ISystemObject> GetSystemObjects(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_SystemObjects.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable systemObjectsDT = new DataTable();
                        systemObjectsDT.Load(reader);

                        List<SystemObject> systemObjects = (Support.IsSupportSchema ? systemObjectsDT.Cast<SystemObjectSchema>() : systemObjectsDT.Cast<SystemObject>()).ToList();

                        foreach (SystemObject systemObject in systemObjects)
                        {
                            if (string.IsNullOrEmpty(systemObject.Type) == false)
                                systemObject.Type = systemObject.Type.Trim();
                        }

                        return systemObjects.Cast<ISystemObject>().ToList();
                    }
                }
            }
        }

        #endregion

        #region Descriptions

        protected override List<IDbObjectDescription> GetDbObjectDescriptions(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_Descriptions.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable descriptionsDT = new DataTable();
                        descriptionsDT.Load(reader);
                        return (Support.IsSupportSchema ? descriptionsDT.Cast<DbObjectDescriptionSchema>() : descriptionsDT.Cast<DbObjectDescription>()).Cast<IDbObjectDescription>().ToList();
                    }
                }
            }
        }

        #endregion

        #region Primary, Unique & Foreign Keys

        protected override Tuple<List<IInternalKey>, List<IInternalKey>, List<IInternalForeignKey>> GetKeys(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_Keys.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataSet keysDS = new DataSet();
                        keysDS.Tables.Add("PrimaryKeys");
                        keysDS.Tables.Add("UniqueKeys");
                        keysDS.Tables.Add("ForeignKeys");
                        keysDS.Load(reader, LoadOption.PreserveChanges, keysDS.Tables["PrimaryKeys"], keysDS.Tables["UniqueKeys"], keysDS.Tables["ForeignKeys"]);
                        return new Tuple<List<IInternalKey>, List<IInternalKey>, List<IInternalForeignKey>>(
                            (Support.IsSupportSchema ? keysDS.Tables["PrimaryKeys"].Cast<PrimaryKeySchemaInternal>().Cast<IInternalKey>() : keysDS.Tables["PrimaryKeys"].Cast<PrimaryKeyInternal>().Cast<IInternalKey>()).ToList(),
                            (Support.IsSupportSchema ? keysDS.Tables["UniqueKeys"].Cast<UniqueKeySchemaInternal>().Cast<IInternalKey>() : keysDS.Tables["UniqueKeys"].Cast<UniqueKeyInternal>().Cast<IInternalKey>()).ToList(),
                            (Support.IsSupportSchema ? keysDS.Tables["ForeignKeys"].Cast<ForeignKeySchemaInternal>().Cast<IInternalForeignKey>() : keysDS.Tables["ForeignKeys"].Cast<ForeignKeyInternal>().Cast<IInternalForeignKey>()).ToList()
                        );
                    }
                }
            }
        }

        #endregion

        #region Indexes

        protected override List<IInternalIndex> GetIndexes(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_Indexes.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable indexesDT = new DataTable();
                        indexesDT.Load(reader);
                        return (Support.IsSupportSchema ? indexesDT.Cast<IndexSchemaInternal>().Cast<IInternalIndex>() : indexesDT.Cast<IndexInternal>().Cast<IInternalIndex>()).ToList();
                    }
                }
            }
        }

        #endregion

        #region Identity Columns

        protected override List<IIdentityColumn> GetIdentityColumns(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_IdentityColumns.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable identityColumnsDT = new DataTable();
                        identityColumnsDT.Load(reader);
                        return (Support.IsSupportSchema ? identityColumnsDT.Cast<IdentityColumnSchema>() : identityColumnsDT.Cast<IdentityColumn>()).Cast<IIdentityColumn>().ToList();
                    }
                }
            }
        }

        #endregion

        #region Computed Columns

        protected override List<IComputedColumn> GetComputedColumns(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_ComputedColumns.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable computedColumnsDT = new DataTable();
                        computedColumnsDT.Load(reader);
                        return (Support.IsSupportSchema ? computedColumnsDT.Cast<ComputedColumnSchema>() : computedColumnsDT.Cast<ComputedColumn>()).Cast<IComputedColumn>().ToList();
                    }
                }
            }
        }

        #endregion

        #region TVPs

        protected override List<ITVP> GetTVPs(IDatabase database)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_TVPs.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable tvpsDT = new DataTable();
                        tvpsDT.Load(reader);
                        return tvpsDT.Cast<TVP>().Cast<ITVP>().ToList();
                    }
                }
            }
        }

        protected override void RemoveSystemObjectsFromTVPs(IDatabase database, List<ISystemObject> systemObjects)
        {
            if (database.TVPs.HasAny() && systemObjects.HasAny())
                database.TVPs = database.TVPs.Where(tvp => systemObjects.Any(so => (so is ISchema schema1 == false || tvp is ISchema schema2 == false || schema1.Schema == schema2.Schema) && so.Name == tvp.Name && so.Type == "TT") == false).ToList();
        }

        protected override List<ITVPColumn> GetTVPColumns(ITVP tvp)
        {
            string connectionString = tvp.Database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                using (IDbCommand command = GetCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetScript(this.GetType(), "SQLServer_TVPColumns.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    var param = new SqlParameter("@tvp_id", SqlDbType.Int)
                    {
                        Value = tvp.TVPId
                    };
                    command.Parameters.Add(param);

                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable tvpColumnsDT = new DataTable();
                        tvpColumnsDT.Load(reader);
                        return tvpColumnsDT.Cast<TVPColumn>().Cast<ITVPColumn>().ToList();
                    }
                }
            }
        }

        protected override DataTable GetTVPDataTable(ITVP tvp)
        {
            if (tvp.TVPDataTable != null)
                return tvp.TVPDataTable;

            DataTable tvpDataTable = new DataTable();

            if (tvp.TVPColumns != null)
            {
                foreach (ITVPColumn column in tvp.TVPColumns)
                {
                    switch ((column.DataTypeName ?? string.Empty).ToLower())
                    {
                        case "bigint": tvpDataTable.Columns.Add(column.ColumnName, typeof(long)); break;
                        case "binary": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte[])); break;
                        case "bit": tvpDataTable.Columns.Add(column.ColumnName, typeof(bool)); break;
                        case "char": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        case "date": tvpDataTable.Columns.Add(column.ColumnName, typeof(DateTime)); break;
                        case "datetime": tvpDataTable.Columns.Add(column.ColumnName, typeof(DateTime)); break;
                        case "datetime2": tvpDataTable.Columns.Add(column.ColumnName, typeof(DateTime)); break;
                        case "datetimeoffset": tvpDataTable.Columns.Add(column.ColumnName, typeof(DateTimeOffset)); break;
                        case "decimal": tvpDataTable.Columns.Add(column.ColumnName, typeof(decimal)); break;
                        case "filestream": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte[])); break;
                        case "float": tvpDataTable.Columns.Add(column.ColumnName, typeof(double)); break;
                        case "geography": tvpDataTable.Columns.Add(column.ColumnName, typeof(Microsoft.SqlServer.Types.SqlGeography)); break;
                        case "geometry": tvpDataTable.Columns.Add(column.ColumnName, typeof(Microsoft.SqlServer.Types.SqlGeometry)); break;
                        case "hierarchyid": tvpDataTable.Columns.Add(column.ColumnName, typeof(Microsoft.SqlServer.Types.SqlHierarchyId)); break;
                        case "image": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte[])); break;
                        case "int": tvpDataTable.Columns.Add(column.ColumnName, typeof(int)); break;
                        case "money": tvpDataTable.Columns.Add(column.ColumnName, typeof(decimal)); break;
                        case "nchar": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        case "ntext": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        case "numeric": tvpDataTable.Columns.Add(column.ColumnName, typeof(decimal)); break;
                        case "nvarchar": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        case "real": tvpDataTable.Columns.Add(column.ColumnName, typeof(float)); break;
                        case "rowversion": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte[])); break;
                        case "smalldatetime": tvpDataTable.Columns.Add(column.ColumnName, typeof(DateTime)); break;
                        case "smallint": tvpDataTable.Columns.Add(column.ColumnName, typeof(short)); break;
                        case "smallmoney": tvpDataTable.Columns.Add(column.ColumnName, typeof(decimal)); break;
                        case "sql_variant": tvpDataTable.Columns.Add(column.ColumnName, typeof(object)); break;
                        case "text": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        case "time": tvpDataTable.Columns.Add(column.ColumnName, typeof(TimeSpan)); break;
                        case "timestamp": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte[])); break;
                        case "tinyint": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte)); break;
                        case "uniqueidentifier": tvpDataTable.Columns.Add(column.ColumnName, typeof(Guid)); break;
                        case "varbinary": tvpDataTable.Columns.Add(column.ColumnName, typeof(byte[])); break;
                        case "varchar": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        case "xml": tvpDataTable.Columns.Add(column.ColumnName, typeof(string)); break;
                        default: tvpDataTable.Columns.Add(column.ColumnName, typeof(object)); break;
                    }
                }
            }

            tvp.TVPDataTable = tvpDataTable;

            return tvpDataTable;
        }

        #endregion

        #region Tables

        protected override IEnumerable<ITable> GetSchemaTables(DbConnection connection, string database)
        {
            return connection.GetSchema("Tables", new string[] { database, null, null, "BASE TABLE" }).Cast<Table>();
        }

        protected override void RemoveSystemObjectsFromTables(IDatabase database, List<ISystemObject> systemObjects)
        {
            if (database.Tables.HasAny() && systemObjects.HasAny())
                database.Tables = database.Tables.Where(t => systemObjects.Any(so => (so is ISchema schema1 == false || t is ISchema schema2 == false || schema1.Schema == schema2.Schema) && so.Name == t.Name && (so.Type == "IT" || so.Type == "S" || so.Type == "U")) == false).ToList();
        }

        protected override IEnumerable<ITableColumn> GetSchemaTableColumns(DbConnection connection, ITable table)
        {
            return connection.GetSchema("Columns", new string[] { table.Database.ToString(), ((ISchema)table).Schema, table.Name, null }).Cast<TableColumn>();
        }

        #endregion

        #region Views

        protected override IEnumerable<IView> GetSchemaViews(DbConnection connection, string database)
        {
            return connection.GetSchema("Tables", new string[] { database, null, null, "VIEW" }).Cast<View>();
        }

        protected override void RemoveSystemObjectsFromViews(IDatabase database, List<ISystemObject> systemObjects)
        {
            if (database.Views.HasAny() && systemObjects.HasAny())
                database.Views = database.Views.Where(v => systemObjects.Any(so => (so is ISchema schema1 == false || v is ISchema schema2 == false || schema1.Schema == schema2.Schema) && so.Name == v.Name && so.Type == "V") == false).ToList();
        }

        protected override IEnumerable<ITableColumn> GetSchemaViewColumns(DbConnection connection, IView view)
        {
            return connection.GetSchema("Columns", new string[] { view.Database.ToString(), ((ISchema)view).Schema, view.Name, null }).Cast<ViewColumn>();
        }

        #endregion

        #region Procedures

        protected override IEnumerable<IProcedure> GetSchemaProcedures(DbConnection connection, string database)
        {
            return connection.GetSchema("Procedures", new string[] { database, null, null, "PROCEDURE" }).Cast<Procedure>();
        }

        protected override void RemoveSystemObjectsFromProcedures(IDatabase database, List<ISystemObject> systemObjects)
        {
            if (database.Procedures.HasAny() && systemObjects.HasAny())
                database.Procedures = database.Procedures.Where(p => systemObjects.Any(so => (so is ISchema schema1 == false || p is ISchema schema2 == false || schema1.Schema == schema2.Schema) && so.Name == p.Name && (so.Type == "P" || so.Type == "PC" || so.Type == "RF" || so.Type == "X")) == false).ToList();
        }

        protected override IEnumerable<IProcedureParameter> GetSchemaProcedureParameters(DbConnection connection, IProcedure procedure)
        {
            return connection.GetSchema("ProcedureParameters", new string[] { procedure.Database.ToString(), ((ISchema)procedure).Schema, procedure.Name, null }).Cast<ProcedureParameter>();
        }

        protected override string GetProcedureCommandText(IProcedure procedure)
        {
            return string.Format("[{0}].[{1}]", ((ISchema)procedure).Schema, procedure.Name);
        }

        protected override IEnumerable<IProcedureColumn> GetSchemaProcedureColumns(DataTable schemaTable)
        {
            return schemaTable.Cast<ProcedureColumn>((string columnName, Type columnType, object value) =>
            {
                if (columnName == "NumericPrecision" || columnName == "NumericScale")
                    return (int?)(value as short?);
                else if (columnName == "IsIdentity")
                    return value as bool? ?? false;
                return value;
            }).Where(c => string.IsNullOrEmpty(c.ColumnName) == false);
        }

        protected override void GetProcedureSchemaSecondTry(IProcedure procedure, DbConnection connection, Exception ex)
        {
            if (ex.Message.StartsWith("Invalid object name '#"))
            {
                try
                {
                    procedure.ProcedureColumns = GetProcedureWithTemporaryTablesSchema(procedure, connection);
                }
                catch
                {
                    procedure.Error = new Exception(ex.Message, new Exception("Temporary tables in stored procedure.\nYou may want to add this code to the stored procedure to retrieve the schema:\nIF 1=0\nBEGIN\n    SET FMTONLY OFF\nEND"));
                }
            }
            else
            {
                procedure.Error = ex;
            }
        }

        protected virtual List<IProcedureColumn> GetProcedureWithTemporaryTablesSchema(IProcedure procedure, DbConnection connection)
        {
            using (IDbCommand command = GetCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 60;

                string commandText = @"
                    IF 1=0
                    BEGIN
                        SET FMTONLY OFF;
                    END
                    exec [{0}].[{1}] ";
                commandText = string.Format(commandText, ((ISchema)procedure).Schema, procedure.Name);
                foreach (IProcedureParameter parameter in procedure.ProcedureParameters.OrderBy(c => c.ParameterOrdinal ?? 0))
                {
                    if (parameter.IsResult == false)
                    {
                        commandText += parameter.ParameterName + ",";
                        command.Parameters.Add(GetParameter(parameter, procedure.Database));
                    }
                }
                commandText = commandText.TrimEnd(',');
                command.CommandText = commandText;

                return GetProcedureSchema(command);
            }
        }

        #endregion

        #region Functions

        protected override IEnumerable<IFunction> GetSchemaFunctions(DbConnection connection, string database)
        {
            return connection.GetSchema("Procedures", new string[] { database, null, null, "FUNCTION" }).Cast<Function>();
        }

        protected override void RemoveSystemObjectsFromFunctions(IDatabase database, List<ISystemObject> systemObjects)
        {
            if (database.Functions.HasAny() && systemObjects.HasAny())
                database.Functions = database.Functions.Where(f => systemObjects.Any(so => (so is ISchema schema1 == false || f is ISchema schema2 == false || schema1.Schema == schema2.Schema) && so.Name == f.Name && (so.Type == "AF" || so.Type == "FN" || so.Type == "FS" || so.Type == "FT" || so.Type == "IF" || so.Type == "TF")) == false).ToList();
        }

        protected override IEnumerable<IProcedureParameter> GetSchemaFunctionParameters(DbConnection connection, IFunction function)
        {
            return connection.GetSchema("ProcedureParameters", new string[] { function.Database.ToString(), ((ISchema)function).Schema, function.Name, null }).Cast<ProcedureParameter>();
        }

        protected override string GetFunctionCommandText(IFunction function)
        {
            string commandText = string.Format("select * from [{0}].[{1}](", ((ISchema)function).Schema, function.Name);
            foreach (IProcedureParameter parameter in function.ProcedureParameters.OrderBy(c => c.ParameterOrdinal ?? 0))
            {
                if (parameter.IsResult == false)
                    commandText += parameter.ParameterName + ",";
            }
            commandText = commandText.TrimEnd(',');
            commandText += ")";
            return commandText;
        }

        protected override IEnumerable<IProcedureColumn> GetSchemaFunctionColumns(DataTable schemaTable)
        {
            return schemaTable.Cast<ProcedureColumn>((string columnName, Type columnType, object value) =>
            {
                if (columnName == "NumericPrecision" || columnName == "NumericScale")
                    return (int?)(value as short?);
                else if (columnName == "IsIdentity")
                    return value as bool? ?? false;
                return value;
            }).Where(c => string.IsNullOrEmpty(c.ColumnName) == false);
        }

        #endregion
    }
}
