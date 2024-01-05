using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a way to navigate a relationship, defined by a foreign key, between tables.</summary>
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

        /// <summary>Gets the table that this navigation property is referencing from.</summary>
        /// <value>The table that this navigation property is referencing from.</value>
        public Table FromTable { get; private set; }

        /// <summary>Gets the name of the navigation property.</summary>
        /// <value>The name of the navigation property.</value>
        public string PropertyName { get { return this.ToString(); } }

        /// <summary>Gets a value indicating whether this navigation property is referencing to a collection of <see cref="ToTable" />.</summary>
        /// <value>
        ///   <c>true</c> if this navigation property is a collection of <see cref="ToTable" />; otherwise, <c>false</c>.</value>
        public bool IsCollection { get { return this.navigationProperty.IsCollection; } }

        /// <summary>Gets a value indicating whether this is a navigation property between tables that are connected to each other through a many-to-many join table.
        /// <para>If this navigation property is virtual then <see cref="ForeignKey" /> returns <see langword="null" /> because there is no foreign key that defines this navigation property.</para>
        /// <para>If <see cref="NavigationProperties.ManyToManyJoinTable" /> is <c>true</c> then this property returns <c>false</c> because all join tables are not hidden.</para></summary>
        /// <value>
        ///   <c>true</c> if this navigation property is a virtual navigation property; otherwise, <c>false</c>.</value>
        /// <seealso cref="ForeignKey" />
        /// <seealso cref="NavigationProperties.ManyToManyJoinTable" />
        public bool IsVirtualNavigationProperty { get { return this.navigationProperty.IsVirtualNavigationProperty; } }

        /// <summary>Gets a value indicating whether this navigation property is visible when <see cref="NavigationProperties.ManyToManyJoinTable" /> is <c>true</c>.</summary>
        /// <value>
        ///   <c>true</c> if this navigation property is visible when <see cref="NavigationProperties.ManyToManyJoinTable" /> is <c>true</c>; otherwise, <c>false</c>.</value>
        public bool IsVisibleWhenManyToManyJoinTableIsOn { get { return this.navigationProperty.IsVisibleWhenManyToManyJoinTableIsOn; } }

        /// <summary>Gets a value indicating whether this navigation property is visible when <see cref="NavigationProperties.ManyToManyJoinTable" /> is <c>false</c>.</summary>
        /// <value>
        ///   <c>true</c> if this navigation property is visible when <see cref="NavigationProperties.ManyToManyJoinTable" /> is <c>false</c>; otherwise, <c>false</c>.</value>
        public bool IsVisibleWhenManyToManyJoinTableIsOff { get { return this.navigationProperty.IsVisibleWhenManyToManyJoinTableIsOff; } }

        private Table toTable;
        /// <summary>Gets the table that this navigation property is referencing to.</summary>
        /// <value>The table that this navigation property is referencing to.</value>
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
        /// <summary>Gets the foreign key that defines the relationship between the tables.
        /// <para>If <see cref="IsVirtualNavigationProperty" /> is <c>true</c> then this property returns <see langword="null" /> because there is no foreign key that defines this navigation property.</para></summary>
        /// <value>The foreign key that defines the relationship between the tables.</value>
        /// <seealso cref="IsVirtualNavigationProperty" />
        /// <seealso cref="NavigationProperties.ManyToManyJoinTable" />
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

        private NavigationProperty inverseProperty;
        /// <summary>Gets the navigation property from <see cref="ToTable" /> to <see cref="FromTable" />.</summary>
        /// <value>The inverse navigation property.</value>
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

        /// <summary>Returns a string that represents this navigation property.</summary>
        /// <returns>A string that represents this navigation property.</returns>
        public override string ToString()
        {
            return this.navigationProperty.ToString();
        }
    }
}
