using System;
using System.Data;

namespace POCOGenerator.Objects
{
    public interface IDbParameter
    {
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
        string Description { get; }

        IDbObject DbObject { get; }

        string ToString();
    }
}
