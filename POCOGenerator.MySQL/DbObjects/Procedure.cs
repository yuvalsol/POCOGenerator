using System;
using System.Collections.Generic;
using System.Linq;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class Procedure : IProcedure
    {
        public string SPECIFIC_NAME { get; set; }

        public IDatabase Database { get; set; }
        public List<IProcedureParameter> ProcedureParameters { get; set; }
        public List<IProcedureColumn> ProcedureColumns { get; set; }
        public string Description { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region IDbObjectTraverse Members

        public string Name { get { return SPECIFIC_NAME; } }
        public IEnumerable<IColumn> Columns { get { return (ProcedureColumns != null ? ProcedureColumns.Cast<IColumn>() : null); } }
        public virtual DbObjectType DbObjectType { get { return DbObjectType.Procedure; } }

        #endregion
    }
}
