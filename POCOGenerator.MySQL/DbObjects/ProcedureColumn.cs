using System;
using MySql.Data.MySqlClient;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class ProcedureColumn : IProcedureColumn
    {
        #region Database Properties

        public string ColumnName { get; set; }
        public int? ColumnOrdinal { get; set; }
        public int? ColumnSize { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public Type DataType { get; set; }
        public bool? AllowDBNull { get; set; }
        public int? ProviderType { get; set; }
        public bool IsIdentity { get; set; } // originally bool?
        public bool? IsLong { get; set; }

        /* not in use. reduce memory.
        public bool? IsUnique { get; set; }
        public bool? IsKey { get; set; }
        public string BaseCatalogName { get; set; }
        public string BaseColumnName { get; set; }
        public string BaseSchemaName { get; set; }
        public string BaseTableName { get; set; }
        public bool? IsAliased { get; set; }
        public bool? IsExpression { get; set; }
        public bool? IsAutoIncrement { get; set; }
        public bool? IsRowVersion { get; set; }
        public bool? IsHidden { get; set; }
        public bool? IsReadOnly { get; set; }
        */

        #endregion

        #region IColumn

        public string DataTypeName
        {
            get
            {
                if (ProviderType == null)
                    return string.Empty;

                MySqlDbType mySqlDbType = (MySqlDbType)Enum.ToObject(typeof(MySqlDbType), ProviderType.Value);

                switch (mySqlDbType)
                {
                    case MySqlDbType.Decimal: return "decimal";
                    case MySqlDbType.Byte: return "tinyint";
                    case MySqlDbType.Int16: return "smallint";
                    case MySqlDbType.Int32: return "int";
                    case MySqlDbType.Float: return "float";
                    case MySqlDbType.Double: return "double";
                    case MySqlDbType.Timestamp: return "timestamp";
                    case MySqlDbType.Int64: return "bigint";
                    case MySqlDbType.Int24: return "mediumint";
                    case MySqlDbType.Date: return "date";
                    case MySqlDbType.Time: return "time";
                    case MySqlDbType.DateTime: return "datetime";
                    case MySqlDbType.Year: return "year";
                    case MySqlDbType.Newdate: return "date";
                    case MySqlDbType.VarString: return "varchar";
                    case MySqlDbType.Bit: return "bit";
                    case MySqlDbType.JSON: return "json";
                    case MySqlDbType.NewDecimal: return "decimal";
                    case MySqlDbType.Enum: return "enum";
                    case MySqlDbType.Set: return "set";
                    case MySqlDbType.TinyBlob: return "tinyblob";
                    case MySqlDbType.MediumBlob: return "mediumblob";
                    case MySqlDbType.LongBlob: return "longblob";
                    case MySqlDbType.Blob:
                        if (ColumnSize != null)
                        {
                            if (ColumnSize.Value == 255)
                                return "tinyblob";
                            else if (ColumnSize.Value == 65535)
                                return "blob";
                            else if (ColumnSize.Value == 16777215)
                                return "mediumblob";
                            else if (ColumnSize.Value == -1)
                                return "longblob";
                        }
                        return "blob";
                    case MySqlDbType.VarChar: return "varchar";
                    case MySqlDbType.String: return "char"; //  MySqlDbType.String returns for enum & set
                    case MySqlDbType.Geometry: return "geometry";
                    case MySqlDbType.UByte: return "tinyint"; // tinyint unsigned
                    case MySqlDbType.UInt16: return "smallint"; // smallint unsigned
                    case MySqlDbType.UInt32: return "int"; // int unsigned
                    case MySqlDbType.UInt64: return "bigint"; // bigint unsigned
                    case MySqlDbType.UInt24: return "mediumint"; // mediumint unsigned
                    case MySqlDbType.Binary: return "binary";
                    case MySqlDbType.VarBinary: return "varbinary";
                    case MySqlDbType.TinyText: return "tinytext";
                    case MySqlDbType.MediumText: return "mediumtext";
                    case MySqlDbType.LongText: return "longtext";
                    case MySqlDbType.Text:
                        if (ColumnSize != null)
                        {
                            if (ColumnSize.Value == 255)
                                return "tinytext";
                            else if (ColumnSize.Value == 65535)
                                return "text";
                            else if (ColumnSize.Value == 16777215)
                                return "mediumtext";
                            else if (ColumnSize.Value == -1)
                                return "longtext";
                        }
                        return "text";
                    case MySqlDbType.Guid: return "varbinary"; // varbinary(16)
                    default: return string.Empty;
                }
            }
        }

        public bool IsUnsigned
        {
            get
            {
                if (ProviderType == null)
                    return false;

                MySqlDbType mySqlDbType = (MySqlDbType)Enum.ToObject(typeof(MySqlDbType), ProviderType.Value);

                switch (mySqlDbType)
                {
                    case MySqlDbType.UByte: // tinyint unsigned
                    case MySqlDbType.UInt16: // smallint unsigned
                    case MySqlDbType.UInt32: // int unsigned
                    case MySqlDbType.UInt64: // bigint unsigned
                    case MySqlDbType.UInt24: // mediumint unsigned
                        return true;
                    default: return false;
                }
            }
        }

        public int? StringPrecision { get { return (IsLong == true ? -1 : ColumnSize); } }
        public int? DateTimePrecision { get { return NumericScale; } }
        public bool IsNullable { get { return (AllowDBNull ?? false); } }
        public bool IsComputed { get { return false; } set { } }

        public string DataTypeDisplay
        {
            get
            {
                return DataTypeName.ToLower();
            }
        }

        public string Precision
        {
            get
            {
                string precision = null;

                string dataType = DataTypeName.ToLower();

                if (dataType == "binary" || dataType == "char byte" ||
                    dataType == "char" || dataType == "character" ||
                    dataType == "nchar" || dataType == "national char" ||
                    dataType == "nvarchar" || dataType == "national varchar" ||
                    dataType == "varbinary" ||
                    dataType == "varchar" || dataType == "character varying")
                {
                    if (StringPrecision != null)
                        precision = "(" + StringPrecision + ")";
                }
                else if (dataType == "bit")
                {
                    if (NumericPrecision != null)
                        precision = "(" + ColumnSize + ")";
                }
                else if (dataType == "decimal" || dataType == "numeric" || dataType == "dec" || dataType == "fixed")
                {
                    if (NumericPrecision != null && NumericScale != null)
                        precision = "(" + NumericPrecision + "," + NumericScale + ")";
                }
                else if (dataType == "datetime" || dataType == "time" || dataType == "timestamp")
                {
                    if (DateTimePrecision != null && DateTimePrecision > 0)
                        precision = "(" + DateTimePrecision + ")";
                }

                return precision;
            }
        }

        #endregion

        #region IProcedureColumn

        public IProcedure Procedure { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return ColumnName + " (" + DataTypeDisplay + Precision + (IsUnsigned ? " unsigned" : string.Empty) + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion
    }
}
