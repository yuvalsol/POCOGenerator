using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class UniqueKeyColumn : IUniqueKeyColumn
    {
        public IUniqueKey UniqueKey { get; set; }
        public ITableColumn TableColumn { get; set; }
        public byte Ordinal { get; set; }

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
