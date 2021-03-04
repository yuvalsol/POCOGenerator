using System;

namespace POCOGenerator.DbObjects
{
    public interface IIdentityColumn : IDbObject
    {
        string Table_Name { get; }
        string Column_Name { get; }
    }
}
