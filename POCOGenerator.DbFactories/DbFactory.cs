using System;
using POCOGenerator.DbHandlers;

namespace POCOGenerator.DbFactories
{
    public sealed class DbFactory
    {
        private DbFactory() { }

        public static DbFactory Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly DbFactory instance = new DbFactory();
        }

        public IDbHandler SQLServerHandler
        {
            get
            {
                return SQLServer.SQLServerHandler.Instance;
            }
        }

        public IDbHandler MySQLHandler
        {
            get
            {
                return MySQL.MySQLHandler.Instance;
            }
        }
    }
}
