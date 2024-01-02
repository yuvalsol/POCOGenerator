using System;

namespace POCOGenerator
{
    /// <summary>Represents a list of result codes that indicate whether the generator finished running successfully or not.</summary>
    [Flags]
    public enum GeneratorResults
    {
        /// <summary>Indicates the generator finished running successfully.</summary>
        None = 0,
        /// <summary>Indicates the generator encountered a fatal error while running.</summary>
        Error = 1,
        /// <summary>Indicates the generator encountered a non-fatal error while running.</summary>
        Warning = (1 << 1),

        /// <summary>Warning. No database objects were included for processing.</summary>
        NoDbObjectsIncluded = (1 << 2) | Warning,

        /// <summary>Error. The connection string is missing.</summary>
        ConnectionStringMissing = (1 << 9) | Error,
        /// <summary>Error. The connection string is malformed.</summary>
        ConnectionStringMalformed = (1 << 10) | Error,
        /// <summary>Error. The connection string is not supported by any RDBMS.</summary>
        ConnectionStringNotMatchAnyRDBMS = (1 << 11) | Error,
        /// <summary>Error. The connection string is supported by more than one RDBMS.</summary>
        ConnectionStringMatchMoreThanOneRDBMS = (1 << 12) | Error,
        /// <summary>Error. The server is not responding. Failed to connect to the server.</summary>
        ServerNotResponding = (1 << 13) | Error,

        /// <summary>Error. The generator encountered an unexpected error.</summary>
        UnexpectedError = (1 << 30) | Error
    }
}
