using System;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class DbObjectDescription : DbObjectDescriptionBase
    {
    }

    internal class DbObjectDescriptionSchema : DbObjectDescription, ISchema
    {
        public string Schema { get; set; }
    }
}
