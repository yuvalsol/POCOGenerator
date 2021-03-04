using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IUniqueKey : IDbObject
    {
        string Name { get; }
        ITable Table { get; }
        List<IUniqueKeyColumn> UniqueKeyColumns { get; }
    }
}
