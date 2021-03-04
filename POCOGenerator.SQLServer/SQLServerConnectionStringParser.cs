using System;
using System.Data.SqlClient;
using POCOGenerator.Db;

namespace POCOGenerator.SQLServer
{
    internal sealed class SQLServerConnectionStringParser : ConnectionStringParser
    {
        private SQLServerConnectionStringParser() { }

        public static SQLServerConnectionStringParser Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly SQLServerConnectionStringParser instance = new SQLServerConnectionStringParser();
        }

        public override void Parse(string connectionString, ref string serverName, ref string initialDatabase, ref string userId, ref bool integratedSecurity)
        {
            var conn = new SqlConnectionStringBuilder(connectionString);
            serverName = conn.DataSource;
            initialDatabase = conn.InitialCatalog;
            userId = conn.UserID;
            integratedSecurity = conn.IntegratedSecurity;
        }

        public override bool Ping(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return false;

            connectionString = SetConnectionTimeoutTo120Seconds(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
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
    }
}
