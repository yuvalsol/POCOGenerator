using System;

namespace POCOGenerator.DbObjects
{
    public interface IComputedColumn : IDbObject
    {
        string Table_Name { get; }
        string Column_Name { get; }
    }
}
