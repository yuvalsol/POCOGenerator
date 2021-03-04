using System;

namespace POCOGenerator.Objects
{
    public sealed class ProcedureColumn : IDbColumn
    {
        private readonly POCOGenerator.DbObjects.IProcedureColumn procedureColumn;

        internal ProcedureColumn(POCOGenerator.DbObjects.IProcedureColumn procedureColumn, Procedure procedure)
        {
            this.procedureColumn = procedureColumn;
            this.Procedure = procedure;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IProcedureColumn procedureColumn)
        {
            return this.procedureColumn == procedureColumn;
        }

        public Procedure Procedure { get; private set; }

        public IDbObject DbObject { get { return this.Procedure; } }

        public string ColumnName { get { return this.procedureColumn.ColumnName; } }
        public int? ColumnOrdinal { get { return this.procedureColumn.ColumnOrdinal; } }
        public string DataTypeName { get { return this.procedureColumn.DataTypeName; } }
        public string DataTypeDisplay { get { return this.procedureColumn.DataTypeDisplay; } }
        public string Precision { get { return this.procedureColumn.Precision; } }
        public int? StringPrecision { get { return this.procedureColumn.StringPrecision; } }
        public int? NumericPrecision { get { return this.procedureColumn.NumericPrecision; } }
        public int? NumericScale { get { return this.procedureColumn.NumericScale; } }
        public int? DateTimePrecision { get { return this.procedureColumn.DateTimePrecision; } }
        public bool IsUnsigned { get { return this.procedureColumn.IsUnsigned; } }
        public bool IsNullable { get { return this.procedureColumn.IsNullable; } }
        public bool IsIdentity { get { return this.procedureColumn.IsIdentity; } }
        public bool IsComputed { get { return this.procedureColumn.IsComputed; } }

        public override string ToString()
        {
            return this.procedureColumn.ToString();
        }
    }
}
