using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IComplexType
    {
        List<IComplexTypeTable> ComplexTypeTables { get; }
    }

    public interface IComplexTypeTable : ITable
    {
        ITable SourceTable { get; }
        string PropertyName { get; }
    }

    public interface IComplexTypeTableColumn : ITableColumn
    {
        ITableColumn SourceTableColumn { get; }
    }
}
