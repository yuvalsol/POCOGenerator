using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a RDBMS server.</summary>
    public sealed class Server
    {
        private readonly POCOGenerator.DbObjects.IServer server;
        private readonly List<DatabaseAccessibleObjects> databasesAccessibleObjects;

        internal Server(POCOGenerator.DbObjects.IServer server, List<DatabaseAccessibleObjects> databasesAccessibleObjects)
        {
            this.server = server;
            this.databasesAccessibleObjects = databasesAccessibleObjects;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IServer server)
        {
            return this.server == server;
        }

        /// <summary>Gets the name of the server.</summary>
        /// <value>The name of the server.</value>
        public string ServerName { get { return this.server.ServerName; } }

        /// <summary>Gets the name of the instance of the server.</summary>
        /// <value>The name of the instance of the server.</value>
        public string InstanceName { get { return this.server.InstanceName; } }
        
        /// <summary>Gets the user identifier connected to the server.</summary>
        /// <value>The user identifier connected to the server.</value>
        public string UserId { get { return this.server.UserId; } }

        private Version version;
        /// <summary>Gets the version of the server.</summary>
        /// <value>The version of the server.</value>
        public Version Version
        {
            get
            {
                if (this.server.Version == null)
                    return null;

                if (this.version == null)
                    this.version = new Version(this.server.Version);

                return this.version;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IDatabase, Database> databases;
        /// <summary>Gets the collection of databases that belong to this server.</summary>
        /// <value>Collection of databases.</value>
        public IEnumerable<Database> Databases
        {
            get
            {
                if (this.server.Databases.IsNullOrEmpty())
                    yield break;

                if (this.databases == null)
                {
                    this.databases = new CachedEnumerable<POCOGenerator.DbObjects.IDatabase, Database>(
                        this.server.Databases.Where(d => this.databasesAccessibleObjects.Any(x => x.Database == d)).ToList(),
                        d => new Database(d, this, this.databasesAccessibleObjects.First(x => x.Database == d))
                    );
                }

                foreach (var database in this.databases)
                {
                    yield return database;
                }
            }
        }

        /// <summary>Returns the fully qualified server name with the version of the server.</summary>
        /// <returns>The fully qualified server name with the version of the server.</returns>
        public string ToStringWithVersion()
        {
            return this.server.ToStringWithVersion();
        }

        /// <summary>Returns the fully qualified server name.</summary>
        /// <returns>The fully qualified server name.</returns>
        public override string ToString()
        {
            return this.server.ToString();
        }
    }
}
