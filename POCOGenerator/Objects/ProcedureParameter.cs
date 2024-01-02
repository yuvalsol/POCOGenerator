using System;
using System.Data;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database stored procedure parameter.</summary>
    public sealed class ProcedureParameter : IDbParameter
    {
        private readonly POCOGenerator.DbObjects.IProcedureParameter procedureParameter;

        internal ProcedureParameter(POCOGenerator.DbObjects.IProcedureParameter procedureParameter, Procedure procedure)
        {
            this.procedureParameter = procedureParameter;
            this.Procedure = procedure;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IProcedureParameter procedureParameter)
        {
            return this.procedureParameter == procedureParameter;
        }

        /// <summary>Gets the stored procedure that this stored procedure parameter belongs to.</summary>
        /// <value>The stored procedure that this stored procedure parameter belongs to.</value>
        public Procedure Procedure { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }

        /// <inheritdoc />
        public string ParameterName { get { return this.procedureParameter.ParameterName; } }
        /// <inheritdoc />
        public string ParameterDataType { get { return this.procedureParameter.ParameterDataType; } }
        /// <inheritdoc />
        public bool ParameterIsUnsigned { get { return this.procedureParameter.ParameterIsUnsigned; } }
        /// <inheritdoc />
        public int? ParameterOrdinal { get { return this.procedureParameter.ParameterOrdinal; } }
        /// <inheritdoc />
        public int? ParameterSize { get { return this.procedureParameter.ParameterSize; } }
        /// <inheritdoc />
        public byte? ParameterPrecision { get { return this.procedureParameter.ParameterPrecision; } }
        /// <inheritdoc />
        public int? ParameterScale { get { return this.procedureParameter.ParameterScale; } }
        /// <inheritdoc />
        public int? ParameterDateTimePrecision { get { return this.procedureParameter.ParameterDateTimePrecision; } }
        /// <inheritdoc />
        public string ParameterMode { get { return this.procedureParameter.ParameterMode; } }
        /// <inheritdoc />
        public ParameterDirection ParameterDirection { get { return this.procedureParameter.ParameterDirection; } }
        /// <inheritdoc />
        public bool IsResult { get { return this.procedureParameter.IsResult; } }

        /// <inheritdoc />
        public string Description { get { return this.procedureParameter.Description; } }

        /// <inheritdoc cref="IDbParameter.ToString" />
        public override string ToString()
        {
            return this.procedureParameter.ToString();
        }
    }
}
