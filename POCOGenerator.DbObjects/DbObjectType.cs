using System;

namespace POCOGenerator.DbObjects
{
    public enum DbObjectType
    {
        None = 0,
        Server = 1,
        Database = 2,
        Schema = 3,
        Tables = 4,
        Table = 5,
        Views = 6,
        View = 7,
        Columns = 8,
        Column = 9,
        PrimaryKeys = 10,
        PrimaryKey = 11,
        UniqueKeys = 12,
        UniqueKey = 13,
        ForeignKeys = 14,
        ForeignKey = 15,
        Indexes = 16,
        Index = 17,
        Procedures = 18,
        Procedure = 19,
        Functions = 20,
        Function = 21,
        ProcedureParameters = 22,
        ProcedureParameter = 23,
        ProcedureColumns = 24,
        ProcedureColumn = 25,
        TVPs = 26,
        TVP = 27,
        TVPColumns = 28,
        TVPColumn = 29
    }
}