using System;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class IdentityColumn : IdentityColumnBase
    {
    }

    internal class IdentityColumnSchema : IdentityColumn, ISchema
    {
        public string Schema { get; set; }
    }
}
