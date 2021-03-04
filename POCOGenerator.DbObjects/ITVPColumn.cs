using System;

namespace POCOGenerator.DbObjects
{
    public interface ITVPColumn : IColumn, IDescription
    {
        ITVP TVP { get; set; }
    }
}
