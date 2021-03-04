using System;
using POCOGenerator.Db.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class SQLServer : ServerBase
    {
        public SQLServer()
        {
            this.RDBMSName = "SQL Server";
        }
    }
}
