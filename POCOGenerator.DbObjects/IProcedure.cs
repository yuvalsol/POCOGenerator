using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IProcedure : IDbObjectTraverse, IDescription
    {
        List<IProcedureParameter> ProcedureParameters { get; set; }
        List<IProcedureColumn> ProcedureColumns { get; set; }
    }
}
