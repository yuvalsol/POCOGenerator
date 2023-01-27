using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface ITableColumn : IColumn, IDescription
    {
        ITable Table { get; set; }
        IPrimaryKeyColumn PrimaryKeyColumn { get; set; }
        List<IUniqueKeyColumn> UniqueKeyColumns { get; set; }
        List<IForeignKeyColumn> ForeignKeyColumns { get; set; } // referencing from this table column to another table column. this is the foreign table column.
        List<IForeignKeyColumn> PrimaryForeignKeyColumns { get; set; } // referenced from another table column to this table column. this is the primary table column.
        List<IIndexColumn> IndexColumns { get; set; }
        IComplexTypeTableColumn ComplexTypeTableColumn { get; set; } // cross reference to complex type table column
        string ColumnDefault { get; }

        string ToFullString();
    }
}
