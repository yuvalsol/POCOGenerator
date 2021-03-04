using System;

namespace POCOGenerator
{
    public static class Disclaimer
    {
        public static readonly string Message = string.Join(Environment.NewLine,
            "Several people reported data loss after using this utility.",
            "Some tables were cleared of all their records but they were able to restore them from backup.",
            "This bug is NOT resolved despite my efforts to replicate and solve it.",
            "Backup your databases before using this utility or use it at your own peril."
        );
    }
}
