using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface ITable : IDbObjectTraverse, IDescription
    {
        List<ITableColumn> TableColumns { get; set; }
        IPrimaryKey PrimaryKey { get; set; }
        List<IUniqueKey> UniqueKeys { get; set; }
        List<IForeignKey> ForeignKeys { get; set; } // this table is the foreign table of the foreign key
        List<IForeignKey> PrimaryForeignKeys { get; set; } // this table is the primary table of the foreign key
        List<IIndex> Indexes { get; set; }
        bool IsJoinTable { get; set; }
    }
}
