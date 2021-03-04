using System;

namespace POCOGenerator.DbObjects
{
    public interface IDbObjectDescription
    {
        string Name { get; }
        string Minor_Name { get; }
        string Description { get; }
        DbObjectType ObjectType { get; }
    }
}
