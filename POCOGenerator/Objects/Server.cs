using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
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

        public string ServerName { get { return this.server.ServerName; } }
        public string InstanceName { get { return this.server.InstanceName; } }
        public string UserId { get { return this.server.UserId; } }

        private Version version;
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

        public string ToStringWithVersion()
        {
            return this.server.ToStringWithVersion();
        }

        public override string ToString()
        {
            return this.server.ToString();
        }
    }
}
