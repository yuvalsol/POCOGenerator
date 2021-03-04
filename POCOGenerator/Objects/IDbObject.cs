using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    public interface IDbObject
    {
        Database Database { get; }
        string Name { get; }
        string Schema { get; }
        string Description { get; }

        IEnumerable<IDbColumn> Columns { get; }

        string Error { get; }

        string ToString();
    }
}
