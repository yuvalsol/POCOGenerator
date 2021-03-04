using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IDbObjectTraverse : IDbObject
    {
        string Name { get; }
        IEnumerable<IColumn> Columns { get; }
        DbObjectType DbObjectType { get; }
        IDatabase Database { get; set; }
        Exception Error { get; set; }
        string ClassName { get; set; }
    }
}
