using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using POCOGenerator.POCOIterators;

namespace POCOGenerator
{
    internal sealed class GeneratorSettings : Settings, IDbIteratorSettings, ICloneable
    {
        #region Constructors

        private readonly object lockObject;

        internal GeneratorSettings(object lockObject)
        {
            this.lockObject = lockObject;

            this.Connection = new ConnectionSettings(lockObject);
            this.POCO = new POCOSettings(lockObject);
            this.ClassName = new ClassNameSettings(lockObject);
            this.NavigationProperties = new NavigationPropertiesSettings(lockObject);
            this.EFAnnotations = new EFAnnotationsSettings(lockObject);
            this.DatabaseObjects = new DatabaseObjectsSettings(lockObject);
            this.SyntaxHighlight = new SyntaxHighlightSettings(lockObject);
        }

        public GeneratorSettings(Settings settings)
            : this(new object())
        {
            #region Connection

            this.Connection.ConnectionString = settings.Connection.ConnectionString;
            this.Connection.RDBMS = settings.Connection.RDBMS;

            #endregion

            #region POCO

            this.POCO.Properties = settings.POCO.Properties;
            this.POCO.Fields = settings.POCO.Fields;
            this.POCO.VirtualProperties = settings.POCO.VirtualProperties;
            this.POCO.OverrideProperties = settings.POCO.OverrideProperties;
            this.POCO.PartialClass = settings.POCO.PartialClass;
            this.POCO.StructTypesNullable = settings.POCO.StructTypesNullable;
            this.POCO.Comments = settings.POCO.Comments;
            this.POCO.CommentsWithoutNull = settings.POCO.CommentsWithoutNull;
            this.POCO.Using = settings.POCO.Using;
            this.POCO.UsingInsideNamespace = settings.POCO.UsingInsideNamespace;
            this.POCO.Namespace = settings.POCO.Namespace;
            this.POCO.WrapAroundEachClass = settings.POCO.WrapAroundEachClass;
            this.POCO.Inherit = settings.POCO.Inherit;
            this.POCO.ColumnDefaults = settings.POCO.ColumnDefaults;
            this.POCO.NewLineBetweenMembers = settings.POCO.NewLineBetweenMembers;
            this.POCO.ComplexTypes = settings.POCO.ComplexTypes;
            this.POCO.EnumSQLTypeToString = settings.POCO.EnumSQLTypeToString;
            this.POCO.EnumSQLTypeToEnumUShort = settings.POCO.EnumSQLTypeToEnumUShort;
            this.POCO.EnumSQLTypeToEnumInt = settings.POCO.EnumSQLTypeToEnumInt;
            this.POCO.Tab = settings.POCO.Tab;

            #endregion

            #region Class Name

            this.ClassName.Singular = settings.ClassName.Singular;
            this.ClassName.IncludeDB = settings.ClassName.IncludeDB;
            this.ClassName.DBSeparator = settings.ClassName.DBSeparator;
            this.ClassName.IncludeSchema = settings.ClassName.IncludeSchema;
            this.ClassName.IgnoreDboSchema = settings.ClassName.IgnoreDboSchema;
            this.ClassName.SchemaSeparator = settings.ClassName.SchemaSeparator;
            this.ClassName.WordsSeparator = settings.ClassName.WordsSeparator;
            this.ClassName.CamelCase = settings.ClassName.CamelCase;
            this.ClassName.UpperCase = settings.ClassName.UpperCase;
            this.ClassName.LowerCase = settings.ClassName.LowerCase;
            this.ClassName.Search = settings.ClassName.Search;
            this.ClassName.Replace = settings.ClassName.Replace;
            this.ClassName.SearchIgnoreCase = settings.ClassName.SearchIgnoreCase;
            this.ClassName.FixedClassName = settings.ClassName.FixedClassName;
            this.ClassName.Prefix = settings.ClassName.Prefix;
            this.ClassName.Suffix = settings.ClassName.Suffix;

            #endregion

            #region Navigation Properties

            this.NavigationProperties.Enable = settings.NavigationProperties.Enable;
            this.NavigationProperties.VirtualNavigationProperties = settings.NavigationProperties.VirtualNavigationProperties;
            this.NavigationProperties.OverrideNavigationProperties = settings.NavigationProperties.OverrideNavigationProperties;
            this.NavigationProperties.ManyToManyJoinTable = settings.NavigationProperties.ManyToManyJoinTable;
            this.NavigationProperties.Comments = settings.NavigationProperties.Comments;
            this.NavigationProperties.ListNavigationProperties = settings.NavigationProperties.ListNavigationProperties;
            this.NavigationProperties.IListNavigationProperties = settings.NavigationProperties.IListNavigationProperties;
            this.NavigationProperties.ICollectionNavigationProperties = settings.NavigationProperties.ICollectionNavigationProperties;
            this.NavigationProperties.IEnumerableNavigationProperties = settings.NavigationProperties.IEnumerableNavigationProperties;

            #endregion

            #region EF Annotations

            this.EFAnnotations.Enable = settings.EFAnnotations.Enable;
            this.EFAnnotations.Column = settings.EFAnnotations.Column;
            this.EFAnnotations.Required = settings.EFAnnotations.Required;
            this.EFAnnotations.RequiredWithErrorMessage = settings.EFAnnotations.RequiredWithErrorMessage;
            this.EFAnnotations.ConcurrencyCheck = settings.EFAnnotations.ConcurrencyCheck;
            this.EFAnnotations.StringLength = settings.EFAnnotations.StringLength;
            this.EFAnnotations.Display = settings.EFAnnotations.Display;
            this.EFAnnotations.Description = settings.EFAnnotations.Description;
            this.EFAnnotations.ComplexType = settings.EFAnnotations.ComplexType;
            this.EFAnnotations.Index = settings.EFAnnotations.Index;
            this.EFAnnotations.ForeignKeyAndInverseProperty = settings.EFAnnotations.ForeignKeyAndInverseProperty;

            #endregion

            #region Database Objects

            this.DatabaseObjects.IncludeAll = settings.DatabaseObjects.IncludeAll;

            #endregion

            #region Tables

            this.DatabaseObjects.Tables.IncludeAll = settings.DatabaseObjects.Tables.IncludeAll;
            this.DatabaseObjects.Tables.ExcludeAll = settings.DatabaseObjects.Tables.ExcludeAll;

            foreach (string item in settings.DatabaseObjects.Tables.Include)
                this.DatabaseObjects.Tables.Include.Add(item);

            foreach (string item in settings.DatabaseObjects.Tables.Exclude)
                this.DatabaseObjects.Tables.Exclude.Add(item);

            #endregion

            #region Views

            this.DatabaseObjects.Views.IncludeAll = settings.DatabaseObjects.Views.IncludeAll;
            this.DatabaseObjects.Views.ExcludeAll = settings.DatabaseObjects.Views.ExcludeAll;

            foreach (string item in settings.DatabaseObjects.Views.Include)
                this.DatabaseObjects.Views.Include.Add(item);

            foreach (string item in settings.DatabaseObjects.Views.Exclude)
                this.DatabaseObjects.Views.Exclude.Add(item);

            #endregion

            #region Stored Procedures

            this.DatabaseObjects.StoredProcedures.IncludeAll = settings.DatabaseObjects.StoredProcedures.IncludeAll;
            this.DatabaseObjects.StoredProcedures.ExcludeAll = settings.DatabaseObjects.StoredProcedures.ExcludeAll;

            foreach (string item in settings.DatabaseObjects.StoredProcedures.Include)
                this.DatabaseObjects.StoredProcedures.Include.Add(item);

            foreach (string item in settings.DatabaseObjects.StoredProcedures.Exclude)
                this.DatabaseObjects.StoredProcedures.Exclude.Add(item);

            #endregion

            #region Functions

            this.DatabaseObjects.Functions.IncludeAll = settings.DatabaseObjects.Functions.IncludeAll;
            this.DatabaseObjects.Functions.ExcludeAll = settings.DatabaseObjects.Functions.ExcludeAll;

            foreach (string item in settings.DatabaseObjects.Functions.Include)
                this.DatabaseObjects.Functions.Include.Add(item);

            foreach (string item in settings.DatabaseObjects.Functions.Exclude)
                this.DatabaseObjects.Functions.Exclude.Add(item);

            #endregion

            #region TVPs

            this.DatabaseObjects.TVPs.IncludeAll = settings.DatabaseObjects.TVPs.IncludeAll;
            this.DatabaseObjects.TVPs.ExcludeAll = settings.DatabaseObjects.TVPs.ExcludeAll;

            foreach (string item in settings.DatabaseObjects.TVPs.Include)
                this.DatabaseObjects.TVPs.Include.Add(item);

            foreach (string item in settings.DatabaseObjects.TVPs.Exclude)
                this.DatabaseObjects.TVPs.Exclude.Add(item);

            #endregion

            #region Syntax Highlight

            this.SyntaxHighlight.Text = settings.SyntaxHighlight.Text;
            this.SyntaxHighlight.Keyword = settings.SyntaxHighlight.Keyword;
            this.SyntaxHighlight.UserType = settings.SyntaxHighlight.UserType;
            this.SyntaxHighlight.String = settings.SyntaxHighlight.String;
            this.SyntaxHighlight.Comment = settings.SyntaxHighlight.Comment;
            this.SyntaxHighlight.Error = settings.SyntaxHighlight.Error;
            this.SyntaxHighlight.Background = settings.SyntaxHighlight.Background;

            #endregion
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            return new GeneratorSettings(this);
        }

        #endregion

        #region Reset

        public void Reset()
        {
            lock (lockObject)
            {
                this.Connection.Reset();
                this.POCO.Reset();
                this.ClassName.Reset();
                this.NavigationProperties.Reset();
                this.EFAnnotations.Reset();
                this.DatabaseObjects.Reset();
                this.SyntaxHighlight.Reset();
            }
        }

        #endregion

        #region Settings

        public Connection Connection { get; private set; }
        public POCO POCO { get; private set; }
        public ClassName ClassName { get; private set; }
        public NavigationProperties NavigationProperties { get; private set; }
        public EFAnnotations EFAnnotations { get; private set; }
        public DatabaseObjects DatabaseObjects { get; private set; }
        public SyntaxHighlight SyntaxHighlight { get; private set; }

        #endregion

        #region Connection Settings

        private sealed class ConnectionSettings : Connection
        {
            private readonly object lockObject;

            internal ConnectionSettings(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.ConnectionString = null;
                    this.RDBMS = RDBMS.None;
                }
            }

            private string connectionString;
            public string ConnectionString
            {
                get
                {
                    lock (lockObject)
                    {
                        return connectionString;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        connectionString = value;
                    }
                }
            }

            private RDBMS rdbms = RDBMS.None;
            public RDBMS RDBMS
            {
                get
                {
                    lock (lockObject)
                    {
                        return rdbms;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        rdbms = value;
                    }
                }
            }
        }

        #endregion

        #region POCO Settings

        private sealed class POCOSettings : POCO, IPOCOIteratorSettings
        {
            private readonly object lockObject;

            internal POCOSettings(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.Properties = true;
                    this.Fields = false;
                    this.VirtualProperties = false;
                    this.OverrideProperties = false;
                    this.PartialClass = false;
                    this.StructTypesNullable = false;
                    this.Comments = false;
                    this.CommentsWithoutNull = false;
                    this.Using = false;
                    this.UsingInsideNamespace = false;
                    this.Namespace = null;
                    this.WrapAroundEachClass = false;
                    this.Inherit = null;
                    this.ColumnDefaults = false;
                    this.NewLineBetweenMembers = false;
                    this.ComplexTypes = false;
                    this.EnumSQLTypeToString = true;
                    this.EnumSQLTypeToEnumUShort = false;
                    this.EnumSQLTypeToEnumInt = false;
                    this.Tab = "    ";
                }
            }

            private bool properties = true;
            private bool fields;

            public bool Properties
            {
                get
                {
                    lock (lockObject)
                    {
                        return properties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (properties != value)
                        {
                            if (value)
                            {
                                properties = true;
                                fields = false;
                            }
                            else
                            {
                                properties = false;
                                fields = true;
                            }
                        }
                    }
                }
            }

            public bool Fields
            {
                get
                {
                    lock (lockObject)
                    {
                        return fields;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (fields != value)
                        {
                            if (value)
                            {
                                properties = false;
                                fields = true;
                            }
                            else
                            {
                                properties = true;
                                fields = false;
                            }
                        }
                    }
                }
            }

            private bool virtualProperties;
            private bool overrideProperties;

            public bool VirtualProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return virtualProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (virtualProperties != value)
                        {
                            virtualProperties = value;

                            if (virtualProperties && virtualProperties == overrideProperties)
                                overrideProperties = false;
                        }
                    }
                }
            }

            public bool OverrideProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return overrideProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (overrideProperties != value)
                        {
                            overrideProperties = value;

                            if (overrideProperties && virtualProperties == overrideProperties)
                                virtualProperties = false;
                        }
                    }
                }
            }

            private bool partialClass;
            public bool PartialClass
            {
                get
                {
                    lock (lockObject)
                    {
                        return partialClass;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        partialClass = value;
                    }
                }
            }

            private bool structTypesNullable;
            public bool StructTypesNullable
            {
                get
                {
                    lock (lockObject)
                    {
                        return structTypesNullable;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        structTypesNullable = value;
                    }
                }
            }

            private bool comments;
            private bool commentsWithoutNull;

            public bool Comments
            {
                get
                {
                    lock (lockObject)
                    {
                        return comments;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (comments != value)
                        {
                            comments = value;

                            if (comments && comments == commentsWithoutNull)
                                commentsWithoutNull = false;
                        }
                    }
                }
            }

            public bool CommentsWithoutNull
            {
                get
                {
                    lock (lockObject)
                    {
                        return commentsWithoutNull;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (commentsWithoutNull != value)
                        {
                            commentsWithoutNull = value;

                            if (commentsWithoutNull && comments == commentsWithoutNull)
                                comments = false;
                        }
                    }
                }
            }

            private bool @using;
            public bool Using
            {
                get
                {
                    lock (lockObject)
                    {
                        return @using;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        @using = value;
                    }
                }
            }

            private bool usingInsideNamespace;
            public bool UsingInsideNamespace
            {
                get
                {
                    lock (lockObject)
                    {
                        return usingInsideNamespace;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        usingInsideNamespace = value;
                    }
                }
            }

            private string @namespace;
            public string Namespace
            {
                get
                {
                    lock (lockObject)
                    {
                        return @namespace;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        @namespace = value;
                    }
                }
            }

            private bool wrapAroundEachClass;
            public bool WrapAroundEachClass
            {
                get
                {
                    lock (lockObject)
                    {
                        return wrapAroundEachClass;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        wrapAroundEachClass = value;
                    }
                }
            }

            private string inherit;
            public string Inherit
            {
                get
                {
                    lock (lockObject)
                    {
                        return inherit;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        inherit = value;
                    }
                }
            }

            private bool columnDefaults;
            public bool ColumnDefaults
            {
                get
                {
                    lock (lockObject)
                    {
                        return columnDefaults;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        columnDefaults = value;
                    }
                }
            }

            private bool newLineBetweenMembers;
            public bool NewLineBetweenMembers
            {
                get
                {
                    lock (lockObject)
                    {
                        return newLineBetweenMembers;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        newLineBetweenMembers = value;
                    }
                }
            }

            private bool complexTypes;
            public bool ComplexTypes
            {
                get
                {
                    lock (lockObject)
                    {
                        return complexTypes;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        complexTypes = value;
                    }
                }
            }

            private bool enumSQLTypeToString = true;
            private bool enumSQLTypeToEnumUShort;
            private bool enumSQLTypeToEnumInt;

            public bool EnumSQLTypeToString
            {
                get
                {
                    lock (lockObject)
                    {
                        return enumSQLTypeToString;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (enumSQLTypeToString != value)
                        {
                            if (value)
                            {
                                enumSQLTypeToString = true;
                                enumSQLTypeToEnumUShort = false;
                                enumSQLTypeToEnumInt = false;
                            }
                            else
                            {
                                enumSQLTypeToString = false;
                                enumSQLTypeToEnumUShort = true;
                                enumSQLTypeToEnumInt = false;
                            }
                        }
                    }
                }
            }

            public bool EnumSQLTypeToEnumUShort
            {
                get
                {
                    lock (lockObject)
                    {
                        return enumSQLTypeToEnumUShort;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (enumSQLTypeToEnumUShort != value)
                        {
                            if (value)
                            {
                                enumSQLTypeToString = false;
                                enumSQLTypeToEnumUShort = true;
                                enumSQLTypeToEnumInt = false;
                            }
                            else
                            {
                                enumSQLTypeToString = true;
                                enumSQLTypeToEnumUShort = false;
                                enumSQLTypeToEnumInt = false;
                            }
                        }
                    }
                }
            }

            public bool EnumSQLTypeToEnumInt
            {
                get
                {
                    lock (lockObject)
                    {
                        return enumSQLTypeToEnumInt;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (enumSQLTypeToEnumInt != value)
                        {
                            if (value)
                            {
                                enumSQLTypeToString = false;
                                enumSQLTypeToEnumUShort = false;
                                enumSQLTypeToEnumInt = true;
                            }
                            else
                            {
                                enumSQLTypeToString = true;
                                enumSQLTypeToEnumUShort = false;
                                enumSQLTypeToEnumInt = false;
                            }
                        }
                    }
                }
            }

            private string tab = "    ";
            public string Tab
            {
                get
                {
                    lock (lockObject)
                    {
                        return tab;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        tab = value;
                    }
                }
            }
        }

        #endregion

        #region Class Name Settings

        private sealed class ClassNameSettings : ClassName, IClassNameIteratorSettings
        {
            private readonly object lockObject;

            internal ClassNameSettings(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.Singular = false;
                    this.IncludeDB = false;
                    this.DBSeparator = null;
                    this.IncludeSchema = false;
                    this.IgnoreDboSchema = false;
                    this.SchemaSeparator = null;
                    this.WordsSeparator = null;
                    this.CamelCase = false;
                    this.UpperCase = false;
                    this.LowerCase = false;
                    this.Search = null;
                    this.Replace = null;
                    this.SearchIgnoreCase = false;
                    this.FixedClassName = null;
                    this.Prefix = null;
                    this.Suffix = null;
                }
            }

            private bool singular;
            public bool Singular
            {
                get
                {
                    lock (lockObject)
                    {
                        return singular;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        singular = value;
                    }
                }
            }

            private bool includeDB;
            public bool IncludeDB
            {
                get
                {
                    lock (lockObject)
                    {
                        return includeDB;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        includeDB = value;
                    }
                }
            }

            private string dbSeparator;
            public string DBSeparator
            {
                get
                {
                    lock (lockObject)
                    {
                        return dbSeparator;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        dbSeparator = value;
                    }
                }
            }

            private bool includeSchema;
            public bool IncludeSchema
            {
                get
                {
                    lock (lockObject)
                    {
                        return includeSchema;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        includeSchema = value;
                    }
                }
            }

            private bool ignoreDboSchema;
            public bool IgnoreDboSchema
            {
                get
                {
                    lock (lockObject)
                    {
                        return ignoreDboSchema;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        ignoreDboSchema = value;
                    }
                }
            }

            private string schemaSeparator;
            public string SchemaSeparator
            {
                get
                {
                    lock (lockObject)
                    {
                        return schemaSeparator;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        schemaSeparator = value;
                    }
                }
            }

            private string wordsSeparator;
            public string WordsSeparator
            {
                get
                {
                    lock (lockObject)
                    {
                        return wordsSeparator;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        wordsSeparator = value;
                    }
                }
            }

            private bool camelCase;
            public bool CamelCase
            {
                get
                {
                    lock (lockObject)
                    {
                        return camelCase;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        camelCase = value;
                    }
                }
            }

            private bool upperCase;
            public bool UpperCase
            {
                get
                {
                    lock (lockObject)
                    {
                        return upperCase;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        upperCase = value;
                    }
                }
            }

            private bool lowerCase;
            public bool LowerCase
            {
                get
                {
                    lock (lockObject)
                    {
                        return lowerCase;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        lowerCase = value;
                    }
                }
            }

            private string search;
            public string Search
            {
                get
                {
                    lock (lockObject)
                    {
                        return search;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        search = value;
                    }
                }
            }

            private string replace;
            public string Replace
            {
                get
                {
                    lock (lockObject)
                    {
                        return replace;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        replace = value;
                    }
                }
            }

            private bool searchIgnoreCase;
            public bool SearchIgnoreCase
            {
                get
                {
                    lock (lockObject)
                    {
                        return searchIgnoreCase;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        searchIgnoreCase = value;
                    }
                }
            }

            private string fixedClassName;
            public string FixedClassName
            {
                get
                {
                    lock (lockObject)
                    {
                        return fixedClassName;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        fixedClassName = value;
                    }
                }
            }

            private string prefix;
            public string Prefix
            {
                get
                {
                    lock (lockObject)
                    {
                        return prefix;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        prefix = value;
                    }
                }
            }

            private string suffix;
            public string Suffix
            {
                get
                {
                    lock (lockObject)
                    {
                        return suffix;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        suffix = value;
                    }
                }
            }
        }

        #endregion

        #region Navigation Properties Settings

        private sealed class NavigationPropertiesSettings : NavigationProperties, INavigationPropertiesIteratorSettings
        {
            private readonly object lockObject;

            internal NavigationPropertiesSettings(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.Enable = false;
                    this.VirtualNavigationProperties = false;
                    this.OverrideNavigationProperties = false;
                    this.ManyToManyJoinTable = false;
                    this.Comments = false;
                    this.ListNavigationProperties = true;
                    this.IListNavigationProperties = false;
                    this.ICollectionNavigationProperties = false;
                    this.IEnumerableNavigationProperties = false;
                }
            }

            private bool enable;
            public bool Enable
            {
                get
                {
                    lock (lockObject)
                    {
                        return enable;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        enable = value;
                    }
                }
            }

            private bool virtualNavigationProperties;
            private bool overrideNavigationProperties;

            public bool VirtualNavigationProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return virtualNavigationProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (virtualNavigationProperties != value)
                        {
                            virtualNavigationProperties = value;

                            if (virtualNavigationProperties && virtualNavigationProperties == overrideNavigationProperties)
                                overrideNavigationProperties = false;
                        }
                    }
                }
            }

            public bool OverrideNavigationProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return overrideNavigationProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (overrideNavigationProperties != value)
                        {
                            overrideNavigationProperties = value;

                            if (overrideNavigationProperties && virtualNavigationProperties == overrideNavigationProperties)
                                virtualNavigationProperties = false;
                        }
                    }
                }
            }

            private bool manyToManyJoinTable;
            public bool ManyToManyJoinTable
            {
                get
                {
                    lock (lockObject)
                    {
                        return manyToManyJoinTable;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        manyToManyJoinTable = value;
                    }
                }
            }

            private bool comments;
            public bool Comments
            {
                get
                {
                    lock (lockObject)
                    {
                        return comments;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        comments = value;
                    }
                }
            }

            private bool listNavigationProperties = true;
            private bool ilistNavigationProperties;
            private bool icollectionNavigationProperties;
            private bool ienumerableNavigationProperties;

            public bool ListNavigationProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return listNavigationProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (listNavigationProperties != value)
                        {
                            if (value)
                            {
                                listNavigationProperties = true;
                                ilistNavigationProperties = false;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = false;
                            }
                            else
                            {
                                listNavigationProperties = false;
                                ilistNavigationProperties = true;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = false;
                            }
                        }
                    }
                }
            }

            public bool IListNavigationProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return ilistNavigationProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (ilistNavigationProperties != value)
                        {
                            if (value)
                            {
                                listNavigationProperties = false;
                                ilistNavigationProperties = true;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = false;
                            }
                            else
                            {
                                listNavigationProperties = true;
                                ilistNavigationProperties = false;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = false;
                            }
                        }
                    }
                }
            }

            public bool ICollectionNavigationProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return icollectionNavigationProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (icollectionNavigationProperties != value)
                        {
                            if (value)
                            {
                                listNavigationProperties = false;
                                ilistNavigationProperties = false;
                                icollectionNavigationProperties = true;
                                ienumerableNavigationProperties = false;
                            }
                            else
                            {
                                listNavigationProperties = true;
                                ilistNavigationProperties = false;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = false;
                            }
                        }
                    }
                }
            }

            public bool IEnumerableNavigationProperties
            {
                get
                {
                    lock (lockObject)
                    {
                        return ienumerableNavigationProperties;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (ienumerableNavigationProperties != value)
                        {
                            if (value)
                            {
                                listNavigationProperties = false;
                                ilistNavigationProperties = false;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = true;
                            }
                            else
                            {
                                listNavigationProperties = true;
                                ilistNavigationProperties = false;
                                icollectionNavigationProperties = false;
                                ienumerableNavigationProperties = false;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region EF Annotations Settings

        private sealed class EFAnnotationsSettings : EFAnnotations, IEFAnnotationsIteratorSettings
        {
            private readonly object lockObject;

            internal EFAnnotationsSettings(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.Enable = false;
                    this.Column = false;
                    this.Required = false;
                    this.RequiredWithErrorMessage = false;
                    this.ConcurrencyCheck = false;
                    this.StringLength = false;
                    this.Display = false;
                    this.Description = false;
                    this.ComplexType = false;
                    this.Index = false;
                    this.ForeignKeyAndInverseProperty = false;
                }
            }

            private bool enable;
            public bool Enable
            {
                get
                {
                    lock (lockObject)
                    {
                        return enable;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        enable = value;
                    }
                }
            }

            private bool column;
            public bool Column
            {
                get
                {
                    lock (lockObject)
                    {
                        return column;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        column = value;
                    }
                }
            }

            private bool required;
            private bool requiredWithErrorMessage;

            public bool Required
            {
                get
                {
                    lock (lockObject)
                    {
                        return required;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (required != value)
                        {
                            required = value;

                            if (required && required == requiredWithErrorMessage)
                                requiredWithErrorMessage = false;
                        }
                    }
                }
            }

            public bool RequiredWithErrorMessage
            {
                get
                {
                    lock (lockObject)
                    {
                        return requiredWithErrorMessage;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        if (requiredWithErrorMessage != value)
                        {
                            requiredWithErrorMessage = value;

                            if (requiredWithErrorMessage && required == requiredWithErrorMessage)
                                required = false;
                        }
                    }
                }
            }

            private bool concurrencyCheck;
            public bool ConcurrencyCheck
            {
                get
                {
                    lock (lockObject)
                    {
                        return concurrencyCheck;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        concurrencyCheck = value;
                    }
                }
            }

            private bool stringLength;
            public bool StringLength
            {
                get
                {
                    lock (lockObject)
                    {
                        return stringLength;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        stringLength = value;
                    }
                }
            }

            private bool display;
            public bool Display
            {
                get
                {
                    lock (lockObject)
                    {
                        return display;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        display = value;
                    }
                }
            }

            private bool description;
            public bool Description
            {
                get
                {
                    lock (lockObject)
                    {
                        return description;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        description = value;
                    }
                }
            }

            private bool complexType;
            public bool ComplexType
            {
                get
                {
                    lock (lockObject)
                    {
                        return complexType;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        complexType = value;
                    }
                }
            }

            private bool index;
            public bool Index
            {
                get
                {
                    lock (lockObject)
                    {
                        return index;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        index = value;
                    }
                }
            }

            private bool foreignKeyAndInverseProperty;
            public bool ForeignKeyAndInverseProperty
            {
                get
                {
                    lock (lockObject)
                    {
                        return foreignKeyAndInverseProperty;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        foreignKeyAndInverseProperty = value;
                    }
                }
            }
        }

        #endregion

        #region Database Objects Settings

        private sealed class DatabaseObjectsSettings : DatabaseObjects
        {
            private readonly object lockObject;

            internal DatabaseObjectsSettings(object lockObject)
            {
                this.lockObject = lockObject;

                this.Tables = new TablesSettings(lockObject);
                this.Views = new ViewsSettings(lockObject);
                this.StoredProcedures = new StoredProceduresSettings(lockObject);
                this.Functions = new FunctionsSettings(lockObject);
                this.TVPs = new TVPsSettings(lockObject);
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.IncludeAll = false;
                    Tables.Reset();
                    Views.Reset();
                    StoredProcedures.Reset();
                    Functions.Reset();
                    TVPs.Reset();
                }
            }

            private bool includeAll;
            public bool IncludeAll
            {
                get
                {
                    lock (lockObject)
                    {
                        return includeAll;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        includeAll = value;
                    }
                }
            }

            public Tables Tables { get; private set; }
            public Views Views { get; private set; }
            public StoredProcedures StoredProcedures { get; private set; }
            public Functions Functions { get; private set; }
            public TVPs TVPs { get; private set; }
        }

        #endregion

        #region Db Objects Settings

        private abstract class DbObjectsSettingsBase
        {
            private readonly object lockObject;

            protected DbObjectsSettingsBase(object lockObject)
            {
                this.lockObject = lockObject;

                this.include = new SyncList<string>(lockObject);
                this.exclude = new SyncList<string>(lockObject);
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.IncludeAll = false;
                    this.ExcludeAll = false;
                    this.Include.Clear();
                    this.Exclude.Clear();
                }
            }

            private bool includeAll;
            public bool IncludeAll
            {
                get
                {
                    lock (lockObject)
                    {
                        return includeAll;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        includeAll = value;
                    }
                }
            }

            private bool excludeAll;
            public bool ExcludeAll
            {
                get
                {
                    lock (lockObject)
                    {
                        return excludeAll;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        excludeAll = value;
                    }
                }
            }

            private readonly SyncList<string> include;
            public IList<string> Include
            {
                get
                {
                    lock (lockObject)
                    {
                        return include;
                    }
                }
            }

            private readonly SyncList<string> exclude;
            public IList<string> Exclude
            {
                get
                {
                    lock (lockObject)
                    {
                        return exclude;
                    }
                }
            }
        }

        private sealed class SyncList<T> : IList<T>
        {
            private readonly List<T> list = new List<T>();
            private readonly object lockObject;

            public SyncList(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public T this[int index]
            {
                get
                {
                    lock (lockObject)
                    {
                        return (T)list[index];
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        list[index] = value;
                    }
                }
            }

            public int Count
            {
                get
                {
                    lock (lockObject)
                    {
                        return list.Count;
                    }
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public void Add(T item)
            {
                lock (lockObject)
                {
                    list.Add(item);
                }
            }

            public void Clear()
            {
                lock (lockObject)
                {
                    list.Clear();
                }
            }

            public bool Contains(T item)
            {
                lock (lockObject)
                {
                    return list.Contains(item);
                }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                lock (lockObject)
                {
                    list.CopyTo(array, arrayIndex);
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                lock (lockObject)
                {
                    return list.GetEnumerator();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                lock (lockObject)
                {
                    return list.GetEnumerator();
                }
            }

            public int IndexOf(T item)
            {
                lock (lockObject)
                {
                    return list.IndexOf(item);
                }
            }

            public void Insert(int index, T item)
            {
                lock (lockObject)
                {
                    list.Insert(index, item);
                }
            }

            public bool Remove(T item)
            {
                lock (lockObject)
                {
                    return list.Remove(item);
                }
            }

            public void RemoveAt(int index)
            {
                lock (lockObject)
                {
                    list.RemoveAt(index);
                }
            }
        }

        #endregion

        #region Tables Settings

        private sealed class TablesSettings : DbObjectsSettingsBase, Tables
        {
            internal TablesSettings(object lockObject)
                : base(lockObject)
            {
            }
        }

        #endregion

        #region Views Settings

        private sealed class ViewsSettings : DbObjectsSettingsBase, Views
        {
            internal ViewsSettings(object lockObject)
                : base(lockObject)
            {
            }
        }

        #endregion

        #region Stored Procedures Settings

        private sealed class StoredProceduresSettings : DbObjectsSettingsBase, StoredProcedures
        {
            internal StoredProceduresSettings(object lockObject)
                : base(lockObject)
            {
            }
        }

        #endregion

        #region Functions Settings

        private sealed class FunctionsSettings : DbObjectsSettingsBase, Functions
        {
            internal FunctionsSettings(object lockObject)
                : base(lockObject)
            {
            }
        }

        #endregion

        #region TVPs Settings

        private sealed class TVPsSettings : DbObjectsSettingsBase, TVPs
        {
            internal TVPsSettings(object lockObject)
                : base(lockObject)
            {
            }
        }

        #endregion

        #region Syntax Highlight Settings

        private sealed class SyntaxHighlightSettings : SyntaxHighlight
        {
            private readonly object lockObject;

            internal SyntaxHighlightSettings(object lockObject)
            {
                this.lockObject = lockObject;
            }

            public void Reset()
            {
                lock (lockObject)
                {
                    this.Text = Color.FromArgb(0, 0, 0);
                    this.Keyword = Color.FromArgb(0, 0, 255);
                    this.UserType = Color.FromArgb(43, 145, 175);
                    this.String = Color.FromArgb(163, 21, 21);
                    this.Comment = Color.FromArgb(0, 128, 0);
                    this.Error = Color.FromArgb(255, 0, 0);
                    this.Background = Color.FromArgb(255, 255, 255);
                }
            }

            private Color text = Color.FromArgb(0, 0, 0);
            public Color Text
            {
                get
                {
                    lock (lockObject)
                    {
                        return text;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        text = value;
                    }
                }
            }

            private Color keyword = Color.FromArgb(0, 0, 255);
            public Color Keyword
            {
                get
                {
                    lock (lockObject)
                    {
                        return keyword;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        keyword = value;
                    }
                }
            }

            private Color userType = Color.FromArgb(43, 145, 175);
            public Color UserType
            {
                get
                {
                    lock (lockObject)
                    {
                        return userType;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        userType = value;
                    }
                }
            }

            private Color @string = Color.FromArgb(163, 21, 21);
            public Color String
            {
                get
                {
                    lock (lockObject)
                    {
                        return @string;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        @string = value;
                    }
                }
            }

            private Color comment = Color.FromArgb(0, 128, 0);
            public Color Comment
            {
                get
                {
                    lock (lockObject)
                    {
                        return comment;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        comment = value;
                    }
                }
            }

            private Color error = Color.FromArgb(255, 0, 0);
            public Color Error
            {
                get
                {
                    lock (lockObject)
                    {
                        return error;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        error = value;
                    }
                }
            }

            private Color background = Color.FromArgb(255, 255, 255);
            public Color Background
            {
                get
                {
                    lock (lockObject)
                    {
                        return background;
                    }
                }

                set
                {
                    lock (lockObject)
                    {
                        background = value;
                    }
                }
            }
        }

        #endregion

        #region Db Iterator Settings

        public IPOCOIteratorSettings POCOIteratorSettings
        {
            get
            {
                return (POCOSettings)this.POCO;
            }
        }

        public IClassNameIteratorSettings ClassNameIteratorSettings
        {
            get
            {
                return (ClassNameSettings)this.ClassName;
            }
        }

        public INavigationPropertiesIteratorSettings NavigationPropertiesIteratorSettings
        {
            get
            {
                return (NavigationPropertiesSettings)this.NavigationProperties;
            }
        }

        public IEFAnnotationsIteratorSettings EFAnnotationsIteratorSettings
        {
            get
            {
                return (EFAnnotationsSettings)this.EFAnnotations;
            }
        }

        #endregion
    }

    #region Settings

    /// <summary>The settings determine the database connection, what database objects will be generated to classes and how the POCO classes will be generated.</summary>
    public interface Settings
    {
        /// <summary>Resets all settings to their default values.</summary>
        void Reset();

        /// <summary>Gets the Connection settings.</summary>
        Connection Connection { get; }

        /// <summary>Gets the POCO settings.</summary>
        POCO POCO { get; }

        /// <summary>Gets the class name settings.</summary>
        ClassName ClassName { get; }

        /// <summary>Gets the navigation properties settings.</summary>
        NavigationProperties NavigationProperties { get; }

        /// <summary>Gets the EF annotations settings.</summary>
        EFAnnotations EFAnnotations { get; }

        /// <summary>Gets the settings that determine which database objects to generate classes out of and which database objects to exclude from generating classes.</summary>
        DatabaseObjects DatabaseObjects { get; }

        /// <summary>Gets the settings that determine the colors for syntax elements.</summary>
        SyntaxHighlight SyntaxHighlight { get; }
    }

    #endregion

    #region Connection

    /// <summary>The settings determine the connection to the RDBMS server.</summary>
    public interface Connection
    {
        /// <summary>Resets the connection settings to their default values.</summary>
        void Reset();

        /// <summary>
        /// Gets or sets the connection string.
        /// <para>When <see cref="RDBMS" /> is set to <see cref="RDBMS.None" />, the generator will try to determine the <see cref="RDBMS" /> based on the connection string.</para></summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the relational database management system.
        /// <para>When this setting is set to <see cref="RDBMS.None" />, the generator will try to determine this setting based on the connection string.</para></summary>
        RDBMS RDBMS { get; set; }
    }

    #endregion

    #region POCO

    /// <summary>The settings determine how the POCO will be generated.</summary>
    public interface POCO
    {
        /// <summary>Resets the POCO settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to generate class members as properties.
        /// <para>The default value is <c>true</c>.</para></summary>
        bool Properties { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate class members as fields.
        /// <para>The default value is <c>false</c>.</para></summary>
        bool Fields { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate class properties with <see langword="virtual" /> modifier.</summary>
        bool VirtualProperties { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate class properties with <see langword="override" /> modifier.</summary>
        bool OverrideProperties { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate classes with <see langword="partial" /> modifier.</summary>
        bool PartialClass { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate struct types as nullable when the SQL column is mapped to a .NET struct type and it is not nullable.</summary>
        bool StructTypesNullable { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate comments for data members with the SQL column's data type and whether the SQL column is nullable or not.</summary>
        bool Comments { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate comments for data members with the SQL column's data type. The column nullability will not be generated.</summary>
        bool CommentsWithoutNull { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate a using directive clause.</summary>
        bool Using { get; set; }

        /// <summary>Gets or sets a value indicating whether the using directive clause will be inside the namespace.</summary>
        bool UsingInsideNamespace { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate classes wrapped with a namespace.
        /// <para>The value must be not null and not an empty string to take effect.</para></summary>
        string Namespace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to wrap a namespace and a using directive clause around each class individually.
        /// <para>This setting is useful when generating to multiple outputs (such as multiple files).</para></summary>
        bool WrapAroundEachClass { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate classes with inheritance clause.
        /// <para>The value must be not null and not an empty string to take effect.</para></summary>
        string Inherit { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate initialize data members with default values.</summary>
        bool ColumnDefaults { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate a new line between class members.</summary>
        bool NewLineBetweenMembers { get; set; }

        /// <summary>Gets or sets a value indicating whether to generate complex types.</summary>
        bool ComplexTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate a string data member for a SQL enum &amp; set date types.
        /// <para>This setting is applicable when the RDBMS supports SQL enum &amp; set data types, such as MySQL. The default value is <c>true</c>.</para></summary>
        bool EnumSQLTypeToString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate an enum of type ushort for a SQL enum date type and an enum of type ulong for a SQL set date type.
        /// <para>This setting is applicable when the RDBMS supports SQL enum &amp; set data types, such as MySQL. The default value is <c>false</c>.</para></summary>
        bool EnumSQLTypeToEnumUShort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate an enum of type int for a SQL enum &amp; set date types.
        /// <para>This setting is applicable when the RDBMS supports SQL enum &amp; set data types, such as MySQL. The default value is <c>false</c>.</para></summary>
        bool EnumSQLTypeToEnumInt { get; set; }

        /// <summary>Gets or sets a value indicating the tab string. The default value is 4 spaces.</summary>
        string Tab { get; set; }
    }

    #endregion

    #region Class Name

    /// <summary>The settings for the class name transformations.</summary>
    public interface ClassName
    {
        /// <summary>Resets the class name settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether the class name is changed from plural to singular.
        /// <para>This setting is applicable only for tables, views &amp; TVPs.</para></summary>
        bool Singular { get; set; }

        /// <summary>Gets or sets a value indicating whether to add the database name to the class name.</summary>
        bool IncludeDB { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the separator to add after the database name.
        /// <para>This setting is applicable only when <see cref="IncludeDB" /> is set to <c>true</c>.
        /// The value must be not null and not an empty string to take effect.</para></summary>
        string DBSeparator { get; set; }

        /// <summary>Gets or sets a value indicating whether to add the schema to the class name.
        /// <para>This setting is applicable only when the RDBMS supports schema, such as SQLServer.</para></summary>
        bool IncludeSchema { get; set; }

        /// <summary>Gets or sets a value indicating whether to not add the schema to the class name when the schema is "dbo".</summary>
        bool IgnoreDboSchema { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the separator to add after the schema name.
        /// <para>This setting is applicable only when <see cref="IncludeSchema" /> is set to <c>true</c>.
        /// The value must be not null and not an empty string to take effect.</para></summary>
        string SchemaSeparator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the separator to add between words in the class name. A word is defined as text between underscores or in a camel case.
        /// <para>The value must be not null and not an empty string to take effect.</para></summary>
        string WordsSeparator { get; set; }

        /// <summary>Gets or sets a value indicating whether to change the class name to camel case.</summary>
        bool CamelCase { get; set; }

        /// <summary>Gets or sets a value indicating whether to change the class name to upper case.</summary>
        bool UpperCase { get; set; }

        /// <summary>Gets or sets a value indicating whether to change the class name to lower case.</summary>
        bool LowerCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the string to search in the class name as part of the search-and-replace.
        /// <para>The searching is case sensitive by default. The searching can be set to insensitive when <see cref="SearchIgnoreCase" /> is set to <c>true</c>.</para></summary>
        string Search { get; set; }

        /// <summary>Gets or sets a value indicating the string to replace with the search string in the class name as part of the search-and-replace.</summary>
        string Replace { get; set; }

        /// <summary>Gets or sets a value indicating whether the searching, as part of the search-and-replace, is case insensitive.</summary>
        bool SearchIgnoreCase { get; set; }

        /// <summary>Gets or sets a value indicating a fixed class name for all generated classes.
        /// <para>The value must be not null and not an empty string to take effect.</para></summary>
        string FixedClassName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating a prefix string to be added to the start of the class name all generated classes.
        /// <para>The value must be not null and not an empty string to take effect.</para></summary>
        string Prefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating a suffix string to be added to the end of the class name all generated classes.
        /// <para>The value must be not null and not an empty string to take effect.</para></summary>
        string Suffix { get; set; }
    }

    #endregion

    #region Navigation Properties

    /// <summary>The settings for the navigation properties.</summary>
    public interface NavigationProperties
    {
        /// <summary>Resets the navigation properties settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to enable navigation properties.</summary>
        bool Enable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether navigation property methods will be modified to be virtual methods.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool VirtualNavigationProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether navigation property methods will be modified to be override methods.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool OverrideNavigationProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate a join table in a many-to-many relationship. In a many-to-many relationship, the join table is hidden by default.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool ManyToManyJoinTable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate a comment of the original SQL Server foreign key.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate navigation properties as <see cref="List{T}" />.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>. The default value is <c>true</c>.</para></summary>
        bool ListNavigationProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate navigation properties as <see cref="IList{T}" />.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>. The default value is <c>true</c>.</para></summary>
        bool IListNavigationProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate navigation properties as <see cref="ICollection{T}" />.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>. The default value is <c>false</c>.</para></summary>
        bool ICollectionNavigationProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate navigation properties as <see cref="IEnumerable{T}" />.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>. The default value is <c>false</c>.</para></summary>
        bool IEnumerableNavigationProperties { get; set; }
    }

    #endregion

    #region EF Annotations

    /// <summary>The settings for the Entity Framework annotations.</summary>
    public interface EFAnnotations
    {
        /// <summary>Resets the Entity Framework annotations settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether Table, Key, MaxLength, Timestamp and DatabaseGenerated attributes are added to data members.</summary>
        bool Enable { get; set; }

        /// <summary>Gets or sets a value indicating whether Column attribute is added to data members.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool Column { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Required attribute is added to data members that are not nullable.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Required attribute, with an error message, is added to data members that are not nullable.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool RequiredWithErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ConcurrencyCheck attribute is added to Timestamp and RowVersion data members.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool ConcurrencyCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether StringLength attribute is added to string data members.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool StringLength { get; set; }

        /// <summary>Gets or sets a value indicating whether Display attribute is added to data members.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool Display { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Description attribute is added to data members.
        /// <para>The description is taken from SQL Server's extended properties (MS_Description) and MySQL's comment columns.
        /// This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to group properties into a ComplexType based on the first underscore in their SQL column name.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool ComplexType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Index attribute is added to data members. Index attribute is applicable for EF6 and above.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool Index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ForeignKey and InverseProperty attributes are added to navigation properties.
        /// <para>This setting is applicable only when <see cref="Enable" /> is set to <c>true</c>.</para></summary>
        bool ForeignKeyAndInverseProperty { get; set; }
    }

    #endregion

    #region Database Objects

    /// <summary>The settings determine which database objects to generate classes out of and which database objects to exclude from generating classes.</summary>
    public interface DatabaseObjects
    {
        /// <summary>Resets the database objects settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to generate classes out of all database objects.</summary>
        bool IncludeAll { get; set; }

        /// <summary>Gets the settings that determine which tables to generate classes out of and which tables to exclude from generating classes.</summary>
        Tables Tables { get; }

        /// <summary>Gets the settings that determine which views to generate classes out of and which views to exclude from generating classes.</summary>
        Views Views { get; }

        /// <summary>Gets the settings that determine which stored procedures to generate classes out of and which stored procedures to exclude from generating classes.</summary>
        StoredProcedures StoredProcedures { get; }

        /// <summary>Gets the settings that determine which functions to generate classes out of and which functions to exclude from generating classes.</summary>
        Functions Functions { get; }

        /// <summary>Gets the settings that determine which TVPs to generate classes out of and which TVPs to exclude from generating classes.</summary>
        TVPs TVPs { get; }
    }

    #endregion

    #region Tables

    /// <summary>The settings determine which tables to generate classes out of and which tables to exclude from generating classes.</summary>
    public interface Tables
    {
        /// <summary>Resets the tables settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to include all tables to generated classes.</summary>
        bool IncludeAll { get; set; }

        /// <summary>Gets or sets a value indicating whether to exclude all tables from generated classes.</summary>
        bool ExcludeAll { get; set; }

        /// <summary>List of tables to include to generated classes.</summary>
        IList<string> Include { get; }

        /// <summary>List of tables to exclude from generated classes.</summary>
        IList<string> Exclude { get; }
    }

    #endregion

    #region Views

    /// <summary>The settings determine which views to generate classes out of and which views to exclude from generating classes.</summary>
    public interface Views
    {
        /// <summary>Resets the views settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to include all views to generated classes.</summary>
        bool IncludeAll { get; set; }

        /// <summary>Gets or sets a value indicating whether to exclude all views from generated classes.</summary>
        bool ExcludeAll { get; set; }

        /// <summary>List of views to include to generated classes.</summary>
        IList<string> Include { get; }

        /// <summary>List of views to exclude from generated classes.</summary>
        IList<string> Exclude { get; }
    }

    #endregion

    #region Stored Procedures

    /// <summary>The settings determine which stored procedures to generate classes out of and which stored procedures to exclude from generating classes.</summary>
    public interface StoredProcedures
    {
        /// <summary>Resets the stored procedures settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to include all stored procedures to generated classes.</summary>
        bool IncludeAll { get; set; }

        /// <summary>Gets or sets a value indicating whether to exclude all stored procedures from generated classes.</summary>
        bool ExcludeAll { get; set; }

        /// <summary>List of stored procedures to include to generated classes.</summary>
        IList<string> Include { get; }

        /// <summary>List of stored procedures to exclude from generated classes.</summary>
        IList<string> Exclude { get; }
    }

    #endregion

    #region Functions

    /// <summary>The settings determine which functions to generate classes out of and which functions to exclude from generating classes.</summary>
    public interface Functions
    {
        /// <summary>Resets the functions settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to include all functions to generated classes.</summary>
        bool IncludeAll { get; set; }

        /// <summary>Gets or sets a value indicating whether to exclude all functions from generated classes.</summary>
        bool ExcludeAll { get; set; }

        /// <summary>List of functions to include to generated classes.</summary>
        IList<string> Include { get; }

        /// <summary>List of functions to exclude from generated classes.</summary>
        IList<string> Exclude { get; }
    }

    #endregion

    #region TVPs

    /// <summary>The settings determine which TVPs to generate classes out of and which TVPs to exclude from generating classes.</summary>
    public interface TVPs
    {
        /// <summary>Resets the TVPs settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets a value indicating whether to include all TVPs to generated classes.</summary>
        bool IncludeAll { get; set; }

        /// <summary>Gets or sets a value indicating whether to exclude all TVPs from generated classes.</summary>
        bool ExcludeAll { get; set; }

        /// <summary>List of TVPs to include to generated classes.</summary>
        IList<string> Include { get; }

        /// <summary>List of TVPs to exclude from generated classes.</summary>
        IList<string> Exclude { get; }
    }

    #endregion

    #region Syntax Highlight

    /// <summary>The settings determine the colors for syntax elements.</summary>
    public interface SyntaxHighlight
    {
        /// <summary>Resets the syntax highlight settings to their default values.</summary>
        void Reset();

        /// <summary>Gets or sets the color for a text (foreground)
        /// that is not keyword, user type, string, comment or an error.
        /// <para>The default color is #000000.</para></summary>
        Color Text { get; set; }

        /// <summary>Gets or sets the color for a C# keyword.
        /// <para>The default color is #0000FF.</para></summary>
        Color Keyword { get; set; }

        /// <summary>Gets or sets the color for a user type.
        /// <para>The default color is #2B91AF.</para></summary>
        Color UserType { get; set; }

        /// <summary>Gets or sets the color for a string.
        /// <para>The default color is #A31515.</para></summary>
        Color String { get; set; }

        /// <summary>Gets or sets the color for a comment.
        /// <para>The default color is #008000.</para></summary>
        Color Comment { get; set; }

        /// <summary>Gets or sets the color for an error.
        /// <para>The default color is #FF0000.</para></summary>
        Color Error { get; set; }

        /// <summary>Gets or sets the background color.
        /// <para>The default color is #FFFFFF.</para></summary>
        Color Background { get; set; }
    }

    #endregion
}
