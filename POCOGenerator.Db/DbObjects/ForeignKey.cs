using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class ForeignKey : IForeignKey
    {
        public string Name { get; set; }
        public bool Is_One_To_One { get; set; }
        public bool Is_One_To_Many { get; set; }
        public bool Is_Many_To_Many { get; set; }
        public bool Is_Many_To_Many_Complete { get; set; }
        public bool Is_Cascade_Delete { get; set; }
        public bool Is_Cascade_Update { get; set; }

        public ITable ForeignTable { get; set; }
        public ITable PrimaryTable { get; set; }

        public List<IForeignKeyColumn> ForeignKeyColumns { get; set; }

        public INavigationProperty NavigationPropertyFromForeignToPrimary { get; set; }
        public INavigationProperty NavigationPropertyFromPrimaryToForeign { get; set; }

        public bool IsVirtualForeignKey { get; set; }

        public List<INavigationProperty> VirtualNavigationProperties { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
