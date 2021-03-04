using System;

namespace POCOGenerator.DbObjects
{
    public interface IPrimaryKeyColumn : IDbObject
    {
        IPrimaryKey PrimaryKey { get; }
        ITableColumn TableColumn { get; }
        byte Ordinal { get; }

        string ToFullString();
    }
}
