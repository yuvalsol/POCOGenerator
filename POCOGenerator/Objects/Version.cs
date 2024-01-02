using System;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a semantic versioning four-part numbers.</summary>
    public sealed class Version
    {
        private readonly POCOGenerator.DbObjects.Version version;

        internal Version(POCOGenerator.DbObjects.Version version)
        {
            this.version = version;
        }

        /// <summary>Gets the major field of the version.</summary>
        /// <value>The major field of the version.</value>
        public int Major { get { return this.version.Major; } }
        
        /// <summary>Gets the minor field of the version.</summary>
        /// <value>The minor field of the version.</value>
        public int Minor { get { return this.version.Minor; } }
        
        /// <summary>Gets the build field of the version.</summary>
        /// <value>The build field of the version.</value>
        public int Build { get { return this.version.Build; } }
        
        /// <summary>Gets the revision field of the version.</summary>
        /// <value>The revision field of the version.</value>
        public int Revision { get { return this.version.Revision; } }

        /// <summary>Returns the version.
        /// <para>The format of the returned string is Major.Minor.Build.Revision.</para></summary>
        /// <returns>The version.</returns>
        public override string ToString()
        {
            return this.version.ToString();
        }

        /// <summary>Returns the version.</summary>
        /// <param name="fieldCount">A number indicating how many fields the returned version includes.</param>
        /// <returns>The version.</returns>
        public string ToString(int fieldCount)
        {
            return this.version.ToString(fieldCount);
        }
    }
}
