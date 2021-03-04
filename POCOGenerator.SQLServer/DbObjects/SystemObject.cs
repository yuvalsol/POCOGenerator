using System;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class SystemObject : SystemObjectBase
    {
    }

    internal class SystemObjectSchema : SystemObject, ISchema
    {
        public string Schema { get; set; }
    }
}
