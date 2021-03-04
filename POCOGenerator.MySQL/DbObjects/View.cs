using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class View : Table, IView
    {
        public override DbObjectType DbObjectType { get { return DbObjectType.View; } }
    }
}
