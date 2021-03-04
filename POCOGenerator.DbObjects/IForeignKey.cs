using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IForeignKey : IDbObject
    {
        string Name { get; }
        bool Is_One_To_One { get; }
        bool Is_One_To_Many { get; }
        bool Is_Many_To_Many { get; }
        bool Is_Many_To_Many_Complete { get; }
        bool Is_Cascade_Delete { get; }
        bool Is_Cascade_Update { get; }

        ITable ForeignTable { get; }
        ITable PrimaryTable { get; }

        List<IForeignKeyColumn> ForeignKeyColumns { get; }

        INavigationProperty NavigationPropertyFromForeignToPrimary { get; }
        INavigationProperty NavigationPropertyFromPrimaryToForeign { get; }

        // when true, this is a foreign key between this foreign and primary tables that are connected between them with a many-to-many join table
        bool IsVirtualForeignKey { get; }

        // navigation properties between this primary table and other tables that are connected between them with a many-to-many join table
        List<INavigationProperty> VirtualNavigationProperties { get; }
    }
}
