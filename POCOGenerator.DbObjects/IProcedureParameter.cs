using System;
using System.Data;

namespace POCOGenerator.DbObjects
{
    public interface IProcedureParameter : IDbObject, IDescription
    {
        IProcedure Procedure { get; set; }
        string ParameterName { get; }
        string ParameterDataType { get; }
        bool ParameterIsUnsigned { get; }
        int? ParameterOrdinal { get; }
        int? ParameterSize { get; }
        byte? ParameterPrecision { get; }
        int? ParameterScale { get; }
        int? ParameterDateTimePrecision { get; }
        string ParameterMode { get; }
        ParameterDirection ParameterDirection { get; }
        bool IsResult { get; }
    }
}
