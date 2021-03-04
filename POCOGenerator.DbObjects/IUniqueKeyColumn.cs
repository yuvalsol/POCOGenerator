using System;

namespace POCOGenerator.DbObjects
{
    public interface IUniqueKeyColumn : IDbObject
    {
        IUniqueKey UniqueKey { get; }
        ITableColumn TableColumn { get; }
        byte Ordinal { get; }

        string ToFullString();
    }
}
