using System;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class ViewColumn : TableColumn
    {
        /* not in use. reduce memory.
        public string VIEW_CATALOG { get; set; }
        public string VIEW_SCHEMA { get; set; }
        public string VIEW_NAME { get; set; }
        public ulong? CHARACTER_OCTET_LENGTH { get; set; }

        public override string TABLE_CATALOG { get { return VIEW_CATALOG; } set { VIEW_CATALOG = value; } }
        public override string TABLE_SCHEMA { get { return VIEW_SCHEMA; } set { VIEW_SCHEMA = value; } }
        public override string TABLE_NAME { get { return VIEW_NAME; } set { VIEW_NAME = value; } }
        */
    }
}
