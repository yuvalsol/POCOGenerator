using System;
using POCOGenerator.DbObjects;
using POCOGenerator.POCOIterators;
using POCOGenerator.POCOWriters;

namespace POCOGenerator.DbHandlers
{
    public interface IDbHandler
    {
        IDbHelper GetDbHelper(string connectionString);
        IConnectionStringParser GetConnectionStringParser();
        IServer GetServer();
        IDbIterator GetIterator(IWriter writer, IDbSupport support, IDbIteratorSettings settings);
    }
}
