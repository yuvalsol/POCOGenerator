using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database stored procedure.</summary>
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

        /// <summary>Gets the error message that occurred during the generating process of this stored procedure.</summary>
        /// <value>The error message that occurred during the generating process of this stored procedure.</value>
        public string Error { get { return (this.procedure.Error != null ? this.procedure.Error.Message : null); } }

        /// <summary>Gets the database that this stored procedure belongs to.</summary>
        /// <value>The database that this stored procedure belongs to.</value>
        public Database Database { get; private set; }

        /// <summary>Gets the collection of database parameters that belong to this stored procedure.</summary>
        /// <value>Collection of database parameters.</value>
        public IEnumerable<IDbParameter> Parameters
        {
            get
            {
                return this.ProcedureParameters;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureParameter, ProcedureParameter> procedureParameters;
        /// <summary>Gets the parameters of the stored procedure.</summary>
        /// <value>The parameters of the stored procedure.</value>
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

        /// <summary>Gets the collection of database columns that belong to this stored procedure.</summary>
        /// <value>Collection of database columns.</value>
        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.ProcedureColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedureColumn, ProcedureColumn> procedureColumns;
        /// <summary>Gets the columns of the stored procedure.</summary>
        /// <value>The columns of the stored procedure.</value>
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

        /// <summary>Gets the name of the stored procedure.</summary>
        /// <value>The name of the stored procedure.</value>
        public string Name { get { return this.procedure.Name; } }

        /// <summary>Gets the schema of the stored procedure.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the stored procedure.</value>
        /// <seealso cref="Support.SupportSchema" />
        public string Schema
        {
            get
            {
                if (this.procedure is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.procedure).Schema;
                return null;
            }
        }

        /// <summary>Gets the description of the stored procedure.</summary>
        /// <value>The description of the stored procedure.</value>
        public string Description { get { return this.procedure.Description; } }

        /// <summary>Returns a string that represents this stored procedure.</summary>
        /// <returns>A string that represents this stored procedure.</returns>
        public override string ToString()
        {
            return this.procedure.ToString();
        }
    }
}
