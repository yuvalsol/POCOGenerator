using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class TableColumn : ITableColumn, IEnumColumn
    {
        #region Database Properties

        public string COLUMN_NAME { get; set; }
        public ulong? ORDINAL_POSITION { get; set; }
        public string COLUMN_DEFAULT { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public ulong? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public ulong? NUMERIC_PRECISION { get; set; }
        public ulong? NUMERIC_SCALE { get; set; }
        public ulong? DATETIME_PRECISION { get; set; }
        public string COLUMN_TYPE { get; set; }
        public string EXTRA { get; set; }

        /* not in use. reduce memory.
        public virtual string TABLE_CATALOG { get; set; }
        public virtual string TABLE_SCHEMA { get; set; }
        public virtual string TABLE_NAME { get; set; }
        public string CHARACTER_SET_NAME { get; set; }
        public string COLLATION_NAME { get; set; }
        public string COLUMN_KEY { get; set; }
        public string PRIVILEGES { get; set; }
        public string COLUMN_COMMENT { get; set; }
        public string GENERATION_EXPRESSION { get; set; }
        */

        #endregion

        #region IColumn

        public string ColumnName { get { return COLUMN_NAME; } }
        public int? ColumnOrdinal { get { return (int?)ORDINAL_POSITION; } }
        public string DataTypeName { get { return DATA_TYPE; } }
        public string DataTypeDisplay { get { return DATA_TYPE.ToLower(); } }
        public int? StringPrecision { get { return (int?)CHARACTER_MAXIMUM_LENGTH; } }
        public int? NumericPrecision { get { return (int?)NUMERIC_PRECISION; } }
        public int? NumericScale { get { return (int?)NUMERIC_SCALE; } }
        public int? DateTimePrecision { get { return (int?)DATETIME_PRECISION; } }
        public bool IsUnsigned { get { return (string.IsNullOrEmpty(COLUMN_TYPE) == false && COLUMN_TYPE.ToLower().Contains("unsigned")); } }
        public bool IsNullable { get { return string.Compare(IS_NULLABLE, "YES", true) == 0; } }

        private bool? is_identity;
        public bool IsIdentity
        {
            get
            {
                if (is_identity == null)
                    is_identity = string.IsNullOrEmpty(EXTRA) == false && EXTRA.ToLower().Contains("auto_increment");
                return is_identity.Value;
            }
            set { is_identity = value; }
        }

        private bool? is_computed;
        public bool IsComputed
        {
            get
            {
                if (is_computed == null)
                    is_computed = string.IsNullOrEmpty(EXTRA) == false && (EXTRA.ToUpper().Contains("VIRTUAL GENERATED") || EXTRA.ToUpper().Contains("VIRTUAL STORED"));
                return is_computed.Value;
            }
            set { is_computed = value; }
        }

        public string Precision
        {
            get
            {
                string precision = null;

                string dataType = DATA_TYPE.ToLower();

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
                        precision = "(" + NumericPrecision + ")";
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
                else if (dataType == "enum" || dataType == "set")
                {
                    if (string.IsNullOrEmpty(COLUMN_TYPE) == false)
                    {
                        int startIndex = COLUMN_TYPE.IndexOf('(');
                        if (startIndex != -1)
                        {
                            int endIndex = COLUMN_TYPE.LastIndexOf(')');
                            if (endIndex != -1)
                                precision = COLUMN_TYPE.Substring(startIndex, endIndex - startIndex + 1);
                        }
                    }
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

        public string ColumnDefault { get { return COLUMN_DEFAULT; } }

        public string ToFullString()
        {
            return
                ColumnName + " (" +
                (PrimaryKeyColumn != null ? "PK, " : string.Empty) +
                (ForeignKeyColumns.HasAny() ? "FK, " : string.Empty) +
                (IsComputed ? "Generated, " : string.Empty) +
                DATA_TYPE + Precision + (IsUnsigned ? " unsigned" : string.Empty) + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion

        #region IEnumColumn

        public IColumn Column { get { return this; } }
        public bool IsEnumDataType { get { return DATA_TYPE.ToLower() == "enum"; } }
        public bool IsSetDataType { get { return DATA_TYPE.ToLower() == "set"; } }

        private static readonly Regex enumLiteralsRegex = new Regex(@"^(?i:enum|set)\s*\((?:\s*,?\s*'(?<literal>.*?)')+\)$", RegexOptions.Compiled);

        private List<string> enumLiterals;
        public List<string> EnumLiterals
        {
            get
            {
                if (enumLiterals == null)
                {
                    Match match = enumLiteralsRegex.Match(COLUMN_TYPE);
                    if (match.Success)
                    {
                        Group group = match.Groups["literal"];
                        if (group.Success)
                            enumLiterals = group.Captures.Cast<Capture>().Select(c => c.Value).ToList();
                    }

                    if (enumLiterals == null)
                        enumLiterals = new List<string>();
                }

                return enumLiterals;
            }
        }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return ColumnName + " (" + DATA_TYPE + Precision + (IsUnsigned ? " unsigned" : string.Empty) + ", " + (IsNullable ? "null" : "not null") + ")";
        }

        #endregion
    }
}
