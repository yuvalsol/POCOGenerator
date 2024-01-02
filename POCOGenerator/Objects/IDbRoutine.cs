using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database routine</summary>
    public interface IDbRoutine : IDbObject
    {
        /// <summary>Gets the collection of database parameters that belong to this database routine.</summary>
        /// <value>Collection of database parameters.</value>
        IEnumerable<IDbParameter> Parameters { get; }
    }
}
