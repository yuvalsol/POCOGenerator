using System;
using System.Data;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class ProcedureParameter : IProcedureParameter
    {
        #region Database Properties

        public int? ORDINAL_POSITION { get; set; }
        public string PARAMETER_MODE { get; set; }
        public string PARAMETER_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public byte? NUMERIC_PRECISION { get; set; }
        public int? NUMERIC_SCALE { get; set; }
        public string DTD_IDENTIFIER { get; set; }

        /* not in use. reduce memory.
        public string SPECIFIC_CATALOG { get; set; }
        public string SPECIFIC_SCHEMA { get; set; }
        public string SPECIFIC_NAME { get; set; }
        public int? CHARACTER_OCTET_LENGTH { get; set; }
        public string CHARACTER_SET_NAME { get; set; }
        public string COLLATION_NAME { get; set; }
        public string ROUTINE_TYPE { get; set; }
        */

        #endregion

        #region IProcedureParameter

        public IProcedure Procedure { get; set; }
        public string ParameterName { get { return PARAMETER_NAME; } }
        public string ParameterDataType { get { return DATA_TYPE; } }
        public bool ParameterIsUnsigned { get { return string.IsNullOrEmpty(DTD_IDENTIFIER) == false && DTD_IDENTIFIER.ToLower().Contains("unsigned"); } }
        public int? ParameterOrdinal { get { return ORDINAL_POSITION; } }
        public int? ParameterSize { get { return CHARACTER_MAXIMUM_LENGTH; } }
        public byte? ParameterPrecision { get { return NUMERIC_PRECISION; } }
        public int? ParameterScale { get { return NUMERIC_SCALE; } }
        public int? ParameterDateTimePrecision { get { return CHARACTER_MAXIMUM_LENGTH; } }
        public string ParameterMode { get { return PARAMETER_MODE; } }

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

        public bool IsResult { get { return false; } }

        #endregion

        #region IDescription

        public string Description { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return ParameterName + " (" + DataTypeDisplay + Precision + (ParameterIsUnsigned ? " unsigned" : string.Empty) + ", " + Direction + ")";
        }

        public string DataTypeDisplay
        {
            get
            {
                return DATA_TYPE.ToLower();
            }
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
                    if (ParameterSize != null)
                        precision = "(" + ParameterSize + ")";
                }
                else if (dataType == "bit")
                {
                    if (ParameterSize != null)
                        precision = "(" + ParameterSize + ")";
                }
                else if (dataType == "decimal" || dataType == "numeric" || dataType == "dec" || dataType == "fixed")
                {
                    if (ParameterPrecision != null && ParameterScale != null)
                        precision = "(" + ParameterPrecision + "," + ParameterScale + ")";
                }
                else if (dataType == "datetime" || dataType == "time" || dataType == "timestamp")
                {
                    if (ParameterDateTimePrecision != null && ParameterDateTimePrecision > 0)
                        precision = "(" + ParameterDateTimePrecision + ")";
                }
                else if (dataType == "enum" || dataType == "set")
                {
                    if (string.IsNullOrEmpty(DTD_IDENTIFIER) == false)
                    {
                        int startIndex = DTD_IDENTIFIER.IndexOf('(');
                        if (startIndex != -1)
                        {
                            int endIndex = DTD_IDENTIFIER.LastIndexOf(')');
                            if (endIndex != -1)
                                precision = DTD_IDENTIFIER.Substring(startIndex, endIndex - startIndex + 1);
                        }
                    }
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
