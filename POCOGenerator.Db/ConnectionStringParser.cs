using System;
using System.Text.RegularExpressions;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db
{
    public abstract class ConnectionStringParser : IConnectionStringParser
    {
        public abstract void Parse(string connectionString, ref string serverName, ref string initialDatabase, ref string userId, ref bool integratedSecurity);

        public virtual bool Validate(string connectionString)
        {
            try
            {
                string serverName = null;
                string initialDatabase = null;
                string userId = null;
                bool integratedSecurity = false;
                this.Parse(connectionString, ref serverName, ref initialDatabase, ref userId, ref integratedSecurity);
                return string.IsNullOrEmpty(serverName) == false;
            }
            catch
            {
                return false;
            }
        }

        public abstract bool Ping(string connectionString);

        public virtual string Fix(string connectionString)
        {
            return connectionString;
        }

        private static readonly Regex connectionTimeoutRegex = new Regex(@"Connect(?:ion)?\s*Timeout\s*=\s*\d+\s*;?", RegexOptions.Compiled);

        protected static string SetConnectionTimeoutTo120Seconds(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return connectionString;

            Match match = connectionTimeoutRegex.Match(connectionString);
            if (match.Success)
                connectionString = connectionString.Remove(match.Index, match.Length);

            connectionString = connectionString.Trim();

            connectionString +=
                (connectionString.EndsWith(";") ? string.Empty : ";") +
                "Connection Timeout=120;";

            return connectionString;
        }
    }
}
