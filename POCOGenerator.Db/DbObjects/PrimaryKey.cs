using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class PrimaryKey : IPrimaryKey
    {
        public string Name { get; set; }
        public ITable Table { get; set; }
        public List<IPrimaryKeyColumn> PrimaryKeyColumns { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
