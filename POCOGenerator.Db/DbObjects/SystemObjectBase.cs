using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public abstract class SystemObjectBase : ISystemObject
    {
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
