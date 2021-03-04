using System;
using System.Data;

namespace POCOGenerator.Objects
{
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

        public Procedure Procedure { get; private set; }

        public IDbObject DbObject { get { return this.Procedure; } }

        public string ParameterName { get { return this.procedureParameter.ParameterName; } }
        public string ParameterDataType { get { return this.procedureParameter.ParameterDataType; } }
        public bool ParameterIsUnsigned { get { return this.procedureParameter.ParameterIsUnsigned; } }
        public int? ParameterOrdinal { get { return this.procedureParameter.ParameterOrdinal; } }
        public int? ParameterSize { get { return this.procedureParameter.ParameterSize; } }
        public byte? ParameterPrecision { get { return this.procedureParameter.ParameterPrecision; } }
        public int? ParameterScale { get { return this.procedureParameter.ParameterScale; } }
        public int? ParameterDateTimePrecision { get { return this.procedureParameter.ParameterDateTimePrecision; } }
        public string ParameterMode { get { return this.procedureParameter.ParameterMode; } }
        public ParameterDirection ParameterDirection { get { return this.procedureParameter.ParameterDirection; } }
        public bool IsResult { get { return this.procedureParameter.IsResult; } }

        public string Description { get { return this.procedureParameter.Description; } }

        public override string ToString()
        {
            return this.procedureParameter.ToString();
        }
    }
}
