using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class NavigationProperty
    {
        private readonly POCOGenerator.DbObjects.INavigationProperty navigationProperty;

        internal NavigationProperty(POCOGenerator.DbObjects.INavigationProperty navigationProperty, Table fromTable)
        {
            this.navigationProperty = navigationProperty;
            this.FromTable = fromTable;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.INavigationProperty navigationProperty)
        {
            return this.navigationProperty == navigationProperty;
        }

        public Table FromTable { get; private set; }

        public string PropertyName { get { return this.ToString(); } }
        public bool IsCollection { get { return this.navigationProperty.IsCollection; } }
        public bool IsVirtualNavigationProperty { get { return this.navigationProperty.IsVirtualNavigationProperty; } }
        public bool IsVisibleWhenShowManyToManyJoinTableIsOn { get { return this.navigationProperty.IsVisibleWhenShowManyToManyJoinTableIsOn; } }
        public bool IsVisibleWhenShowManyToManyJoinTableIsOff { get { return this.navigationProperty.IsVisibleWhenShowManyToManyJoinTableIsOff; } }

        private Table toTable;
        public Table ToTable
        {
            get
            {
                if (this.toTable == null)
                {
                    POCOGenerator.DbObjects.ITable table = (
                        this.navigationProperty.IsFromForeignToPrimary ?
                        this.navigationProperty.ForeignKey.PrimaryTable :
                        this.navigationProperty.ForeignKey.ForeignTable
                    );

                    this.toTable =
                        this.FromTable.Database.Tables
                        .Union(this.FromTable.Database.AccessibleTables)
                        .First(t => t.InternalEquals(table));
                }

                return this.toTable;
            }
        }

        private ForeignKey foreignKey;
        public ForeignKey ForeignKey
        {
            get
            {
                if (this.navigationProperty.IsVirtualNavigationProperty)
                    return null;

                if (foreignKey == null)
                {
                    if (this.navigationProperty.IsFromForeignToPrimary)
                        this.foreignKey = this.FromTable.ForeignKeys.First(fk => fk.InternalEquals(this.navigationProperty.ForeignKey));
                    else
                        this.foreignKey = this.ToTable.ForeignKeys.First(fk => fk.InternalEquals(this.navigationProperty.ForeignKey));
                }

                return foreignKey;
            }
        }

        public NavigationProperty inverseProperty;
        public NavigationProperty InverseProperty
        {
            get
            {
                if (this.navigationProperty.InverseProperty == null)
                    return null;

                if (this.navigationProperty.IsVirtualNavigationProperty)
                    return null;

                if (inverseProperty == null)
                    inverseProperty = this.ToTable.NavigationProperties.First(np => np.InternalEquals(this.navigationProperty.InverseProperty));

                return inverseProperty;
            }
        }

        public override string ToString()
        {
            return this.navigationProperty.ToString();
        }
    }
}
