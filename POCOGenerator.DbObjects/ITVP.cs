using System;
using System.Collections.Generic;
using System.Data;

namespace POCOGenerator.DbObjects
{
    public interface ITVP : IDbObjectTraverse, IDescription
    {
        int TVPId { get; }
        List<ITVPColumn> TVPColumns { get; set; }
        DataTable TVPDataTable { get; set; }
    }
}
