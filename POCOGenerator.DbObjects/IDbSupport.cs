using System;

namespace POCOGenerator.DbObjects
{
    public interface IDbSupport
    {
        bool this[string key] { get; set; }
        bool IsSupportSchema { get; set; }
        bool IsSupportTableFunctions { get; set; }
        bool IsSupportTVPs { get; set; }
        bool IsSupportEnumDataType { get; set; }
        string DefaultSchema { get; set; }
        Version Version { get; set; }
    }
}
