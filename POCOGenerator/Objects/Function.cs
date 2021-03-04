using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    public sealed class Function : IDbRoutine
    {
        private readonly POCOGenerator.DbObjects.IFunction function;

        internal Function(POCOGenerator.DbObjects.IFunction function, Database database)
        {
            this.function = function;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IFunction function)
        {
            return this.function == function;
        }

        internal string ClassName { get { return this.function.ClassName; } }
        public string Error { get { return (this.function.Error != null ? this.function.Error.Message : null); } }

        public Database Database { get; private set; }

        public IEnumerable<IDbParameter> Parameters
        {
            get
            {
                return this.FunctionParameters;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureParameter, FunctionParameter> functionParameters;
        public IEnumerable<FunctionParameter> FunctionParameters
        {
            get
            {
                if (this.function.ProcedureParameters.IsNullOrEmpty())
                    yield break;

                if (this.functionParameters == null)
                    this.functionParameters = new CachedEnumerable<POCOGenerator.DbObjects.IProcedureParameter, FunctionParameter>(this.function.ProcedureParameters, pp => new FunctionParameter(pp, this));

                foreach (var functionParameter in this.functionParameters)
                {
                    yield return functionParameter;
                }
            }
        }

        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.FunctionColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureColumn, FunctionColumn> functionColumns;
        public IEnumerable<FunctionColumn> FunctionColumns
        {
            get
            {
                if (this.function.ProcedureColumns.IsNullOrEmpty())
                    yield break;

                if (this.functionColumns == null)
                    this.functionColumns = new CachedEnumerable<POCOGenerator.DbObjects.IProcedureColumn, FunctionColumn>(this.function.ProcedureColumns, pc => new FunctionColumn(pc, this));

                foreach (var functionColumn in this.functionColumns)
                {
                    yield return functionColumn;
                }
            }
        }

        public string Name { get { return this.function.Name; } }

        public string Schema
        {
            get
            {
                if (this.function is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.function).Schema;
                return null;
            }
        }

        public string Description { get { return this.function.Description; } }

        public override string ToString()
        {
            return this.function.ToString();
        }
    }
}
