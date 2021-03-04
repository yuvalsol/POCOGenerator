using System;

namespace POCOGenerator
{
    [Flags]
    public enum GeneratorResults
    {
        None = 0,
        Error = (1 << 0),
        Warning = (1 << 1),

        NoDbObjectsIncluded = (1 << 2) | Warning,

        ConnectionStringMissing = (1 << 9) | Error,
        ConnectionStringMalformed = (1 << 10) | Error,
        ConnectionStringNotMatchAnyRDBMS = (1 << 11) | Error,
        ConnectionStringMatchMoreThanOneRDBMS = (1 << 12) | Error,
        ServerNotResponding = (1 << 13) | Error,

        UnexpectedError = (1 << 30) | Error
    }
}
