using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    public interface IDbRoutine : IDbObject
    {
        IEnumerable<IDbParameter> Parameters { get; }
    }
}
