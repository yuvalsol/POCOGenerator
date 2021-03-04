using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class View : Table, IView
    {
        public override DbObjectType DbObjectType { get { return DbObjectType.View; } }
    }
}
