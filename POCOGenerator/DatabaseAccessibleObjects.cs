using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator
{
    internal class DatabaseAccessibleObjects
    {
        public IDatabase Database { get; set; }
        public List<ITable> Tables { get; set; }
        public List<ITable> AccessibleTables { get; set; }
        public List<IView> Views { get; set; }
        public List<IProcedure> Procedures { get; set; }
        public List<IFunction> Functions { get; set; }
        public List<ITVP> TVPs { get; set; }
    }
}
