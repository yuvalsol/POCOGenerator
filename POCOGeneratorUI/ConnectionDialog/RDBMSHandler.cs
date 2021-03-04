using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using POCOGenerator;

namespace POCOGeneratorUI.ConnectionDialog
{
    internal interface IRDBMSHandler
    {
        RDBMS RDBMS { get; }
        DbConnection GetConnection(string connectionString = null);
        DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null);
        void SetConnectionStringBuilder(DbConnectionStringBuilder builder, string server, bool integratedSecurity, string userID, string password, string database);
        bool TestConnection(string connectionString);
        string[] GetDatabases(string connectionString);

        string GetServer(DbConnectionStringBuilder builder);
        bool GetIntegratedSecurity(DbConnectionStringBuilder builder);
        string GetUserID(DbConnectionStringBuilder builder);
        string GetPassword(DbConnectionStringBuilder builder);
        string GetDatabase(DbConnectionStringBuilder builder);
        int GetConnectionTimeout(DbConnectionStringBuilder builder);
        void SetServer(DbConnectionStringBuilder builder, string server);
        void SetIntegratedSecurity(DbConnectionStringBuilder builder, bool integratedSecurity);
        void SetUserID(DbConnectionStringBuilder builder, string userID);
        void SetPassword(DbConnectionStringBuilder builder, string password);
        void SetDatabase(DbConnectionStringBuilder builder, string database);
        void SetConnectionTimeout(DbConnectionStringBuilder builder, int? connectionTimeout);
    }

    internal static class RDBMSHandlerFactory
    {
        private static readonly Dictionary<RDBMS, IRDBMSHandler> handlers = new Dictionary<RDBMS, IRDBMSHandler>();

        static RDBMSHandlerFactory()
        {
            var sqlServerHandler = new SQLServerHandler();
            handlers.Add(sqlServerHandler.RDBMS, sqlServerHandler);

            var mySQLHandler = new MySQLHandler();
            handlers.Add(mySQLHandler.RDBMS, mySQLHandler);
        }

        public static IRDBMSHandler GetRDBMSHandler(RDBMS rdbms)
        {
            if (handlers.ContainsKey(rdbms))
                return handlers[rdbms];

            throw new KeyNotFoundException("RDBMS not found");
        }

        public static IEnumerable<IRDBMSHandler> GetRDBMSHandlers()
        {
            foreach (var handler in handlers)
            {
                yield return handler.Value;
            }
        }

        private abstract class RDBMSHandler<TDbConnection, TConnectionStringBuilder>
            where TDbConnection : DbConnection, new()
            where TConnectionStringBuilder : DbConnectionStringBuilder, new()
        {
            public RDBMSHandler(RDBMS rdbms)
            {
                RDBMS = rdbms;
            }

            public virtual RDBMS RDBMS { get; private set; }

            public virtual DbConnection GetConnection(string connectionString = null)
            {
                DbConnection connection = new TDbConnection();
                if (string.IsNullOrEmpty(connectionString) == false)
                    connection.ConnectionString = connectionString;
                return connection;
            }

            public virtual DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null)
            {
                TConnectionStringBuilder builder = new TConnectionStringBuilder();
                if (string.IsNullOrEmpty(connectionString) == false)
                    builder.ConnectionString = connectionString;
                return builder;
            }

            public virtual void SetConnectionStringBuilder(DbConnectionStringBuilder builder, string server, bool integratedSecurity, string userID, string password, string database)
            {
                SetIntegratedSecurity(builder, integratedSecurity);
                SetServer(builder, server);
                SetUserID(builder, userID);
                SetPassword(builder, password);
                SetDatabase(builder, database);
            }

            public abstract string GetServer(DbConnectionStringBuilder builder);
            public abstract bool GetIntegratedSecurity(DbConnectionStringBuilder builder);
            public abstract string GetUserID(DbConnectionStringBuilder builder);
            public abstract string GetPassword(DbConnectionStringBuilder builder);
            public abstract string GetDatabase(DbConnectionStringBuilder builder);
            public abstract int GetConnectionTimeout(DbConnectionStringBuilder builder);
            public abstract void SetServer(DbConnectionStringBuilder builder, string server);
            public abstract void SetIntegratedSecurity(DbConnectionStringBuilder builder, bool integratedSecurity);
            public abstract void SetUserID(DbConnectionStringBuilder builder, string userID);
            public abstract void SetPassword(DbConnectionStringBuilder builder, string password);
            public abstract void SetDatabase(DbConnectionStringBuilder builder, string database);
            public abstract void SetConnectionTimeout(DbConnectionStringBuilder builder, int? connectionTimeout);

            public virtual bool TestConnection(string connectionString)
            {
                using (var connection = GetConnection(connectionString))
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

            public virtual string[] GetDatabases(string connectionString)
            {
                using (var connection = GetConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        return connection.GetSchema("Databases").AsEnumerable().Select(row => row["database_name"] as string).OrderBy(d => d).ToArray();
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        private class SQLServerHandler : RDBMSHandler<SqlConnection, SqlConnectionStringBuilder>, IRDBMSHandler
        {
            public SQLServerHandler() : base(RDBMS.SQLServer)
            {
            }

            public override string ToString()
            {
                return "SQL Server";
            }

            public override string GetServer(DbConnectionStringBuilder builder)
            {
                return ((SqlConnectionStringBuilder)builder).DataSource;
            }

            public override bool GetIntegratedSecurity(DbConnectionStringBuilder builder)
            {
                return ((SqlConnectionStringBuilder)builder).IntegratedSecurity;
            }

            public override string GetUserID(DbConnectionStringBuilder builder)
            {
                return ((SqlConnectionStringBuilder)builder).UserID;
            }

            public override string GetPassword(DbConnectionStringBuilder builder)
            {
                return ((SqlConnectionStringBuilder)builder).Password;
            }

            public override string GetDatabase(DbConnectionStringBuilder builder)
            {
                return ((SqlConnectionStringBuilder)builder).InitialCatalog;
            }

            public override int GetConnectionTimeout(DbConnectionStringBuilder builder)
            {
                return ((SqlConnectionStringBuilder)builder).ConnectTimeout;
            }

            public override void SetServer(DbConnectionStringBuilder builder, string server)
            {
                if (string.IsNullOrEmpty(server))
                {
                    var b = (SqlConnectionStringBuilder)builder;
                    b.Remove("Date Source");
                    if (b.IntegratedSecurity)
                        b.Remove("Integrated Security");
                }
                else
                {
                    ((SqlConnectionStringBuilder)builder).DataSource = server;
                }
            }

            public override void SetIntegratedSecurity(DbConnectionStringBuilder builder, bool integratedSecurity)
            {
                if (integratedSecurity)
                    ((SqlConnectionStringBuilder)builder).IntegratedSecurity = true;
                else
                    ((SqlConnectionStringBuilder)builder).Remove("Integrated Security");
            }

            public override void SetUserID(DbConnectionStringBuilder builder, string userID)
            {
                if (string.IsNullOrEmpty(userID))
                    ((SqlConnectionStringBuilder)builder).Remove("User ID");
                else
                    ((SqlConnectionStringBuilder)builder).UserID = userID;
            }

            public override void SetPassword(DbConnectionStringBuilder builder, string password)
            {
                if (string.IsNullOrEmpty(password))
                    ((SqlConnectionStringBuilder)builder).Remove("Password");
                else
                    ((SqlConnectionStringBuilder)builder).Password = password;
            }

            public override void SetDatabase(DbConnectionStringBuilder builder, string database)
            {
                if (string.IsNullOrEmpty(database))
                    ((SqlConnectionStringBuilder)builder).Remove("Initial Catalog");
                else
                    ((SqlConnectionStringBuilder)builder).InitialCatalog = database;
            }

            public override void SetConnectionTimeout(DbConnectionStringBuilder builder, int? connectionTimeout)
            {
                if (connectionTimeout == null)
                    ((SqlConnectionStringBuilder)builder).Remove("Connect Timeout");
                else
                    ((SqlConnectionStringBuilder)builder).ConnectTimeout = connectionTimeout.Value;
            }
        }

        private class MySQLHandler : RDBMSHandler<MySqlConnection, MySqlConnectionStringBuilder>, IRDBMSHandler
        {
            public MySQLHandler() : base(RDBMS.MySQL)
            {
            }

            public override string ToString()
            {
                return "My SQL";
            }

            public override string GetServer(DbConnectionStringBuilder builder)
            {
                return ((MySqlConnectionStringBuilder)builder).Server;
            }

            public override bool GetIntegratedSecurity(DbConnectionStringBuilder builder)
            {
                return ((MySqlConnectionStringBuilder)builder).IntegratedSecurity;
            }

            public override string GetUserID(DbConnectionStringBuilder builder)
            {
                return ((MySqlConnectionStringBuilder)builder).UserID;
            }

            public override string GetPassword(DbConnectionStringBuilder builder)
            {
                return ((MySqlConnectionStringBuilder)builder).Password;
            }

            public override string GetDatabase(DbConnectionStringBuilder builder)
            {
                return ((MySqlConnectionStringBuilder)builder).Database;
            }

            public override int GetConnectionTimeout(DbConnectionStringBuilder builder)
            {
                return (int)((MySqlConnectionStringBuilder)builder).ConnectionTimeout;
            }

            public override void SetServer(DbConnectionStringBuilder builder, string server)
            {
                if (string.IsNullOrEmpty(server))
                {
                    var b = (MySqlConnectionStringBuilder)builder;
                    b.Remove("server");
                    if (b.IntegratedSecurity)
                        b.Remove("Integrated Security");
                }
                else
                {
                    ((MySqlConnectionStringBuilder)builder).Server = server;
                }
            }

            public override void SetIntegratedSecurity(DbConnectionStringBuilder builder, bool integratedSecurity)
            {
                if (integratedSecurity)
                    ((MySqlConnectionStringBuilder)builder).IntegratedSecurity = true;
                else
                    ((MySqlConnectionStringBuilder)builder).Remove("Integrated Security");
            }

            public override void SetUserID(DbConnectionStringBuilder builder, string userID)
            {
                if (string.IsNullOrEmpty(userID))
                    ((MySqlConnectionStringBuilder)builder).Remove("user id");
                else
                    ((MySqlConnectionStringBuilder)builder).UserID = userID;
            }

            public override void SetPassword(DbConnectionStringBuilder builder, string password)
            {
                if (string.IsNullOrEmpty(password))
                    ((MySqlConnectionStringBuilder)builder).Remove("password");
                else
                    ((MySqlConnectionStringBuilder)builder).Password = password;
            }

            public override void SetDatabase(DbConnectionStringBuilder builder, string database)
            {
                if (string.IsNullOrEmpty(database))
                    ((MySqlConnectionStringBuilder)builder).Remove("database");
                else
                    ((MySqlConnectionStringBuilder)builder).Database = database;
            }

            public override void SetConnectionTimeout(DbConnectionStringBuilder builder, int? connectionTimeout)
            {
                if (connectionTimeout == null)
                    ((MySqlConnectionStringBuilder)builder).Remove("connectiontimeout");
                else
                    ((MySqlConnectionStringBuilder)builder).ConnectionTimeout = (uint)connectionTimeout.Value;
            }
        }
    }
}
