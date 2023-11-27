using System;
using POCOGenerator;

namespace POCOGeneratorUI
{
    [Serializable]
    internal class UISettings
    {
        public RDBMS RDBMS { get; set; }
        public string ConnectionString { get; set; }

        public bool SupportSchema { get; set; }
        public bool SupportTVPs { get; set; }
        public bool SupportEnumDataType { get; set; }

        public bool dbObjectsForm_IsEnableTables { get; set; }
        public bool dbObjectsForm_IsEnableViews { get; set; }
        public bool dbObjectsForm_IsEnableProcedures { get; set; }
        public bool dbObjectsForm_IsEnableFunctions { get; set; }
        public bool dbObjectsForm_IsEnableTVPs { get; set; }

        // POCO
        public bool rdbProperties_Checked { get; set; }
        public bool rdbFields_Checked { get; set; }
        public bool chkVirtualProperties_Checked { get; set; }
        public bool chkOverrideProperties_Checked { get; set; }
        public bool chkPartialClass_Checked { get; set; }
        public bool chkStructTypesNullable_Checked { get; set; }
        public bool chkComments_Checked { get; set; }
        public bool chkCommentsWithoutNull_Checked { get; set; }
        public bool chkUsing_Checked { get; set; }
        public bool chkUsingInsideNamespace_Checked { get; set; }
        public string txtNamespace_Text { get; set; }
        public string txtInherit_Text { get; set; }
        public bool chkColumnDefaults_Checked { get; set; }
        public bool chkNewLineBetweenMembers_Checked { get; set; }
        public bool chkComplexTypes_Checked { get; set; }
        public bool rdbEnumSQLTypeToString_Checked { get; set; }
        public bool rdbEnumSQLTypeToEnumUShort_Checked { get; set; }
        public bool rdbEnumSQLTypeToEnumInt_Checked { get; set; }

        // Navigation Properties
        public bool chkNavigationProperties_Checked { get; set; }
        public bool chkVirtualNavigationProperties_Checked { get; set; }
        public bool chkOverrideNavigationProperties_Checked { get; set; }
        public bool chkManyToManyJoinTable_Checked { get; set; }
        public bool chkNavigationPropertiesComments_Checked { get; set; }
        public bool rdbListNavigationProperties_Checked { get; set; }
        public bool rdbIListNavigationProperties_Checked { get; set; }
        public bool rdbICollectionNavigationProperties_Checked { get; set; }
        public bool rdbIEnumerableNavigationProperties_Checked { get; set; }

        // Class Name
        public bool chkSingular_Checked { get; set; }
        public bool chkIncludeDB_Checked { get; set; }
        public string txtDBSeparator_Text { get; set; }
        public bool chkIncludeSchema_Checked { get; set; }
        public bool chkIgnoreDboSchema_Checked { get; set; }
        public string txtSchemaSeparator_Text { get; set; }
        public string txtWordsSeparator_Text { get; set; }
        public bool chkCamelCase_Checked { get; set; }
        public bool chkUpperCase_Checked { get; set; }
        public bool chkLowerCase_Checked { get; set; }
        public string txtSearch_Text { get; set; }
        public string txtReplace_Text { get; set; }
        public bool chkSearchIgnoreCase_Checked { get; set; }
        public string txtFixedClassName_Text { get; set; }
        public string txtPrefix_Text { get; set; }
        public string txtSuffix_Text { get; set; }

        // EF Annotations
        public bool chkEF_Checked { get; set; }
        public bool chkEFColumn_Checked { get; set; }
        public bool chkEFRequired_Checked { get; set; }
        public bool chkEFRequiredWithErrorMessage_Checked { get; set; }
        public bool chkEFConcurrencyCheck_Checked { get; set; }
        public bool chkEFStringLength_Checked { get; set; }
        public bool chkEFDisplay_Checked { get; set; }
        public bool chkEFDescription_Checked { get; set; }
        public bool chkEFComplexType_Checked { get; set; }
        public bool chkEFIndex_Checked { get; set; }
        public bool chkEFForeignKeyAndInverseProperty_Checked { get; set; }

        // Export To Files
        public string txtFolder_Text { get; set; }
        public bool rdbSingleFile_Checked { get; set; }
        public bool rdbMultipleFilesSingleFolder_Checked { get; set; }
        public bool rdbMultipleFilesRelativeFolders_Checked { get; set; }
        public bool rdbFileNameName_Checked { get; set; }
        public bool rdbFileNameSchemaName_Checked { get; set; }
        public bool rdbFileNameDatabaseName_Checked { get; set; }
        public bool rdbFileNameDatabaseSchemaName_Checked { get; set; }
    }
}
