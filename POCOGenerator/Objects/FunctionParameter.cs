using System;
using System.Data;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database table-valued function parameter.</summary>
    public sealed class FunctionParameter : IDbParameter
    {
        private readonly POCOGenerator.DbObjects.IProcedureParameter procedureParameter;

        internal FunctionParameter(POCOGenerator.DbObjects.IProcedureParameter procedureParameter, Function function)
        {
            this.procedureParameter = procedureParameter;
            this.Function = function;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IProcedureParameter procedureParameter)
        {
            return this.procedureParameter == procedureParameter;
        }

        /// <summary>Gets the function that this function parameter belongs to.</summary>
        /// <value>The function that this function parameter belongs to.</value>
        public Function Function { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }

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
