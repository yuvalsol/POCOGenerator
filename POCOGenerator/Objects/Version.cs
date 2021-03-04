using System;

namespace POCOGenerator.Objects
{
    public sealed class Version
    {
        private readonly POCOGenerator.DbObjects.Version version;

        internal Version(POCOGenerator.DbObjects.Version version)
        {
            this.version = version;
        }

        public int Major { get { return this.version.Major; } }
        public int Minor { get { return this.version.Minor; } }
        public int Build { get { return this.version.Build; } }
        public int Revision { get { return this.version.Revision; } }

        public override string ToString()
        {
            return this.version.ToString();
        }

        public string ToString(int fieldCount)
        {
            return this.version.ToString(fieldCount);
        }
    }
}
