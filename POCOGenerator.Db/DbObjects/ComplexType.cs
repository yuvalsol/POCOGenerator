using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class ComplexType : IComplexType
    {
        public List<IComplexTypeTable> ComplexTypeTables { get; set; }
    }

    internal class ComplexTypeTable : IComplexTypeTable
    {
        public ITable SourceTable { get; internal set; }
        public string PropertyName { get; internal set; }

        #region IDbObjectTraverse

        public string Name { get; internal set; }
        public IEnumerable<IColumn> Columns { get; internal set; }
        public DbObjectType DbObjectType { get; internal set; }
        public IDatabase Database { get; set; }
        public Exception Error { get; set; }
        public string ClassName { get; set; }

        #endregion

        #region ITable

        public List<ITableColumn> TableColumns { get; set; }
        public IPrimaryKey PrimaryKey { get; set; }
        public List<IUniqueKey> UniqueKeys { get; set; }
        public List<IForeignKey> ForeignKeys { get; set; }
        public List<IForeignKey> PrimaryForeignKeys { get; set; }
        public List<IIndex> Indexes { get; set; }
        public bool IsJoinTable { get; set; }

        #endregion

        #region IDescription

        public string Description { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return this.SourceTable.ToString().Replace(this.SourceTable.Name, this.Name);
        }

        #endregion
    }

    internal sealed class ComplexTypeTableWithSchema : ComplexTypeTable, ISchema
    {
        #region ISchema

        public string Schema { get; internal set; }

        #endregion
    }

    internal sealed class ComplexTypeTableColumn : IComplexTypeTableColumn
    {
        public ITableColumn SourceTableColumn { get; internal set; }

        #region IColumn

        public string ColumnName { get; internal set; }
        public int? ColumnOrdinal { get; internal set; }
        public string DataTypeName { get; internal set; }
        public string DataTypeDisplay { get; internal set; }
        public string Precision { get; internal set; }
        public int? StringPrecision { get; internal set; }
        public int? NumericPrecision { get; internal set; }
        public int? NumericScale { get; internal set; }
        public int? DateTimePrecision { get; internal set; }
        public bool IsUnsigned { get; internal set; }
        public bool IsNullable { get; internal set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }

        #endregion

        #region ITableColumn

        public ITable Table { get; set; }
        public IPrimaryKeyColumn PrimaryKeyColumn { get; set; }
        public List<IUniqueKeyColumn> UniqueKeyColumns { get; set; }
        public List<IForeignKeyColumn> ForeignKeyColumns { get; set; }
        public List<IForeignKeyColumn> PrimaryForeignKeyColumns { get; set; }
        public List<IIndexColumn> IndexColumns { get; set; }
        public string ColumnDefault { get; internal set; }

        public string ToFullString()
        {
            return this.SourceTableColumn.ToFullString().Replace(this.SourceTableColumn.ColumnName, this.ColumnName);
        }

        #endregion

        #region IDescription

        public string Description { get; set; }

        #endregion

        #region IDbObject

        public override string ToString()
        {
            return this.SourceTableColumn.ToString().Replace(this.SourceTableColumn.ColumnName, this.ColumnName);
        }

        #endregion
    }
}
