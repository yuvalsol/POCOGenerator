using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IServer : IDbObject
    {
        string RDBMSName { get; set; }

        string ServerName { get; set; }
        string InstanceName { get; set; }
        Version Version { get; set; }
        string UserId { get; set; }

        List<IDatabase> Databases { get; set; }

        string ToStringWithVersion();
    }
}