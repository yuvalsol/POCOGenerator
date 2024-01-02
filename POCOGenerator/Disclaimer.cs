using System;

namespace POCOGenerator
{
    /// <summary>Provides a disclaimer message about POCO Generator.</summary>
    public static class Disclaimer
    {
        /// <summary>The disclaimer message about POCO Generator.</summary>
        public static readonly string Message =
            @"One person reported data loss after using this utility (Comments section in the original article on CodeProject).
Some tables were cleared of all their records but they were able to restore them from backup.
This error is NOT resolved despite my efforts to replicate and solve it.
Backup your database before using this utility or use it at your own risk.";
    }
}
