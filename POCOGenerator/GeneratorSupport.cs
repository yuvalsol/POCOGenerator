using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator
{
    /// <summary>Provides information about the RDBMS and what capabilities it supports.</summary>
    public interface Support
    {
        /// <summary>Gets a value indicating whether the RDBMS supports schema.</summary>
        /// <value>
        ///   <c>true</c> if the RDBMS supports schema; otherwise, <c>false</c>.</value>
        /// <seealso cref="DefaultSchema" />
        bool SupportSchema { get; }

        /// <summary>Gets a value indicating whether the RDBMS supports table functions.
        /// <para>Table function returns a table as result. Scalar function returns a single scalar value.</para></summary>
        /// <value>
        ///   <c>true</c> if the RDBMS supports table functions; otherwise, <c>false</c>.</value>
        bool SupportTableFunctions { get; }

        /// <summary>Gets a value indicating whether the RDBMS supports user-defined table types.</summary>
        /// <value>
        ///   <c>true</c> if the RDBMS supports user-defined table types; otherwise, <c>false</c>.</value>
        bool SupportTVPs { get; }

        /// <summary>Gets a value indicating whether the RDBMS supports enum &amp; set data types.</summary>
        /// <value>
        ///   <c>true</c> if the RDBMS supports enum &amp; set data types; otherwise, <c>false</c>.</value>
        bool SupportEnumDataType { get; }

        /// <summary>Gets the default schema.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The default schema.</value>
        /// <seealso cref="SupportSchema" />
        string DefaultSchema { get; }

        /// <summary>Gets the version of the server.</summary>
        /// <value>The version of the server.</value>
        string Version { get; }
    }

    internal sealed class GeneratorSupport : Support
    {
        private readonly IDbSupport support;

        public GeneratorSupport(IDbSupport support)
        {
            this.support = support;
        }

        public bool SupportSchema { get { return support.IsSupportSchema; } }
        public bool SupportTableFunctions { get { return support.IsSupportTableFunctions; } }
        public bool SupportTVPs { get { return support.IsSupportTVPs; } }
        public bool SupportEnumDataType { get { return support.IsSupportEnumDataType; } }
        public string DefaultSchema { get { return support.DefaultSchema; } }
        public string Version { get { return support.Version.ToString(); } }
    }
}
