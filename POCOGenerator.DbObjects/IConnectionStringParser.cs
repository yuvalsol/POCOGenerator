using System;

namespace POCOGenerator.DbObjects
{
    public interface IConnectionStringParser
    {
        void Parse(string connectionString, ref string serverName, ref string initialDatabase, ref string userId, ref bool integratedSecurity);
        bool Validate(string connectionString);
        bool Ping(string connectionString);
        string Fix(string connectionString);
    }
}
