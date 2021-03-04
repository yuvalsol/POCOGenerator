using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IEnumColumn
    {
        IColumn Column { get; }
        bool IsEnumDataType { get; }
        bool IsSetDataType { get; }
        List<string> EnumLiterals { get; }
    }
}
