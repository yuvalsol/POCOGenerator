using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class TVPColumn : ITVPColumn
    {
        #region Database Properties

        public string data_type { get; set; }
        public string name { get; set; }
        public int column_id { get; set; }
        public short max_length { get; set; }
        public byte precision { get; set; }
        public byte scale { get; set; }
        public bool? is_nullable { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }

        /* not in use. reduce memory.
        public int object_id { get; set; }
        public byte system_type_id { get; set; }
        public int user_type_id { get; set; }
        public string collation_name { get; set; }
        public bool is_ansi_padded { get; set; }
        public bool is_rowguidcol { get; set; }
        public bool is_filestream { get; set; }
        public bool? is_replicated { get; set; }
        public bool? is_non_sql_subscribed { get; set; }
        public bool? is_merge_published { get; set; }
        public bool? is_dts_replicated { get; set; }
        public bool is_xml_document { get; set; }
        public int xml_collection_id { get; set; }
        public int default_object_id { get; set; }
        public int rule_object_id { get; set; }
        public bool? is_sparse { get; set; }
        public bool? is_column_set { get; set; }
        */

        #endregion

        #region IColumn

        public string ColumnName { get { return name; } }
        public int? ColumnOrdinal { get { return column_id; } }
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

        public int? StringPrecision
        {
            get
            {
                if (max_length > 0)
                    return (data_type.ToLower() == "nchar" || data_type.ToLower() == "nvarchar" ? max_length / 2 : max_length);
                else
                    return max_length;
            }
        }

        public int? NumericPrecision { get { return (int?)precision; } }
        public int? NumericScale { get { return scale; } }
        public int? DateTimePrecision { get { return scale; } }
        public bool IsUnsigned { get { return false; } }
        public bool IsNullable { get { return (is_nullable != null && is_nullable.Value); } }
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
                    if (max_length == -1)
                        precision = "(max)";
                    else if (max_length > 0)
                        precision = "(" + (dataType == "nchar" || dataType == "nvarchar" ? max_length / 2 : max_length) + ")";
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

        #region ITVPColumn

        public ITVP TVP { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return ColumnName + " (" + DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion
    }
}
