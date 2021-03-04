using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator
{
    public interface Support
    {
        bool SupportSchema { get; }
        bool SupportTableFunctions { get; }
        bool SupportTVPs { get; }
        bool SupportEnumDataType { get; }
        string DefaultSchema { get; }
        string Version { get; }
    }

    internal sealed class GeneratorSupport : Support
    {
        private IDbSupport support;

        public GeneratorSupport(IDbSupport support)
        {
            this.support = support;
        }

        public bool SupportSchema { get { return support.IsSupportSchema; } }
        public bool SupportTableFunctions { get { return support.IsSupportTableFunctions; } }
        public bool SupportTVPs { get { return support.IsSupportTVPs; } }
        public bool SupportEnumDataType { get { return support.IsSupportEnumDataType; } }
        public string DefaultSchema { get { return support.DefaultSchema; } }
        public string Version { get { return support.Version.ToString(); } }
    }
}
