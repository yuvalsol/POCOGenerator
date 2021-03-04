using System;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class ComputedColumn : ComputedColumnBase
    {
    }

    internal class ComputedColumnSchema : ComputedColumn, ISchema
    {
        public string Schema { get; set; }
    }
}
