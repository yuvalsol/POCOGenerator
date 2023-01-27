using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IComplexTypeTableColumn : IColumn
    {
        IComplexTypeTable ComplexTypeTable { get; set; }
        string ColumnDefault { get; }
        List<ITableColumn> TableColumns { get; } // cross reference to table columns

        string ToFullString();
    }
}
