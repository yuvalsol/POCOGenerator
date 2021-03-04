using System;

namespace POCOGenerator.DbObjects
{
    public interface IDbHelper
    {
        IDbSupport Support { get; }

        void BuildServerSchema(
            IServer server,
            string initialDatabase,
            bool isEnableTables,
            bool isEnableViews,
            bool isEnableStoredProcedures,
            bool isEnableFunctions,
            bool isEnableTVPs
        );
    }
}
