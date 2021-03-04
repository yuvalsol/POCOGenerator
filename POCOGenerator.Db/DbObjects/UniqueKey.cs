using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class UniqueKey : IUniqueKey
    {
        public string Name { get; set; }
        public ITable Table { get; set; }
        public List<IUniqueKeyColumn> UniqueKeyColumns { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
