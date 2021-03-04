using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IPrimaryKey : IDbObject
    {
        string Name { get; }
        ITable Table { get; }
        List<IPrimaryKeyColumn> PrimaryKeyColumns { get; }
    }
}
