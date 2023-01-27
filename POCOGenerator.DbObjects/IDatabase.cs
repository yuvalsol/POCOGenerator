using System;
using System.Collections.Generic;

namespace POCOGenerator.DbObjects
{
    public interface IDatabase : IDbObject, IDescription
    {
        IServer Server { get; set; }
        List<ITable> Tables { get; set; }
        List<IView> Views { get; set; }
        List<IProcedure> Procedures { get; set; }
        List<IFunction> Functions { get; set; }
        List<ITVP> TVPs { get; set; }
        List<IComplexType> ComplexTypes { get; set; }
        List<Exception> Errors { get; set; }

        string Name { get; }

        string GetDatabaseConnectionString(string connectionString);
    }
}