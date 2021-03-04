using System;
using POCOGenerator.Db.DbObjects;

namespace POCOGenerator.MySQL
{
    public class MySQLSupport : DbSupportBase
    {
        internal MySQLSupport()
            : base()
        {
            IsSupportSchema = false;
            IsSupportTableFunctions = false;
            IsSupportTVPs = false;
            IsSupportEnumDataType = true;
        }
    }
}
