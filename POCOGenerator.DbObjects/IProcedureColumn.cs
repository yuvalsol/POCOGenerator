using System;

namespace POCOGenerator.DbObjects
{
    public interface IProcedureColumn : IColumn
    {
        IProcedure Procedure { get; set; }
    }
}
