using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public abstract class DbObjectDescriptionBase : IDbObjectDescription
    {
        public virtual string Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string Minor_Name { get; set; }
        public virtual string Description { get; set; }

        public virtual DbObjectType ObjectType
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                    return DbObjectType.None;

                switch (Type.ToLower())
                {
                    case "database": return DbObjectType.Database;
                    case "schema": return DbObjectType.Schema;
                    case "table": return DbObjectType.Table;
                    case "view": return DbObjectType.View;
                    case "tablecolumn": return DbObjectType.Column;
                    case "viewcolumn": return DbObjectType.Column;
                    case "index": return DbObjectType.Index;
                    case "procedure": return DbObjectType.Procedure;
                    case "function": return DbObjectType.Function;
                    case "procedureparameter": return DbObjectType.ProcedureParameter;
                    case "functionparameter": return DbObjectType.ProcedureParameter;
                    case "tvp": return DbObjectType.TVP;
                    case "tvpcolumn": return DbObjectType.TVPColumn;
                    default: return DbObjectType.None;
                }
            }
        }
    }
}
