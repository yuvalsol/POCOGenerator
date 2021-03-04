using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IIndex : IDbObject, IDescription
    {
        string Name { get; }
        ITable Table { get; } // can be IView
        bool Is_Unique { get; }
        bool Is_Clustered { get; }
        List<IIndexColumn> IndexColumns { get; }

        string ToFullString();
    }
}
