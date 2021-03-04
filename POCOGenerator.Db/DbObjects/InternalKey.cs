using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public interface IInternalKey
    {
        string Name { get; }
        string Table_Name { get; }
        byte Ordinal { get; }
        string Column_Name { get; }
    }

    public abstract class InternalKeyBase : IInternalKey
    {
        public virtual string Name { get; set; }
        public virtual string Table_Name { get; set; }
        public virtual byte Ordinal { get; set; }
        public virtual string Column_Name { get; set; }
    }

    public sealed class PrimaryKeyInternal : InternalKeyBase
    {
    }

    public sealed class PrimaryKeySchemaInternal : InternalKeyBase, ISchema
    {
        public string Schema { get; set; }
    }

    public sealed class UniqueKeyInternal : InternalKeyBase
    {
    }

    public sealed class UniqueKeySchemaInternal : InternalKeyBase, ISchema
    {
        public string Schema { get; set; }
    }
}
