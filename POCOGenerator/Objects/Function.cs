using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database table-valued function.</summary>
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

        /// <summary>Gets the error message that occurred during the generating process of this function.</summary>
        /// <value>The error message that occurred during the generating process of this function.</value>
        public string Error { get { return (this.function.Error != null ? this.function.Error.Message : null); } }

        /// <summary>Gets the database that this function belongs to.</summary>
        /// <value>The database that this function belongs to.</value>
        public Database Database { get; private set; }

        /// <summary>Gets the collection of database parameters that belong to this function.</summary>
        /// <value>Collection of database parameters.</value>
        public IEnumerable<IDbParameter> Parameters
        {
            get
            {
                return this.FunctionParameters;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureParameter, FunctionParameter> functionParameters;
        /// <summary>Gets the parameters of the function.</summary>
        /// <value>The parameters of the function.</value>
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

        /// <summary>Gets the collection of database columns that belong to this function.</summary>
        /// <value>Collection of database columns.</value>
        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.FunctionColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureColumn, FunctionColumn> functionColumns;
        /// <summary>Gets the columns of the function.</summary>
        /// <value>The columns of the function.</value>
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

        /// <summary>Gets the name of the function.</summary>
        /// <value>The name of the function.</value>
        public string Name { get { return this.function.Name; } }

        /// <summary>Gets the schema of the function.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the function.</value>
        /// <seealso cref="Support.SupportSchema" />
        public string Schema
        {
            get
            {
                if (this.function is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.function).Schema;
                return null;
            }
        }

        /// <summary>Gets the description of the function.</summary>
        /// <value>The description of the function.</value>
        public string Description { get { return this.function.Description; } }

        /// <summary>Returns a string that represents this function.</summary>
        /// <returns>A string that represents this function.</returns>
        public override string ToString()
        {
            return this.function.ToString();
        }
    }
}
