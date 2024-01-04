using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace POCOGenerator.DbObjects
{
    public sealed class Version : IComparable, IComparable<Version>, IComparer, IComparer<Version>, IEquatable<Version>, IEqualityComparer<Version>, ICloneable
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Build { get; private set; }
        public int Revision { get; private set; }

        #region Constructors

        private Version() { }

        public Version(int major, int minor = 0, int build = 0, int revision = 0)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
            this.Revision = revision;
        }

        public Version(Version version) : this(version.Major, version.Minor, version.Build, version.Revision) { }

        public Version(string version) : this(Parse(version)) { }

        #endregion

        #region Parse

        private static readonly Regex regexVersion = new Regex(@"(?<Major>\d+)(?:\.(?<Minor>\d+))?(?:\.(?<Build>\d+))?(?:\.(?<Revision>\d+))?", RegexOptions.Compiled);

        public static Version Parse(string version)
        {
            if (version == null)
                throw new ArgumentNullException("versionString");

            Match match = regexVersion.Match(version);
            if (match.Success == false)
                throw new ArgumentException("Version was not found in input string");

            Version result = new Version();

            Group groupMajor = match.Groups["Major"];
            result.Major = int.Parse(groupMajor.Value);

            Group groupMinor = match.Groups["Minor"];
            if (groupMinor.Success)
            {
                result.Minor = int.Parse(groupMinor.Value);

                Group groupBuild = match.Groups["Build"];
                if (groupBuild.Success)
                {
                    result.Build = int.Parse(groupBuild.Value);

                    Group groupRevision = match.Groups["Revision"];
                    if (groupRevision.Success)
                        result.Revision = int.Parse(groupRevision.Value);
                }
            }

            return result;
        }

        public static bool TryParse(string version, out Version result)
        {
            try
            {
                result = Parse(version);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        #endregion

        #region To String

        public override string ToString()
        {
            return ToString(4);
        }

        public string ToString(int fieldCount)
        {
            if (fieldCount < 0 || fieldCount > 4)
                throw new ArgumentException("fieldCount must be between 0 and 4", "fieldCount");

            if (fieldCount == 0)
                return string.Empty;

            string format = (fieldCount == 1 ? "{0}" : (fieldCount == 2 ? "{0}.{1}" : (fieldCount == 3 ? "{0}.{1}.{2}" : "{0}.{1}.{2}.{3}")));

            return string.Format(format,
                this.Major.ToString(CultureInfo.InvariantCulture),
                this.Minor.ToString(CultureInfo.InvariantCulture),
                this.Build.ToString(CultureInfo.InvariantCulture),
                this.Revision.ToString(CultureInfo.InvariantCulture)
            );
        }

        #endregion

        #region Compare To

        public bool Equals(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return CompareVersions(this.Major, this.Minor, this.Build, this.Revision, major, minor, build, revision) == 0;
        }

        public bool NotEqual(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return CompareVersions(this.Major, this.Minor, this.Build, this.Revision, major, minor, build, revision) != 0;
        }

        public bool LessThan(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return CompareVersions(this.Major, this.Minor, this.Build, this.Revision, major, minor, build, revision) < 0;
        }

        public bool LessThanOrEqual(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return CompareVersions(this.Major, this.Minor, this.Build, this.Revision, major, minor, build, revision) <= 0;
        }

        public bool GreaterThan(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return CompareVersions(this.Major, this.Minor, this.Build, this.Revision, major, minor, build, revision) > 0;
        }

        public bool GreaterThanOrEqual(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return CompareVersions(this.Major, this.Minor, this.Build, this.Revision, major, minor, build, revision) >= 0;
        }

        #endregion

        #region Operator Overloading

        public static bool operator ==(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) == 0;
        }

        public static bool operator !=(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) != 0;
        }

        public static bool operator <(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) < 0;
        }

        public static bool operator <=(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) <= 0;
        }

        public static bool operator >(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) > 0;
        }

        public static bool operator >=(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) >= 0;
        }

        public override bool Equals(object obj)
        {
            return CompareVersions(this, obj as Version) == 0;
        }

        public override int GetHashCode()
        {
            int hash = 5381;
            hash = ((hash << 5) + hash) ^ this.Major;
            hash = ((hash << 5) + hash) ^ this.Minor;
            hash = ((hash << 5) + hash) ^ this.Build;
            hash = ((hash << 5) + hash) ^ this.Revision;
            return hash;
        }

        #endregion

        #region Numeric Operator Overloading

        public static bool operator ==(Version v1, int v2Major)
        {
            return CompareVersions(v1, v2Major) == 0;
        }

        public static bool operator !=(Version v1, int v2Major)
        {
            return CompareVersions(v1, v2Major) != 0;
        }

        public static bool operator <(Version v1, int v2Major)
        {
            return CompareVersions(v1, v2Major) < 0;
        }

        public static bool operator <=(Version v1, int v2Major)
        {
            return CompareVersions(v1, v2Major) <= 0;
        }

        public static bool operator >(Version v1, int v2Major)
        {
            return CompareVersions(v1, v2Major) > 0;
        }

        public static bool operator >=(Version v1, int v2Major)
        {
            return CompareVersions(v1, v2Major) >= 0;
        }

        public static bool operator ==(int v1Major, Version v2)
        {
            return CompareVersions(v1Major, v2) == 0;
        }

        public static bool operator !=(int v1Major, Version v2)
        {
            return CompareVersions(v1Major, v2) != 0;
        }

        public static bool operator <(int v1Major, Version v2)
        {
            return CompareVersions(v1Major, v2) < 0;
        }

        public static bool operator <=(int v1Major, Version v2)
        {
            return CompareVersions(v1Major, v2) <= 0;
        }

        public static bool operator >(int v1Major, Version v2)
        {
            return CompareVersions(v1Major, v2) > 0;
        }

        public static bool operator >=(int v1Major, Version v2)
        {
            return CompareVersions(v1Major, v2) >= 0;
        }

        public static bool operator ==(Version v1, double v2MajorMinor)
        {
            return CompareVersions(v1, v2MajorMinor) == 0;
        }

        public static bool operator !=(Version v1, double v2MajorMinor)
        {
            return CompareVersions(v1, v2MajorMinor) != 0;
        }

        public static bool operator <(Version v1, double v2MajorMinor)
        {
            return CompareVersions(v1, v2MajorMinor) < 0;
        }

        public static bool operator <=(Version v1, double v2MajorMinor)
        {
            return CompareVersions(v1, v2MajorMinor) <= 0;
        }

        public static bool operator >(Version v1, double v2MajorMinor)
        {
            return CompareVersions(v1, v2MajorMinor) > 0;
        }

        public static bool operator >=(Version v1, double v2MajorMinor)
        {
            return CompareVersions(v1, v2MajorMinor) >= 0;
        }

        public static bool operator ==(double v1MajorMinor, Version v2)
        {
            return CompareVersions(v1MajorMinor, v2) == 0;
        }

        public static bool operator !=(double v1MajorMinor, Version v2)
        {
            return CompareVersions(v1MajorMinor, v2) != 0;
        }

        public static bool operator <(double v1MajorMinor, Version v2)
        {
            return CompareVersions(v1MajorMinor, v2) < 0;
        }

        public static bool operator <=(double v1MajorMinor, Version v2)
        {
            return CompareVersions(v1MajorMinor, v2) <= 0;
        }

        public static bool operator >(double v1MajorMinor, Version v2)
        {
            return CompareVersions(v1MajorMinor, v2) > 0;
        }

        public static bool operator >=(double v1MajorMinor, Version v2)
        {
            return CompareVersions(v1MajorMinor, v2) >= 0;
        }

        #endregion

        #region IComparable

        public int CompareTo(object obj)
        {
            return CompareVersions(this, obj as Version);
        }

        #endregion

        #region IComparable<Version>

        public int CompareTo(Version other)
        {
            return CompareVersions(this, other);
        }

        #endregion

        #region IComparer

        public int Compare(object v1, object v2)
        {
            return CompareVersions(v1 as Version, v2 as Version);
        }

        #endregion

        #region IComparer<Version>

        public int Compare(Version v1, Version v2)
        {
            return CompareVersions(v1, v2);
        }

        #endregion

        #region IEquatable<Version>

        public bool Equals(Version other)
        {
            return CompareVersions(this, other) == 0;
        }

        #endregion

        #region IEqualityComparer<Version>

        public bool Equals(Version v1, Version v2)
        {
            return CompareVersions(v1, v2) == 0;
        }

        public int GetHashCode(Version obj)
        {
            if (obj == null)
                return 0;
            return obj.GetHashCode();
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            return new Version(this);
        }

        #endregion

        #region Compare Versions

        private static int CompareVersions(Version v1, Version v2)
        {
            if (ReferenceEquals(v1, v2))
                return 0;

            if (v2 is null)
                return 1;

            if (v1 is null)
                return -1;

            return CompareVersions(v1.Major, v1.Minor, v1.Build, v1.Revision, v2.Major, v2.Minor, v2.Build, v2.Revision);
        }

        private static int CompareVersions(Version v1, int v2Major)
        {
            if (v1 is null)
                return -1;

            return CompareVersions(v1.Major, v1.Minor, v1.Build, v1.Revision, v2Major, 0, 0, 0);
        }

        private static int CompareVersions(int v1Major, Version v2)
        {
            if (v2 is null)
                return 1;

            return CompareVersions(v1Major, 0, 0, 0, v2.Major, v2.Minor, v2.Build, v2.Revision);
        }

        private static int CompareVersions(Version v1, double v2MajorMinor)
        {
            if (v1 is null)
                return -1;

            string[] segments = v2MajorMinor.ToString(CultureInfo.InvariantCulture).Split('.');
            int v2Major = int.Parse(segments[0]);
            int v2Minor = (segments.Length == 2 ? int.Parse(segments[1]) : 0);

            return CompareVersions(v1.Major, v1.Minor, v1.Build, v1.Revision, v2Major, v2Minor, 0, 0);
        }

        private static int CompareVersions(double v1MajorMinor, Version v2)
        {
            if (v2 is null)
                return 1;

            string[] segments = v1MajorMinor.ToString(CultureInfo.InvariantCulture).Split('.');
            int v1Major = int.Parse(segments[0]);
            int v1Minor = (segments.Length == 2 ? int.Parse(segments[1]) : 0);

            return CompareVersions(v1Major, v1Minor, 0, 0, v2.Major, v2.Minor, v2.Build, v2.Revision);
        }

        private static int CompareVersions(int v1Major, int v1Minor, int v1Build, int v1Revision, int v2Major, int v2Minor, int v2Build, int v2Revision)
        {
            if (v1Major < v2Major)
                return -1;

            if (v1Major > v2Major)
                return 1;

            if (v1Minor < v2Minor)
                return -1;

            if (v1Minor > v2Minor)
                return 1;

            if (v1Build < v2Build)
                return -1;

            if (v1Build > v2Build)
                return 1;

            if (v1Revision < v2Revision)
                return -1;

            if (v1Revision > v2Revision)
                return 1;

            return 0;
        }

        #endregion
    }
}
