using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.SQLServer.DbObjects
{
    internal class Function : Procedure, IFunction
    {
        public override DbObjectType DbObjectType { get { return DbObjectType.Function; } }
    }
}
