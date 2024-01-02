using System;

namespace POCOGenerator
{
    /// <summary>Represents a list of supported relational database management systems.</summary>
    public enum RDBMS
    {
        /// <summary>Specifies no RDBMS.</summary>
        None = 0,
        /// <summary>Specifies the SQL Server RDBMS.</summary>
        SQLServer = 1,
        /// <summary>Specifies the MySQL RDBMS.</summary>
        MySQL = 2
    }
}
