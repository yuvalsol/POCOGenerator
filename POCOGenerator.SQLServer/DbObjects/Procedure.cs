using System;
using System.Collections.Generic;
using System.Linq;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class Procedure : IProcedure, ISchema
    {
        public string routine_schema { get; set; }
        public string routine_name { get; set; }

        public IDatabase Database { get; set; }
        public List<IProcedureParameter> ProcedureParameters { get; set; }
        public List<IProcedureColumn> ProcedureColumns { get; set; }
        public string Description { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public override string ToString()
        {
            return Schema + "." + Name;
        }

        #region IDbObjectTraverse Members

        public string Schema { get { return routine_schema; } }
        public string Name { get { return routine_name; } }
        public IEnumerable<IColumn> Columns { get { return (ProcedureColumns != null ? ProcedureColumns.Cast<IColumn>() : null); } }
        public virtual DbObjectType DbObjectType { get { return DbObjectType.Procedure; } }

        #endregion
    }
}
