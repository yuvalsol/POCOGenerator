using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class IndexColumn : IIndexColumn
    {
        public IIndex Index { get; set; }
        public ITableColumn TableColumn { get; set; }
        public byte Ordinal { get; set; }
        public bool Is_Descending { get; set; }

        public override string ToString()
        {
            return TableColumn.ToString();
        }

        public string ToFullString()
        {
            return TableColumn.ToFullString();
        }
    }
}
