using System;
using POCOGenerator.Db.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class MySQL : ServerBase
    {
        public MySQL()
        {
            this.RDBMSName = "MySQL";
        }
    }
}
