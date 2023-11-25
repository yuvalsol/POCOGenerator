using System;
using MySql.Data.MySqlClient;
using POCOGenerator.Db;

namespace POCOGenerator.MySQL
{
    internal sealed class MySQLConnectionStringParser : ConnectionStringParser
    {
        private MySQLConnectionStringParser() { }

        public static MySQLConnectionStringParser Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly MySQLConnectionStringParser instance = new MySQLConnectionStringParser();
        }

        public override void Parse(string connectionString, ref string serverName, ref string initialDatabase, ref string userId, ref bool integratedSecurity)
        {
            var conn = new MySqlConnectionStringBuilder(connectionString);
            serverName = conn.Server;
            initialDatabase = conn.Database;
            userId = conn.UserID;
            integratedSecurity = conn.IntegratedSecurity;
        }

        public override bool Ping(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return false;

            connectionString = SetConnectionTimeoutTo120Seconds(connectionString);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public override string Fix(string connectionString)
        {
            if (connectionString.IndexOf("Allow User Variables", StringComparison.OrdinalIgnoreCase) == -1)
            {
                connectionString = connectionString.TrimEnd(';', ' ') + ";Allow User Variables=true";
            }
            else
            {
                string allowUserVariables = "Allow User Variables=false";
                int index = connectionString.IndexOf(allowUserVariables, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                    connectionString = connectionString.Remove(index, allowUserVariables.Length).Insert(index, "Allow User Variables=true");
            }

            return connectionString;
        }
    }
}
