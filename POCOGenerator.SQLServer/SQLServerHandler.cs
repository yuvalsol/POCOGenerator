using System;
using POCOGenerator.Db;
using POCOGenerator.DbHandlers;
using POCOGenerator.DbObjects;
using POCOGenerator.POCOIterators;
using POCOGenerator.POCOWriters;

namespace POCOGenerator.SQLServer
{
    public sealed class SQLServerHandler : IDbHandler
    {
        private SQLServerHandler() { }

        public static SQLServerHandler Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly SQLServerHandler instance = new SQLServerHandler();
        }

        public IDbHelper GetDbHelper(string connectionString)
        {
            return new SQLServerHelper(connectionString);
        }

        public IConnectionStringParser GetConnectionStringParser()
        {
            return SQLServerConnectionStringParser.Instance;
        }

        public IServer GetServer()
        {
            return new SQLServer.DbObjects.SQLServer();
        }

        public IDbIterator GetIterator(IWriter writer, IDbSupport support, IDbIteratorSettings settings)
        {
            return new SQLServerIterator(writer, support, settings);
        }
    }
}
