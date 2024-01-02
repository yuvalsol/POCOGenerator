using System;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database table-valued function column.</summary>
    public sealed class FunctionColumn : IDbColumn
    {
        private readonly POCOGenerator.DbObjects.IProcedureColumn procedureColumn;

        internal FunctionColumn(POCOGenerator.DbObjects.IProcedureColumn procedureColumn, Function function)
        {
            this.procedureColumn = procedureColumn;
            this.Function = function;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IProcedureColumn procedureColumn)
        {
            return this.procedureColumn == procedureColumn;
        }

        /// <summary>Gets the function that this function column belongs to.</summary>
        /// <value>The function that this function column belongs to.</value>
        public Function Function { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }

        /// <inheritdoc />
        public string ColumnName { get { return this.procedureColumn.ColumnName; } }
        /// <inheritdoc />
        public int? ColumnOrdinal { get { return this.procedureColumn.ColumnOrdinal; } }
        /// <inheritdoc />
        public string DataTypeName { get { return this.procedureColumn.DataTypeName; } }
        /// <inheritdoc />
        public string DataTypeDisplay { get { return this.procedureColumn.DataTypeDisplay; } }
        /// <inheritdoc />
        public string Precision { get { return this.procedureColumn.Precision; } }
        /// <inheritdoc />
        public int? StringPrecision { get { return this.procedureColumn.StringPrecision; } }
        /// <inheritdoc />
        public int? NumericPrecision { get { return this.procedureColumn.NumericPrecision; } }
        /// <inheritdoc />
        public int? NumericScale { get { return this.procedureColumn.NumericScale; } }
        /// <inheritdoc />
        public int? DateTimePrecision { get { return this.procedureColumn.DateTimePrecision; } }
        /// <inheritdoc />
        public bool IsUnsigned { get { return this.procedureColumn.IsUnsigned; } }
        /// <inheritdoc />
        public bool IsNullable { get { return this.procedureColumn.IsNullable; } }
        /// <inheritdoc />
        public bool IsIdentity { get { return this.procedureColumn.IsIdentity; } }
        /// <inheritdoc />
        public bool IsComputed { get { return this.procedureColumn.IsComputed; } }

        /// <inheritdoc cref="IDbColumn.ToString" />
        public override string ToString()
        {
            return this.procedureColumn.ToString();
        }
    }
}
