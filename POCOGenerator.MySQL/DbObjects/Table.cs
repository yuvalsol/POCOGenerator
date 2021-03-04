using System;
using System.Collections.Generic;
using System.Linq;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class Table : ITable
    {
        public string TABLE_NAME { get; set; }

        public IDatabase Database { get; set; }
        public List<ITableColumn> TableColumns { get; set; }
        public IPrimaryKey PrimaryKey { get; set; }
        public List<IUniqueKey> UniqueKeys { get; set; }
        public List<IForeignKey> ForeignKeys { get; set; }
        public List<IForeignKey> PrimaryForeignKeys { get; set; }
        public List<IIndex> Indexes { get; set; }
        public bool IsJoinTable { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region IDbObjectTraverse Members

        public string Name { get { return TABLE_NAME; } }
        public IEnumerable<IColumn> Columns { get { return (TableColumns != null ? TableColumns.Cast<IColumn>() : null); } }
        public virtual DbObjectType DbObjectType { get { return DbObjectType.Table; } }

        #endregion
    }
}
