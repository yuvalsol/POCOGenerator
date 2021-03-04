using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    public sealed class Procedure : IDbRoutine
    {
        private readonly POCOGenerator.DbObjects.IProcedure procedure;

        internal Procedure(POCOGenerator.DbObjects.IProcedure procedure, Database database)
        {
            this.procedure = procedure;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IProcedure procedure)
        {
            return this.procedure == procedure;
        }

        internal string ClassName { get { return this.procedure.ClassName; } }
        public string Error { get { return (this.procedure.Error != null ? this.procedure.Error.Message : null); } }

        public Database Database { get; private set; }

        public IEnumerable<IDbParameter> Parameters
        {
            get
            {
                return this.ProcedureParameters;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureParameter, ProcedureParameter> procedureParameters;
        public IEnumerable<ProcedureParameter> ProcedureParameters
        {
            get
            {
                if (this.procedure.ProcedureParameters.IsNullOrEmpty())
                    yield break;

                if (this.procedureParameters == null)
                    this.procedureParameters = new CachedEnumerable<POCOGenerator.DbObjects.IProcedureParameter, ProcedureParameter>(this.procedure.ProcedureParameters, pp => new ProcedureParameter(pp, this));

                foreach (var procedureParameter in this.procedureParameters)
                {
                    yield return procedureParameter;
                }
            }
        }

        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.ProcedureColumns;
            }
        }
        
        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureColumn, ProcedureColumn> procedureColumns;
        public IEnumerable<ProcedureColumn> ProcedureColumns
        {
            get
            {
                if (this.procedure.ProcedureColumns.IsNullOrEmpty())
                    yield break;

                if (this.procedureColumns == null)
                    this.procedureColumns = new CachedEnumerable<POCOGenerator.DbObjects.IProcedureColumn, ProcedureColumn>(this.procedure.ProcedureColumns, pc => new ProcedureColumn(pc, this));

                foreach (var procedureColumn in this.procedureColumns)
                {
                    yield return procedureColumn;
                }
            }
        }

        public string Name { get { return this.procedure.Name; } }

        public string Schema
        {
            get
            {
                if (this.procedure is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.procedure).Schema;
                return null;
            }
        }

        public string Description { get { return this.procedure.Description; } }

        public override string ToString()
        {
            return this.procedure.ToString();
        }
    }
}
