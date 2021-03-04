using System;

namespace POCOGenerator.DbObjects
{
    public interface IIndexColumn : IDbObject
    {
        IIndex Index { get; }
        ITableColumn TableColumn { get; }
        byte Ordinal { get; }
        bool Is_Descending { get; }

        string ToFullString();
    }
}
