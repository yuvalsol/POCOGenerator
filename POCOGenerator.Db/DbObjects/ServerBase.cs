using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public abstract class ServerBase : IServer
    {
        public virtual string RDBMSName { get; set; }

        public virtual string ServerName { get; set; }
        public virtual string InstanceName { get; set; }
        public virtual Version Version { get; set; }
        public virtual string UserId { get; set; }

        public virtual List<IDatabase> Databases { get; set; }

        public override string ToString()
        {
            return ServerName + (string.IsNullOrEmpty(InstanceName) ? string.Empty : "\\" + InstanceName);
        }

        public virtual string ToStringWithVersion()
        {
            string serverName = this.ToString();

            if (Version != null)
            {
                serverName += string.Format(" ({0} {1}{2})",
                    RDBMSName,
                    Version.ToString(3),
                    (string.IsNullOrEmpty(UserId) ? string.Empty : " - " + UserId)
                );
            }

            return serverName;
        }
    }
}