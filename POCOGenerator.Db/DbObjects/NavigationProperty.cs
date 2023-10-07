using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class NavigationProperty : INavigationProperty
    {
        public IForeignKey ForeignKey { get; set; }
        public bool IsFromForeignToPrimary { get; set; }
        public bool IsCollection { get; set; }
        public string PropertyName { get; set; }

        public INavigationProperty InverseProperty { get; set; }

        public bool IsVirtualNavigationProperty { get; set; }

        public bool IsVisibleWhenManyToManyJoinTableIsOn { get; set; }
        public bool IsVisibleWhenManyToManyJoinTableIsOff { get; set; }

        public string RenamedPropertyName { get; set; }
        public string ClassName { get; set; }
        public bool HasMultipleRelationships { get; set; }

        public override string ToString()
        {
            return (string.IsNullOrEmpty(RenamedPropertyName) ? PropertyName : RenamedPropertyName);
        }
    }
}
