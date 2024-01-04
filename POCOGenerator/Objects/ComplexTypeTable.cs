using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents entity framework complex type.</summary>
    public sealed class ComplexTypeTable : IDbObject
    {
        private readonly POCOGenerator.DbObjects.IComplexTypeTable complexTypeTable;

        internal ComplexTypeTable(POCOGenerator.DbObjects.IComplexTypeTable complexTypeTable, Database database)
        {
            this.complexTypeTable = complexTypeTable;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IComplexTypeTable complexTypeTable)
        {
            return this.complexTypeTable == complexTypeTable;
        }

        internal string ClassName { get { return this.complexTypeTable.ClassName; } }

        /// <summary>Gets the error message that occurred during the generating process of this complex type.</summary>
        /// <value>The error message that occurred during the generating process of this complex type.</value>
        public string Error { get { return this.complexTypeTable.Error?.Message; } }

        /// <summary>Gets the database that this complex type belongs to.</summary>
        /// <value>The database that this complex type belongs to.</value>
        public Database Database { get; private set; }

        /// <summary>Gets the collection of database columns that belong to this complex type.</summary>
        /// <value>Collection of database columns.</value>
        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.ComplexTypeTableColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTableColumn, ComplexTypeTableColumn> complexTypeTableColumns;
        /// <summary>Gets the columns of the complex type.</summary>
        /// <value>The columns of the complex type.</value>
        public IEnumerable<ComplexTypeTableColumn> ComplexTypeTableColumns
        {
            get
            {
                if (this.complexTypeTable.ComplexTypeTableColumns.IsNullOrEmpty())
                    yield break;

                if (this.complexTypeTableColumns == null)
                    this.complexTypeTableColumns = new CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTableColumn, ComplexTypeTableColumn>(this.complexTypeTable.ComplexTypeTableColumns, c => new ComplexTypeTableColumn(c, this));

                foreach (var complexTypeTableColumn in this.complexTypeTableColumns)
                {
                    yield return complexTypeTableColumn;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITable, Table> tables;
        /// <summary>Gets the tables of the complex type.</summary>
        /// <value>The tables of the complex type.</value>
        public IEnumerable<Table> Tables
        {
            get
            {
                if (this.complexTypeTable.Tables.IsNullOrEmpty())
                    yield break;

                if (this.tables == null)
                {
                    this.tables = new CachedEnumerable<POCOGenerator.DbObjects.ITable, Table>(
                        this.complexTypeTable.Tables,
                        t1 => this.Database.Tables.First(t2 => t2.InternalEquals(t1))
                    );
                }

                foreach (var table in this.tables)
                {
                    yield return table;
                }
            }
        }

        /// <summary>Gets the name of the complex type.</summary>
        /// <value>The name of the complex type.</value>
        public string Name { get { return this.complexTypeTable.Name; } }

        /// <summary>Gets the schema of the complex type.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the complex type.</value>
        /// <seealso cref="Support.SupportSchema" />
        public string Schema
        {
            get
            {
                if (this.complexTypeTable is POCOGenerator.DbObjects.ISchema schema)
                    return schema.Schema;
                return null;
            }
        }

        /// <summary>Gets the description of the complex type.</summary>
        /// <value>Returns <see langword="null" />.</value>
        public string Description { get { return null; } }

        /// <summary>Returns a string that represents this complex type.</summary>
        /// <returns>A string that represents this complex type.</returns>
        public override string ToString()
        {
            return this.complexTypeTable.ToString();
        }
    }
}
