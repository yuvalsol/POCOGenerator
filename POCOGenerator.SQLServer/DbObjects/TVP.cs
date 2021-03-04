using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class TVP : ITVP, ISchema
    {
        public string tvp_schema { get; set; }
        public string tvp_name { get; set; }
        public int type_table_object_id { get; set; }

        public IDatabase Database { get; set; }
        public List<ITVPColumn> TVPColumns { get; set; }
        public string Description { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        public int TVPId { get { return type_table_object_id; } }
        public DataTable TVPDataTable { get; set; }

        public override string ToString()
        {
            return Schema + "." + Name;
        }

        #region IDbObjectTraverse Members

        public string Schema { get { return tvp_schema; } }
        public string Name { get { return tvp_name; } }
        public IEnumerable<IColumn> Columns { get { return (TVPColumns != null ? TVPColumns.Cast<IColumn>() : null); } }
        public DbObjectType DbObjectType { get { return DbObjectType.TVP; } }

        #endregion
    }
}
