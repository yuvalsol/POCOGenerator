using System;
using POCOGenerator.Db.DbObjects;

namespace POCOGenerator.SQLServer
{
    public class SQLServerSupport : DbSupportBase
    {
        internal SQLServerSupport()
            : base()
        {
            IsSupportSchema = true;
            IsSupportTableFunctions = true;
            IsSupportTVPs = true;
            IsSupportEnumDataType = false;
            DefaultSchema = "dbo";
        }
    }
}
