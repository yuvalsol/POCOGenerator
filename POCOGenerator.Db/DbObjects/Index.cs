using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class Index : IIndex
    {
        public string Name { get; set; }
        public ITable Table { get; set; } // can be IView
        public bool Is_Unique { get; set; }
        public bool Is_Clustered { get; set; }
        public List<IIndexColumn> IndexColumns { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public string ToFullString()
        {
            return
                Name + " (" +
                (Is_Unique ? "unique" : "not unique") + ", " +
                (Is_Clustered ? "clustered" : "not clustered") + ")";
        }
    }
}
