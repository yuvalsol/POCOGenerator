using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using POCOGenerator.Db;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;
using POCOGenerator.MySQL.DbObjects;

namespace POCOGenerator.MySQL
{
    internal class MySQLHelper : DbHelper
    {
        #region Constructor

        public MySQLHelper(string connectionString)
            : base(connectionString, new MySQLSupport())
        {
        }

        #endregion

        #region Connection, Command & Parameter

        protected override DbConnection GetConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        protected override IDbCommand GetCommand()
        {
            return new MySqlCommand();
        }

        protected override IDataParameter GetParameter(IProcedureParameter parameter, IDatabase database)
        {
            MySqlParameter mySqlParameter = new MySqlParameter();
            mySqlParameter.ParameterName = parameter.ParameterName;
            mySqlParameter.Value = DBNull.Value;

            string dataType = (parameter.ParameterDataType ?? string.Empty).ToLower();
            bool isUnsigned = parameter.ParameterIsUnsigned;

            // https://dev.mysql.com/doc/refman/8.0/en/data-types.html
            switch (dataType)
            {
                // https://www.xaprb.com/blog/2006/04/11/bit-values-in-mysql
                case "bit": mySqlParameter.MySqlDbType = (Support["Version_At_Least_5.0.3"] ? MySqlDbType.Bit : /*tinyint(1)*/ MySqlDbType.Byte); break;

                case "tinyint": mySqlParameter.MySqlDbType = (parameter.ParameterIsUnsigned ? MySqlDbType.UByte : MySqlDbType.Byte); break;

                case "bool":
                case "boolean": mySqlParameter.MySqlDbType = MySqlDbType.Byte; break;

                case "smallint": mySqlParameter.MySqlDbType = (parameter.ParameterIsUnsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16); break;

                case "mediumint": mySqlParameter.MySqlDbType = (parameter.ParameterIsUnsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24); break;

                case "int":
                case "integer": mySqlParameter.MySqlDbType = (parameter.ParameterIsUnsigned ? MySqlDbType.UInt32 : MySqlDbType.Int32); break;

                case "bigint": mySqlParameter.MySqlDbType = (parameter.ParameterIsUnsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64); break;
                case "serial": mySqlParameter.MySqlDbType = MySqlDbType.UInt64; break;

                case "decimal":
                case "dec":
                case "numeric":
                case "fixed": mySqlParameter.MySqlDbType = (Support["Version_At_Least_5.0.3"] ? MySqlDbType.NewDecimal : MySqlDbType.Decimal); break;

                case "float": mySqlParameter.MySqlDbType = MySqlDbType.Float; break;

                case "double": mySqlParameter.MySqlDbType = MySqlDbType.Double; break;

                case "real": mySqlParameter.MySqlDbType = (Support["REAL_AS_FLOAT"] ? MySqlDbType.Float : MySqlDbType.Double); break;

                case "timestamp": mySqlParameter.MySqlDbType = MySqlDbType.Timestamp; break;
                case "datetime": mySqlParameter.MySqlDbType = MySqlDbType.DateTime; break;
                case "date": mySqlParameter.MySqlDbType = MySqlDbType.Date; break;
                case "newdate": mySqlParameter.MySqlDbType = MySqlDbType.Newdate; break;
                case "time": mySqlParameter.MySqlDbType = MySqlDbType.Time; break;
                case "year": mySqlParameter.MySqlDbType = MySqlDbType.Year; break;

                case "char":
                case "nchar":
                case "national char":
                case "character":
                case "string": mySqlParameter.MySqlDbType = MySqlDbType.String; break;

                case "binary":
                case "char byte": mySqlParameter.MySqlDbType = MySqlDbType.Binary; break;

                case "varchar":
                case "nvarchar":
                case "national varchar":
                case "character varying": mySqlParameter.MySqlDbType = MySqlDbType.VarChar; break;

                case "varbinary": mySqlParameter.MySqlDbType = MySqlDbType.VarBinary; break;
                case "varstring": mySqlParameter.MySqlDbType = MySqlDbType.VarString; break;

                case "tinytext": mySqlParameter.MySqlDbType = MySqlDbType.TinyText; break;
                case "mediumtext": mySqlParameter.MySqlDbType = MySqlDbType.MediumText; break;
                case "text": mySqlParameter.MySqlDbType = MySqlDbType.Text; break;
                case "longtext": mySqlParameter.MySqlDbType = MySqlDbType.LongText; break;

                case "tinyblob": mySqlParameter.MySqlDbType = MySqlDbType.TinyBlob; break;
                case "mediumblob": mySqlParameter.MySqlDbType = MySqlDbType.MediumBlob; break;
                case "blob": mySqlParameter.MySqlDbType = MySqlDbType.Blob; break;
                case "longblob": mySqlParameter.MySqlDbType = MySqlDbType.LongBlob; break;

                case "set": mySqlParameter.MySqlDbType = MySqlDbType.Set; break;
                case "enum": mySqlParameter.MySqlDbType = MySqlDbType.Enum; break;

                case "geometry":
                case "geometrycollection":
                case "geomcollection":
                case "point":
                case "multipoint":
                case "linestring":
                case "multilinestring":
                case "polygon":
                case "multipolygon": mySqlParameter.MySqlDbType = MySqlDbType.Geometry; break;

                case "json": mySqlParameter.MySqlDbType = MySqlDbType.JSON; break;

                case "guid": mySqlParameter.MySqlDbType = MySqlDbType.Guid; break;
            }

            switch (mySqlParameter.MySqlDbType)
            {
                case MySqlDbType.Int16:
                case MySqlDbType.UInt16:
                case MySqlDbType.Int24:
                case MySqlDbType.UInt24:
                case MySqlDbType.Int32:
                case MySqlDbType.UInt32:
                case MySqlDbType.Int64:
                case MySqlDbType.UInt64:
                case MySqlDbType.Float:
                case MySqlDbType.Decimal:
                case MySqlDbType.NewDecimal:
                case MySqlDbType.Double:
                    if (parameter.ParameterSize == -1 || parameter.ParameterSize > 0)
                        mySqlParameter.Size = parameter.ParameterSize.Value;
                    if (parameter.ParameterPrecision != null)
                        mySqlParameter.Precision = parameter.ParameterPrecision.Value;
                    if (parameter.ParameterScale != null)
                        mySqlParameter.Scale = (byte)parameter.ParameterScale.Value;
                    break;

                case MySqlDbType.String:
                case MySqlDbType.VarString:
                case MySqlDbType.VarChar:
                    if (parameter.ParameterSize == -1 || parameter.ParameterSize > 0)
                        mySqlParameter.Size = parameter.ParameterSize.Value;
                    break;
            }

            mySqlParameter.Direction = parameter.ParameterDirection;

            return mySqlParameter;
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
                        command.CommandText = GetScript(this.GetType(), "MySQL_Version.sql");
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

        protected override void GetServerConfiguration(IServer server)
        {
            Support["Version_At_Least_5.0.3"] = (Support.Version != null && Support.Version.GreaterThanOrEqual(5, 0, 3));
            Support["Version_At_Least_8.0.13"] = (Support.Version != null && Support.Version.GreaterThanOrEqual(8, 0, 13));

            SetRealAsFloat();
        }

        protected void SetRealAsFloat()
        {
            try
            {
                string connectionString = ConnectionString;
                using (DbConnection connection = GetConnection(connectionString))
                {
                    using (IDbCommand command = GetCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = GetScript(this.GetType(), "MySQL_RealAsFloat.sql");
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        string sql_mode = command.ExecuteScalar() as string;
                        Support["REAL_AS_FLOAT"] = (string.IsNullOrEmpty(sql_mode) == false && sql_mode.Contains("REAL_AS_FLOAT"));
                    }
                }
            }
            catch
            {
                Support["REAL_AS_FLOAT"] = false;
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
                server.Databases = server.Databases.Where(t => t.ToString() != "sys" && t.ToString() != "mysql" && t.ToString() != "information_schema" && t.ToString() != "performance_schema").ToList();
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
                    command.CommandText = GetScript(this.GetType(), "MySQL_Descriptions.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    MySqlParameter mySqlParameter = new MySqlParameter("@database_name", MySqlDbType.VarChar, 64);
                    mySqlParameter.Value = database.ToString();
                    command.Parameters.Add(mySqlParameter);

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
                    command.CommandText = GetScript(this.GetType(), "MySQL_Keys.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    MySqlParameter mySqlParameter = new MySqlParameter("@database_name", MySqlDbType.VarChar, 64);
                    mySqlParameter.Value = database.ToString();
                    command.Parameters.Add(mySqlParameter);

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
                    command.CommandText = GetScript(this.GetType(), "MySQL_Indexes.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    MySqlParameter mySqlParameter = new MySqlParameter("@database_name", MySqlDbType.VarChar, 64);
                    mySqlParameter.Value = database.ToString();
                    command.Parameters.Add(mySqlParameter);

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
                    command.CommandText = GetScript(this.GetType(), "MySQL_IdentityColumns.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    MySqlParameter mySqlParameter = new MySqlParameter("@database_name", MySqlDbType.VarChar, 64);
                    mySqlParameter.Value = database.ToString();
                    command.Parameters.Add(mySqlParameter);

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
                    command.CommandText = GetScript(this.GetType(), "MySQL_ComputedColumns.sql");
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    MySqlParameter mySqlParameter = new MySqlParameter("@database_name", MySqlDbType.VarChar, 64);
                    mySqlParameter.Value = database.ToString();
                    command.Parameters.Add(mySqlParameter);

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

        #region Tables

        protected override IEnumerable<ITable> GetSchemaTables(DbConnection connection, string database)
        {
            return connection.GetSchema("Tables", new string[] { null, database, null, "BASE TABLE" }).Cast<Table>();
        }

        protected override IEnumerable<ITableColumn> GetSchemaTableColumns(DbConnection connection, ITable table)
        {
            return connection.GetSchema("Columns", new string[] { null, table.Database.ToString(), table.Name, null }).Cast<TableColumn>();
        }

        #endregion

        #region Views

        protected override IEnumerable<IView> GetSchemaViews(DbConnection connection, string database)
        {
            return connection.GetSchema("Views", new string[] { null, database, null }).Cast<View>();
        }

        protected override IEnumerable<ITableColumn> GetSchemaViewColumns(DbConnection connection, IView view)
        {
            return connection.GetSchema("ViewColumns", new string[] { null, view.Database.ToString(), view.Name, null }).Cast<ViewColumn>();
        }

        #endregion

        #region Procedures

        protected override IEnumerable<IProcedure> GetSchemaProcedures(DbConnection connection, string database)
        {
            return connection.GetSchema("Procedures", new string[] { null, database, null, "PROCEDURE" }).Cast<Procedure>();
        }

        protected override IEnumerable<IProcedureParameter> GetSchemaProcedureParameters(DbConnection connection, IProcedure procedure)
        {
            return connection.GetSchema("Procedure Parameters", new string[] { null, procedure.Database.ToString(), procedure.Name, "PROCEDURE" }).Cast<ProcedureParameter>();
        }

        protected override string GetProcedureCommandText(IProcedure procedure)
        {
            return string.Format("{0}.{1}", procedure.Database.ToString(), procedure.Name);
        }

        protected override IEnumerable<IProcedureColumn> GetSchemaProcedureColumns(DataTable schemaTable)
        {
            return schemaTable.Cast<ProcedureColumn>((string columnName, Type columnType, object value) =>
            {
                if (columnName == "IsIdentity")
                    return value as bool? ?? false;
                return value;
            }).Where(c => string.IsNullOrEmpty(c.ColumnName) == false);
        }

        #endregion
    }
}
