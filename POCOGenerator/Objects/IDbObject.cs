using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database object</summary>
    public interface IDbObject
    {
        /// <summary>Gets the database that this database object belongs to.</summary>
        /// <value>The database that this database object belongs to.</value>
        Database Database { get; }

        /// <summary>Gets the name of the database object.</summary>
        /// <value>The name of the database object.</value>
        string Name { get; }

        /// <summary>Gets the schema of the database object.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the database object.</value>
        /// <seealso cref="Support.SupportSchema" />
        string Schema { get; }

        /// <summary>Gets the description of the database object.</summary>
        /// <value>The description of the database object.</value>
        string Description { get; }

        /// <summary>Gets the collection of database columns that belong to this database object.</summary>
        /// <value>Collection of database columns.</value>
        IEnumerable<IDbColumn> Columns { get; }

        /// <summary>Gets the error message that occurred during the generating process of this database object.</summary>
        /// <value>The error message that occurred during the generating process of this database object.</value>
        string Error { get; }

        /// <summary>Returns a string that represents this database object.</summary>
        /// <returns>A string that represents this database object.</returns>
        string ToString();
    }
}
