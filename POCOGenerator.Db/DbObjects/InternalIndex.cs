using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public interface IInternalIndex
    {
        bool Is_Table_Index { get; }
        bool Is_View_Index { get; }
        string Name { get; }
        string Table_Name { get; }
        bool Is_Unique { get; }
        bool Is_Clustered { get; }
        byte Ordinal { get; }
        string Column_Name { get; }
        bool Is_Descending { get; }
    }

    public abstract class InternalIndexBase : IInternalIndex
    {
        public virtual bool Is_Table_Index { get; set; }
        public virtual bool Is_View_Index { get; set; }
        public virtual string Name { get; set; }
        public virtual string Table_Name { get; set; }
        public virtual bool Is_Unique { get; set; }
        public virtual bool Is_Clustered { get; set; }
        public virtual byte Ordinal { get; set; }
        public virtual string Column_Name { get; set; }
        public virtual bool Is_Descending { get; set; }
    }

    public sealed class IndexInternal : InternalIndexBase
    {
    }

    public sealed class IndexSchemaInternal : InternalIndexBase, ISchema
    {
        public string Schema { get; set; }
    }
}
