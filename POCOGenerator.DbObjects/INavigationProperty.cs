using System;

namespace POCOGenerator.DbObjects
{
    public interface INavigationProperty : IDbObject
    {
        IForeignKey ForeignKey { get; }
        bool IsFromForeignToPrimary { get; }
        bool IsCollection { get; }
        string PropertyName { get; }

        INavigationProperty InverseProperty { get; }

        // when true, this is a navigation property between 2 tables that are connected between them with a many-to-many join table
        bool IsVirtualNavigationProperty { get; }

        bool IsVisibleWhenManyToManyJoinTableIsOn { get; }
        bool IsVisibleWhenManyToManyJoinTableIsOff { get; }

        string RenamedPropertyName { get; set; }
        string ClassName { get; set; }
        bool HasMultipleRelationships { get; set; }
    }
}
