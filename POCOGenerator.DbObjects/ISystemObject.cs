using System;

namespace POCOGenerator.DbObjects
{
    public interface ISystemObject : IDbObject
    {
        string Name { get; }
        string Type { get; }
    }
}
