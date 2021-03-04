using System;

namespace POCOGenerator.POCOIterators
{
    #region Db Iterator Settings

    public interface IDbIteratorSettings
    {
        IPOCOIteratorSettings POCOIteratorSettings { get; }
        INavigationPropertiesIteratorSettings NavigationPropertiesIteratorSettings { get; }
        IClassNameIteratorSettings ClassNameIteratorSettings { get; }
        IEFAnnotationsIteratorSettings EFAnnotationsIteratorSettings { get; }
    }

    #endregion

    #region POCO Iterator Settings

    public interface IPOCOIteratorSettings
    {
        bool Properties { get; set; }
        bool Fields { get; set; }
        bool VirtualProperties { get; set; }
        bool OverrideProperties { get; set; }
        bool PartialClass { get; set; }
        bool StructTypesNullable { get; set; }
        bool Comments { get; set; }
        bool CommentsWithoutNull { get; set; }
        bool Using { get; set; }
        bool UsingInsideNamespace { get; set; }
        string Namespace { get; set; }
        bool WrapAroundEachClass { get; set; }
        string Inherit { get; set; }
        bool ColumnDefaults { get; set; }
        bool NewLineBetweenMembers { get; set; }
        bool EnumSQLTypeToString { get; set; }
        bool EnumSQLTypeToEnumUShort { get; set; }
        bool EnumSQLTypeToEnumInt { get; set; }
        string Tab { get; set; }
    }

    #endregion

    #region Navigation Properties Iterator Settings

    public interface INavigationPropertiesIteratorSettings
    {
        bool Enable { get; set; }
        bool VirtualNavigationProperties { get; set; }
        bool OverrideNavigationProperties { get; set; }
        bool ShowManyToManyJoinTable { get; set; }
        bool Comments { get; set; }
        bool ListNavigationProperties { get; set; }
        bool ICollectionNavigationProperties { get; set; }
        bool IEnumerableNavigationProperties { get; set; }
    }

    #endregion

    #region Class Name Iterator Settings

    public interface IClassNameIteratorSettings
    {
        bool Singular { get; set; }
        bool IncludeDB { get; set; }
        string DBSeparator { get; set; }
        bool IncludeSchema { get; set; }
        bool IgnoreDboSchema { get; set; }
        string SchemaSeparator { get; set; }
        string WordsSeparator { get; set; }
        bool CamelCase { get; set; }
        bool UpperCase { get; set; }
        bool LowerCase { get; set; }
        string Search { get; set; }
        string Replace { get; set; }
        bool SearchIgnoreCase { get; set; }
        string FixedClassName { get; set; }
        string Prefix { get; set; }
        string Suffix { get; set; }
    }

    #endregion

    #region EF Annotations Iterator Settings

    public interface IEFAnnotationsIteratorSettings
    {
        bool Enable { get; set; }
        bool Column { get; set; }
        bool Required { get; set; }
        bool RequiredWithErrorMessage { get; set; }
        bool ConcurrencyCheck { get; set; }
        bool StringLength { get; set; }
        bool Display { get; set; }
        bool Description { get; set; }
        bool ComplexType { get; set; }
        bool Index { get; set; }
        bool ForeignKeyAndInverseProperty { get; set; }
    }

    #endregion
}
