using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class ForeignKeyColumn : IForeignKeyColumn
    {
        public IForeignKey ForeignKey { get; set; }

        public ITableColumn ForeignTableColumn { get; set; }
        public ITableColumn PrimaryTableColumn { get; set; }

        public bool Is_Foreign_Column_PK { get; set; }
        public bool Is_Primary_Column_PK { get; set; }
        public int Ordinal { get; set; }

        public bool IsVirtualForeignKeyColumn { get; set; }

        public override string ToString()
        {
            return ForeignTableColumn.ToString();
        }

        public string ToFullString()
        {
            return ForeignTableColumn.ToFullString();
        }
    }
}
