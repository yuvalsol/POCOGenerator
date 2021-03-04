using System;
using System.Data;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class ProcedureParameter : IProcedureParameter
    {
        #region Database Properties

        public int? ordinal_position { get; set; }
        public string parameter_mode { get; set; }
        public string is_result { get; set; }
        public string parameter_name { get; set; }
        public string data_type { get; set; }
        public int? character_maximum_length { get; set; }
        public byte? numeric_precision { get; set; }
        public int? numeric_scale { get; set; }
        public short? datetime_precision { get; set; }

        /* not in use. reduce memory.
        public string specific_catalog { get; set; }
        public string specific_schema { get; set; }
        public string specific_name { get; set; }
        public string as_locator { get; set; }
        public int? character_octet_length { get; set; }
        public string collation_catalog { get; set; }
        public string collation_schema { get; set; }
        public string collation_name { get; set; }
        public string character_set_catalog { get; set; }
        public string character_set_schema { get; set; }
        public string character_set_name { get; set; }
        public short? numeric_precision_radix { get; set; }
        public string interval_type { get; set; }
        public short? interval_precision { get; set; }
        */

        #endregion

        #region IProcedureParameter

        public IProcedure Procedure { get; set; }
        public string ParameterName { get { return parameter_name; } }
        public string ParameterDataType { get { return data_type; } }
        public bool ParameterIsUnsigned { get { return false; } }
        public int? ParameterOrdinal { get { return ordinal_position; } }
        public int? ParameterSize { get { return character_maximum_length; } }
        public byte? ParameterPrecision { get { return numeric_precision; } }
        public int? ParameterScale { get { return numeric_scale; } }
        public int? ParameterDateTimePrecision { get { return datetime_precision; } }
        public string ParameterMode { get { return parameter_mode; } }

        public ParameterDirection ParameterDirection
        {
            get
            {
                if (string.Compare(ParameterMode, "IN", true) == 0)
                    return ParameterDirection.Input;
                else if (string.Compare(ParameterMode, "INOUT", true) == 0)
                    return ParameterDirection.InputOutput;
                else if (string.Compare(ParameterMode, "OUT", true) == 0)
                    return ParameterDirection.Output;
                return ParameterDirection.Input;
            }
        }

        public bool IsResult { get { return string.Compare(is_result, "YES", true) == 0; } }

        #endregion

        #region IDescription

        public string Description { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            if (IsResult)
                return "Returns " + DataTypeDisplay + Precision;
            return ParameterName + " (" + DataTypeDisplay + Precision + ", " + Direction + ")";
        }

        public string DataTypeDisplay
        {
            get
            {
                if (ParameterDataType == "xml")
                    return "XML";
                return ParameterDataType;
            }
        }
        
        public string Precision
        {
            get
            {
                string precision = null;

                string dataType = ParameterDataType.ToLower();

                if (dataType == "binary" || dataType == "varbinary" || dataType == "char" || dataType == "nchar" || dataType == "nvarchar" || dataType == "varchar")
                {
                    if (ParameterSize == -1)
                        precision = "(max)";
                    else if (ParameterSize > 0)
                        precision = "(" + ParameterSize + ")";
                }
                else if (dataType == "decimal" || dataType == "numeric")
                {
                    precision = "(" + ParameterPrecision + "," + ParameterScale + ")";
                }
                else if (dataType == "datetime2" || dataType == "datetimeoffset" || dataType == "time")
                {
                    precision = "(" + ParameterDateTimePrecision + ")";
                }
                else if (dataType == "xml")
                {
                    precision = "(.)";
                }

                return precision;
            }
        }

        public string Direction
        {
            get
            {
                if (string.Compare(ParameterMode, "IN", true) == 0)
                    return "Input";
                else if (string.Compare(ParameterMode, "INOUT", true) == 0)
                    return "Input/Output";
                else if (string.Compare(ParameterMode, "OUT", true) == 0)
                    return "Output";
                return null;
            }
        }

        #endregion
    }
}
