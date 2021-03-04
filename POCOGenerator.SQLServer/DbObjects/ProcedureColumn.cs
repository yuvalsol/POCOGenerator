using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class ProcedureColumn : IProcedureColumn
    {
        #region Database Properties

        public string ColumnName { get; set; }
        public int? ColumnOrdinal { get; set; }
        public int? ColumnSize { get; set; }
        public int? NumericPrecision { get; set; } // originally short?
        public int? NumericScale { get; set; } // originally short?
        public bool? AllowDBNull { get; set; }
        public bool IsIdentity { get; set; } // originally bool?
        public bool? IsLong { get; set; }
        public string DataTypeName { get; set; }

        /* not in use. reduce memory.
        public bool? IsUnique { get; set; }
        public bool? IsKey { get; set; }
        public string BaseServerName { get; set; }
        public string BaseCatalogName { get; set; }
        public string BaseColumnName { get; set; }
        public string BaseSchemaName { get; set; }
        public string BaseTableName { get; set; }
        public Type DataType { get; set; }
        public int? ProviderType { get; set; }
        public bool? IsAliased { get; set; }
        public bool? IsExpression { get; set; }
        public bool? IsAutoIncrement { get; set; }
        public bool? IsRowVersion { get; set; }
        public bool? IsHidden { get; set; }
        public bool? IsReadOnly { get; set; }
        public Type ProviderSpecificDataType { get; set; }
        public string XmlSchemaCollectionDatabase { get; set; }
        public string XmlSchemaCollectionOwningSchema { get; set; }
        public string XmlSchemaCollectionName { get; set; }
        public string UdtAssemblyQualifiedName { get; set; }
        public int? NonVersionedProviderType { get; set; }
        public bool? IsColumnSet { get; set; }
        */

        #endregion

        #region IColumn

        public int? StringPrecision { get { return (IsLong == true ? -1 : ColumnSize); } }
        public int? DateTimePrecision { get { return NumericScale; } }
        public bool IsUnsigned { get { return false; } }
        public bool IsNullable { get { return (AllowDBNull ?? false); } }
        public bool IsComputed { get { return false; } set { } }

        public string DataTypeDisplay
        {
            get
            {
                if (DataTypeName == "xml")
                    return "XML";
                // sys.geography, sys.geometry, sys.hierarchyid
                if (DataTypeName.Contains("sys."))
                    return DataTypeName.Substring(DataTypeName.IndexOf("sys.") + 4);
                return DataTypeName;
            }
        }

        public string Precision
        {
            get
            {
                string precision = null;

                string dataType = DataTypeName.ToLower();

                if (dataType == "binary" || dataType == "varbinary" || dataType == "char" || dataType == "nchar" || dataType == "nvarchar" || dataType == "varchar")
                {
                    if (IsLong == true)
                        precision = "(max)";
                    else if (ColumnSize > 0)
                        precision = "(" + ColumnSize + ")";
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

        #region IProcedureColumn

        public IProcedure Procedure { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return ColumnName + " (" + DataTypeDisplay + Precision + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion
    }
}
