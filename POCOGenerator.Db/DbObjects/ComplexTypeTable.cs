using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal class ComplexTypeTable : IComplexTypeTable
    {
        public List<IComplexTypeTableColumn> ComplexTypeTableColumns { get; set; }

        public List<ITable> Tables { get; internal set; }

        #region IDbObjectTraverse

        public string Name { get; internal set; }
        public IEnumerable<IColumn> Columns { get; internal set; }
        public DbObjectType DbObjectType { get; internal set; }
        public IDatabase Database { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }

    internal sealed class ComplexTypeTableWithSchema : ComplexTypeTable, ISchema
    {
        #region ISchema

        public string Schema { get; internal set; }

        #endregion
    }
}
