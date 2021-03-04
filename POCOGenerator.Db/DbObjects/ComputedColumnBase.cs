using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public abstract class ComputedColumnBase : IComputedColumn
    {
        public virtual string Table_Name { get; set; }
        public virtual string Column_Name { get; set; }

        public override string ToString()
        {
            return Column_Name;
        }
    }
}
