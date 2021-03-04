using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    public abstract class DbSupportBase : IDbSupport
    {
        protected Dictionary<string, bool> support = new Dictionary<string, bool>()
        {
            { "IsSupportSchema", false },
            { "IsSupportTableFunctions", false },
            { "IsSupportTVPs", false },
            { "IsSupportEnumDataType", false }
        };

        public virtual bool this[string key]
        {
            get { return support[key]; }
            set { support[key] = value; }
        }

        public virtual bool IsSupportSchema { get { return support["IsSupportSchema"]; } set { support["IsSupportSchema"] = value; } }
        public virtual bool IsSupportTableFunctions { get { return support["IsSupportTableFunctions"]; } set { support["IsSupportTableFunctions"] = value; } }
        public virtual bool IsSupportTVPs { get { return support["IsSupportTVPs"]; } set { support["IsSupportTVPs"] = value; } }
        public virtual bool IsSupportEnumDataType { get { return support["IsSupportEnumDataType"]; } set { support["IsSupportEnumDataType"] = value; } }

        public virtual string DefaultSchema { get; set; }
        public virtual POCOGenerator.DbObjects.Version Version { get; set; }
    }
}
