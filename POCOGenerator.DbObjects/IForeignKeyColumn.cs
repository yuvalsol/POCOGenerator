using System;

namespace POCOGenerator.DbObjects
{
    public interface IForeignKeyColumn : IDbObject
    {
        IForeignKey ForeignKey { get; }

        ITableColumn ForeignTableColumn { get; }
        ITableColumn PrimaryTableColumn { get; }

        bool Is_Foreign_Column_PK { get; }
        bool Is_Primary_Column_PK { get; }
        int Ordinal { get; }

        // when true, this is a foreign key column between this foreign key's foreign and primary tables that are connected between them with a many-to-many join table
        bool IsVirtualForeignKeyColumn { get; }

        string ToFullString();
    }
}
