using System;

namespace POCOGenerator.Db.DbObjects
{
    public interface IInternalForeignKey
    {
        int Id { get; }
        string Name { get; }
        bool Is_One_To_One { get; }
        bool Is_One_To_Many { get; }
        bool Is_Many_To_Many { get; }
        bool Is_Many_To_Many_Complete { get; }
        bool Is_Cascade_Delete { get; }
        bool Is_Cascade_Update { get; }
        string Foreign_Table_Id { get; }
        string Foreign_Table { get; }
        string Foreign_Column_Id { get; }
        string Foreign_Column { get; }
        bool Is_Foreign_Column_PK { get; }
        string Primary_Table_Id { get; }
        string Primary_Table { get; }
        string Primary_Column_Id { get; }
        string Primary_Column { get; }
        bool Is_Primary_Column_PK { get; }
        int Ordinal { get; }
    }

    public interface IInternalForeignKeySchema
    {
        string Foreign_Schema_Id { get; set; }
        string Foreign_Schema { get; set; }
        string Primary_Schema_Id { get; set; }
        string Primary_Schema { get; set; }
    }

    public abstract class InternalForeignKeyBase : IInternalForeignKey
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Is_One_To_One { get; set; }
        public virtual bool Is_One_To_Many { get; set; }
        public virtual bool Is_Many_To_Many { get; set; }
        public virtual bool Is_Many_To_Many_Complete { get; set; }
        public virtual bool Is_Cascade_Delete { get; set; }
        public virtual bool Is_Cascade_Update { get; set; }
        public virtual string Foreign_Table_Id { get; set; }
        public virtual string Foreign_Table { get; set; }
        public virtual string Foreign_Column_Id { get; set; }
        public virtual string Foreign_Column { get; set; }
        public virtual bool Is_Foreign_Column_PK { get; set; }
        public virtual string Primary_Table_Id { get; set; }
        public virtual string Primary_Table { get; set; }
        public virtual string Primary_Column_Id { get; set; }
        public virtual string Primary_Column { get; set; }
        public virtual bool Is_Primary_Column_PK { get; set; }
        public virtual int Ordinal { get; set; }
    }

    public sealed class ForeignKeyInternal : InternalForeignKeyBase
    {
    }

    public sealed class ForeignKeySchemaInternal : InternalForeignKeyBase, IInternalForeignKeySchema
    {
        public string Foreign_Schema_Id { get; set; }
        public string Foreign_Schema { get; set; }
        public string Primary_Schema_Id { get; set; }
        public string Primary_Schema { get; set; }
    }
}
