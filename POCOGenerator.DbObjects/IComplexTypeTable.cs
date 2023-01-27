using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IComplexTypeTable : IDbObjectTraverse
    {
        List<IComplexTypeTableColumn> ComplexTypeTableColumns { get; set; }

        List<ITable> Tables { get; } // cross reference to tables
    }
}
