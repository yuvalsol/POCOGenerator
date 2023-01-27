using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class TableColumn : ITableColumn
    {
        #region Database Properties

        public string column_name { get; set; }
        public int? ordinal_position { get; set; }
        public string column_default { get; set; }
        public string is_nullable { get; set; }
        public string data_type { get; set; }
        public int? character_maximum_length { get; set; }
        public byte? numeric_precision { get; set; }
        public int? numeric_scale { get; set; }
        public short? datetime_precision { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }

        /* not in use. reduce memory.
        public string table_catalog { get; set; }
        public string table_schema { get; set; }
        public string table_name { get; set; }
        public int? character_octet_length { get; set; }
        public short? numeric_precision_radix { get; set; }
        public string character_set_catalog { get; set; }
        public string character_set_schema { get; set; }
        public string character_set_name { get; set; }
        public string collation_catalog { get; set; }
        public bool? is_sparse { get; set; }
        public bool? is_column_set { get; set; }
        public bool? is_filestream { get; set; }
        */

        #endregion

        #region IColumn

        public string ColumnName { get { return column_name; } }
        public int? ColumnOrdinal { get { return ordinal_position; } }
        public string DataTypeName { get { return data_type; } }

        public string DataTypeDisplay
        {
            get
            {
                if (data_type == "xml")
                    return "XML";
                return data_type;
            }
        }

        public int? StringPrecision { get { return character_maximum_length; } }
        public int? NumericPrecision { get { return (int?)numeric_precision; } }
        public int? NumericScale { get { return numeric_scale; } }
        public int? DateTimePrecision { get { return datetime_precision; } }
        public bool IsUnsigned { get { return false; } }
        public bool IsNullable { get { return string.Compare(is_nullable, "YES", true) == 0; } }
        public bool IsIdentity { get { return is_identity; } set { is_identity = value; } }
        public bool IsComputed { get { return is_computed; } set { is_computed = value; } }

        public string Precision
        {
            get
            {
                string precision = null;

                string dataType = data_type.ToLower();

                if (dataType == "binary" || dataType == "varbinary" || dataType == "char" || dataType == "nchar" || dataType == "nvarchar" || dataType == "varchar")
                {
                    if (StringPrecision == -1)
                        precision = "(max)";
                    else if (StringPrecision > 0)
                        precision = "(" + StringPrecision + ")";
                }
                else if (dataType == "decimal" || dataType == "numeric")
                {
                    precision = "(" + NumericPrecision + "," + NumericScale + ")";
                }
                else if (dataType == "datetime2" || dataType == "datetimeoffset" || dataType == "time")
                {
                    precision = "(" + DateTimePrecision + ")";
                }
                else if (dataType == "xml")
                {
                    precision = "(.)";
                }

                return precision;
            }
        }

        #endregion

        #region IDescription

        public string Description { get; set; }

        #endregion

        #region ITableColumn

        public ITable Table { get; set; }

        public IPrimaryKeyColumn PrimaryKeyColumn { get; set; }
        public List<IUniqueKeyColumn> UniqueKeyColumns { get; set; }
        public List<IForeignKeyColumn> ForeignKeyColumns { get; set; }
        public List<IForeignKeyColumn> PrimaryForeignKeyColumns { get; set; }
        public List<IIndexColumn> IndexColumns { get; set; }
        public IComplexTypeTableColumn ComplexTypeTableColumn { get; set; }

        public string ColumnDefault { get { return column_default; } }

        public string ToFullString()
        {
            return
                ColumnName + " (" +
                (PrimaryKeyColumn != null ? "PK, " : string.Empty) +
                (ForeignKeyColumns.HasAny() ? "FK, " : string.Empty) +
                (IsComputed ? "Computed, " : string.Empty) +
                DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return ColumnName + " (" + DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion
    }
}
