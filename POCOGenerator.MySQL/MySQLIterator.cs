using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using POCOGenerator.DbObjects;
using POCOGenerator.POCOIterators;
using POCOGenerator.POCOWriters;
using POCOGenerator.Utils;

namespace POCOGenerator.MySQL
{
    internal class MySQLIterator : DbIterator
    {
        #region Constructor

        public MySQLIterator(IWriter writer, IDbSupport support, IDbIteratorSettings settings)
            : base(writer, support, settings)
        {
        }

        #endregion

        #region Using

        protected override void WriteSpecialSQLTypesUsingClause(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.WriteKeyword("using");
            writer.WriteLine(" System.Data.Spatial;");
        }

        #endregion

        #region Class Constructor - Column Defaults

        protected override bool IsSQLTypeMappedToBool(string dataTypeName, bool isUnsigned, int? numericPrecision)
        {
            return (
                dataTypeName == "bit" ||
                (dataTypeName == "tinyint" && isUnsigned == false && numericPrecision == 1) ||
                dataTypeName == "bool" ||
                dataTypeName == "boolean"
            );
        }

        protected override bool IsSQLTypeMappedToBoolTrue(string dataTypeName, bool isUnsigned, int? numericPrecision, string cleanColumnDefault)
        {
            return
                IsSQLTypeMappedToBool(dataTypeName, isUnsigned, numericPrecision) &&
                (cleanColumnDefault.IndexOf("1", StringComparison.OrdinalIgnoreCase) != -1); // b'1'
        }

        protected override bool IsSQLTypeMappedToBoolFalse(string dataTypeName, bool isUnsigned, int? numericPrecision, string cleanColumnDefault)
        {
            return
                IsSQLTypeMappedToBool(dataTypeName, isUnsigned, numericPrecision) &&
                (cleanColumnDefault.IndexOf("0", StringComparison.OrdinalIgnoreCase) != -1); // b'0'
        }

        protected override bool IsSQLTypeMappedToByte(string dataTypeName, bool isUnsigned, int? numericPrecision)
        {
            return (dataTypeName == "tinyint" && isUnsigned);
        }

        protected override bool IsSQLTypeMappedToSByte(string dataTypeName, bool isUnsigned, int? numericPrecision)
        {
            return (dataTypeName == "tinyint" && isUnsigned == false && numericPrecision != 1);
        }

        protected override bool IsSQLTypeMappedToShort(string dataTypeName, bool isUnsigned, int? numericPrecision)
        {
            return (
                (dataTypeName == "smallint" && isUnsigned == false) ||
                dataTypeName == "year");
        }

        protected override bool IsSQLTypeMappedToInt(string dataTypeName, bool isUnsigned, int? numericPrecision)
        {
            return (
                (dataTypeName == "int" && isUnsigned == false) ||
                (dataTypeName == "integer" && isUnsigned == false) ||
                dataTypeName == "mediumint" || // same for unsigned
                (dataTypeName == "smallint" && isUnsigned)
            );
        }

        protected override bool IsSQLTypeMappedToLong(string dataTypeName, bool isUnsigned, int? numericPrecision)
        {
            return (
                (dataTypeName == "bigint" && isUnsigned == false) ||
                (dataTypeName == "int" && isUnsigned) ||
                (dataTypeName == "integer" && isUnsigned)
            );
        }

        protected override bool IsSQLTypeMappedToFloat(string dataTypeName, bool isUnsigned, int? numericPrecision, int? numericScale)
        {
            return (
                (dataTypeName == "float" && isUnsigned == false) ||
                (dataTypeName == "real" && support["REAL_AS_FLOAT"] && isUnsigned == false)
            );
        }

        protected override bool IsSQLTypeMappedToDouble(string dataTypeName, bool isUnsigned, int? numericPrecision, int? numericScale)
        {
            return (
                (dataTypeName == "double" && isUnsigned == false) ||
                (dataTypeName == "real" && support["REAL_AS_FLOAT"] == false && isUnsigned == false)
            );
        }

        protected override bool IsSQLTypeMappedToDecimal(string dataTypeName, bool isUnsigned, int? numericPrecision, int? numericScale)
        {
            return (
                dataTypeName == "decimal" ||
                dataTypeName == "numeric" ||
                dataTypeName == "dec" ||
                dataTypeName == "fixed" ||
                (dataTypeName == "double" && isUnsigned) ||
                (dataTypeName == "float" && isUnsigned) ||
                (dataTypeName == "real" && isUnsigned) || // same for REAL_AS_FLOAT on & off
                (dataTypeName == "bigint" && isUnsigned) ||
                (dataTypeName == "serial")
            );
        }

        protected override bool IsSQLTypeMappedToDateTime(string dataTypeName, int? dateTimePrecision)
        {
            return (
                dataTypeName == "date" ||
                dataTypeName == "datetime" ||
                dataTypeName == "timestamp"
            );
        }

        protected override bool IsSQLTypeMappedToTimeSpan(string dataTypeName, int? dateTimePrecision)
        {
            return (dataTypeName == "time");
        }

        protected override bool IsColumnDefaultNow(string cleanColumnDefault)
        {
            return (
                cleanColumnDefault.IndexOf("CURRENT_TIMESTAMP", StringComparison.OrdinalIgnoreCase) != -1 ||
                cleanColumnDefault.IndexOf("NOW", StringComparison.OrdinalIgnoreCase) != -1 ||
                cleanColumnDefault.IndexOf("LOCALTIME", StringComparison.OrdinalIgnoreCase) != -1 ||
                cleanColumnDefault.IndexOf("LOCALTIMESTAMP", StringComparison.OrdinalIgnoreCase) != -1
            );
        }

        protected override bool IsSQLTypeMappedToString(string dataTypeName, int? stringPrecision)
        {
            return (
                dataTypeName == "char" ||
                dataTypeName == "character" ||
                dataTypeName == "varchar" ||
                dataTypeName == "character varying" ||
                dataTypeName == "nchar" ||
                dataTypeName == "national char" ||
                dataTypeName == "nvarchar" ||
                dataTypeName == "national varchar" ||
                dataTypeName == "text" ||
                dataTypeName == "tinytext" ||
                dataTypeName == "mediumtext" ||
                dataTypeName == "longtext" ||
                dataTypeName == "enum" ||
                dataTypeName == "set" ||
                dataTypeName == "json"
            );
        }

        protected override bool IsSQLTypeMappedToByteArray(string dataTypeName)
        {
            return (
                dataTypeName == "binary" ||
                dataTypeName == "char byte" ||
                dataTypeName == "varbinary" ||
                dataTypeName == "blob" ||
                dataTypeName == "tinyblob" ||
                dataTypeName == "mediumblob" ||
                dataTypeName == "longblob"
            );
        }

        protected override bool IsSQLTypeRDBMSSpecificType(string dataTypeName)
        {
            return IsSQLTypeMappedToDbGeometry(dataTypeName);
        }

        protected virtual bool IsSQLTypeMappedToDbGeometry(string dataTypeName)
        {
            return (
                dataTypeName == "geometry" ||
                dataTypeName == "geometrycollection" ||
                dataTypeName == "geomcollection" ||
                dataTypeName == "linestring" ||
                dataTypeName == "multilinestring" ||
                dataTypeName == "point" ||
                dataTypeName == "multipoint" ||
                dataTypeName == "polygon" ||
                dataTypeName == "multipolygon"
            );
        }

        protected override List<ITableColumn> GetTableColumnsWithColumnDefaultConstructor(IDbObjectTraverse dbObject)
        {
            // MySQL doesn't differentiate between NULL and ""
            // it returns "" for NULL
            return GetColumnDefaults_NotNullOrEmpty(dbObject);
        }

        protected override void WriteColumnDefaultConstructorInitializationString(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            // MySQL doesn't differentiate between NULL and ""
            // it returns "" for NULL
            if (string.IsNullOrEmpty(columnDefault) == false)
                base.WriteColumnDefaultConstructorInitializationString(columnDefault, column, namespaceOffset);
        }

        protected override void WriteColumnDefaultConstructorInitializationByteArray(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationByteArray_String(columnDefault, column, namespaceOffset);
        }

        protected override void WriteColumnDefaultConstructorInitializationRDBMSSpecificType(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            string dataTypeName = (column.DataTypeName ?? string.Empty).ToLower();
            if (IsSQLTypeMappedToDbGeometry(dataTypeName))
                WriteColumnDefaultConstructorInitializationDbGeometry(dataTypeName, columnDefault, column, namespaceOffset);
            else
                WriteColumnDefaultConstructorInitializationComment(column.ColumnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDbGeometry(string dataTypeName, string columnDefault, ITableColumn column, string namespaceOffset)
        {
            bool isComment = false;
            if (dataTypeName == "geometry")
                try { DbGeometry.FromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "geometrycollection" || dataTypeName == "geomcollection")
                try { DbGeometry.GeometryCollectionFromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "linestring")
                try { DbGeometry.LineFromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "multilinestring")
                try { DbGeometry.MultiLineFromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "point")
                try { DbGeometry.PointFromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "multipoint")
                try { DbGeometry.MultiPointFromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "polygon")
                try { DbGeometry.PolygonFromText(columnDefault, 0); }
                catch { isComment = true; }
            else if (dataTypeName == "multipolygon")
                try { DbGeometry.MultiPolygonFromText(columnDefault, 0); }
                catch { isComment = true; }

            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset, isComment);

            if (isComment)
            {
                if (settings.POCOIteratorSettings.Using == false)
                    writer.WriteComment("System.Data.Spatial.");
                writer.WriteComment("DbGeometry");
                writer.WriteComment(".");

                if (dataTypeName == "geometry")
                    writer.WriteComment("FromText");
                else if (dataTypeName == "geometrycollection" || dataTypeName == "geomcollection")
                    writer.WriteComment("GeometryCollectionFromText");
                else if (dataTypeName == "linestring")
                    writer.WriteComment("LineFromText");
                else if (dataTypeName == "multilinestring")
                    writer.WriteComment("MultiLineFromText");
                else if (dataTypeName == "point")
                    writer.WriteComment("PointFromText");
                else if (dataTypeName == "multipoint")
                    writer.WriteComment("MultiPointFromText");
                else if (dataTypeName == "polygon")
                    writer.WriteComment("PolygonFromText");
                else if (dataTypeName == "multipolygon")
                    writer.WriteComment("MultiPolygonFromText");

                writer.WriteComment("(");
                writer.WriteComment("\"");
                writer.WriteComment(columnDefault.Replace("\\", "\\\\").Replace("\"", "\\\""));
                writer.WriteComment("\"");
                writer.WriteComment(", 0)");
            }
            else
            {
                if (settings.POCOIteratorSettings.Using == false)
                    writer.Write("System.Data.Spatial.");
                writer.WriteUserType("DbGeometry");
                writer.Write(".");

                if (dataTypeName == "geometry")
                    writer.Write("FromText");
                else if (dataTypeName == "geometrycollection" || dataTypeName == "geomcollection")
                    writer.Write("GeometryCollectionFromText");
                else if (dataTypeName == "linestring")
                    writer.Write("LineFromText");
                else if (dataTypeName == "multilinestring")
                    writer.Write("MultiLineFromText");
                else if (dataTypeName == "point")
                    writer.Write("PointFromText");
                else if (dataTypeName == "multipoint")
                    writer.Write("MultiPointFromText");
                else if (dataTypeName == "polygon")
                    writer.Write("PolygonFromText");
                else if (dataTypeName == "multipolygon")
                    writer.Write("MultiPolygonFromText");

                writer.Write("(");
                writer.WriteString("\"");
                writer.WriteString(columnDefault.Replace("\\", "\\\\").Replace("\"", "\\\""));
                writer.WriteString("\"");
                writer.Write(", 0)");
            }

            WriteColumnDefaultConstructorInitializationEnd(isComment);
        }

        #endregion

        #region Class Constructor - Enum Columns

        // https://dev.mysql.com/doc/refman/8.0/en/enum.html
        // https://www.careerride.com/question-20-MySQL

        protected override List<ITableColumn> GetTableColumnsWithEnumConstructor(IDbObjectTraverse dbObject, List<IEnumColumn> enumColumns)
        {
            if (settings.POCOIteratorSettings.ColumnDefaults && dbObject.DbObjectType == DbObjectType.Table)
            {
                if (enumColumns.HasAny())
                {
                    // has column default
                    var lst1 = enumColumns.Where(c => c.IsEnumDataType || c.IsSetDataType).Cast<ITableColumn>().Where(c => string.IsNullOrEmpty(c.ColumnDefault) == false);

                    // If an ENUM column is declared NOT NULL, its default value is the first element of the list of permitted values.
                    // For string types other than ENUM, the default value is the empty string. For ENUM, the default is the first enumeration value
                    var lst2 = enumColumns.Where(c => c.IsEnumDataType).Cast<ITableColumn>().Where(c => c.IsNullable == false && string.IsNullOrEmpty(c.ColumnDefault));

                    return lst1.Union(lst2).ToList();
                }
            }

            return null;
        }

        protected override void WriteEnumConstructorInitialization(ITableColumn column, string namespaceOffset)
        {
            if ((column is IEnumColumn enumColumn) == false)
                return;

            if (settings.POCOIteratorSettings.EnumSQLTypeToString == false && (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort || settings.POCOIteratorSettings.EnumSQLTypeToEnumInt))
            {
                string cleanColumnName = NameHelper.CleanName(enumColumn.Column.ColumnName);

                if (enumColumn.IsEnumDataType)
                    WriteEnumDataTypeConstructorInitialization(column, namespaceOffset, enumColumn, cleanColumnName);
                else if (enumColumn.IsSetDataType)
                    WriteSetDataTypeConstructorInitialization(column, namespaceOffset, enumColumn, cleanColumnName);
            }
            else if (settings.POCOIteratorSettings.EnumSQLTypeToString && settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort == false && settings.POCOIteratorSettings.EnumSQLTypeToEnumInt == false)
            {
                // If an ENUM column is declared NOT NULL, its default value is the first element of the list of permitted values.
                // For string types other than ENUM, the default value is the empty string. For ENUM, the default is the first enumeration value
                string literal = GetEnumDataTypeLiteralConstructorInitialization(enumColumn);
                if (string.IsNullOrEmpty(literal) == false)
                {
                    string cleanLiteral = NameHelper.CleanEnumLiteral(literal);
                    base.WriteColumnDefaultConstructorInitializationString(cleanLiteral, column, namespaceOffset);
                }
            }
        }

        protected override string GetEnumDataTypeLiteralConstructorInitialization(IEnumColumn enumColumn)
        {
            if (enumColumn.IsEnumDataType == false)
                return null;

            if ((enumColumn is ITableColumn tableColumn) == false)
                return null;

            // has column default
            if (string.IsNullOrEmpty(tableColumn.ColumnDefault) == false)
            {
                if (enumColumn.EnumLiterals.Contains(tableColumn.ColumnDefault, StringComparer.Ordinal))
                    return tableColumn.ColumnDefault;
            }
            // If an ENUM column is declared NOT NULL, its default value is the first element of the list of permitted values.
            // For string types other than ENUM, the default value is the empty string. For ENUM, the default is the first enumeration value
            else if (tableColumn.IsNullable == false && string.IsNullOrEmpty(tableColumn.ColumnDefault))
            {
                return enumColumn.EnumLiterals.FirstOrDefault();
            }

            return null;
        }

        protected override List<string> GetSetDataTypeLiteralsConstructorInitialization(IEnumColumn enumColumn)
        {
            if (enumColumn.IsSetDataType == false)
                return null;

            if ((enumColumn is ITableColumn tableColumn) == false)
                return null;

            if (string.IsNullOrEmpty(tableColumn.ColumnDefault) == false)
                return tableColumn.ColumnDefault.Split(',').Where(literal => enumColumn.EnumLiterals.Contains(literal, StringComparer.Ordinal)).ToList();

            return null;
        }

        #endregion

        #region Column Attributes

        #region EF

        protected override bool IsEFAttributeMaxLength(string dataTypeName)
        {
            return (
                dataTypeName == "char" ||
                dataTypeName == "character" ||
                dataTypeName == "varchar" ||
                dataTypeName == "character varying" ||
                dataTypeName == "nchar" ||
                dataTypeName == "national char" ||
                dataTypeName == "nvarchar" ||
                dataTypeName == "national varchar" ||
                dataTypeName == "binary" ||
                dataTypeName == "char byte" ||
                dataTypeName == "varbinary"
            );
        }

        protected override bool IsEFAttributeStringLength(string dataTypeName)
        {
            return (
                dataTypeName == "char" ||
                dataTypeName == "character" ||
                dataTypeName == "varchar" ||
                dataTypeName == "character varying" ||
                dataTypeName == "nchar" ||
                dataTypeName == "national char" ||
                dataTypeName == "nvarchar" ||
                dataTypeName == "national varchar" ||
                dataTypeName == "binary" ||
                dataTypeName == "char byte" ||
                dataTypeName == "varbinary"
            );
        }

        protected override bool IsEFAttributeTimestamp(string dataTypeName)
        {
            return (dataTypeName == "timestamp");
        }

        protected override bool IsEFAttributeConcurrencyCheck(string dataTypeName)
        {
            return (dataTypeName == "timestamp");
        }

        #endregion

        #endregion

        #region Column

        protected override void WriteDbColumnDataType(IColumn column)
        {
            if (support.IsSupportEnumDataType)
            {
                if (settings.POCOIteratorSettings.EnumSQLTypeToString == false && (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort || settings.POCOIteratorSettings.EnumSQLTypeToEnumInt))
                {
                    if (column is IEnumColumn enumColumn)
                    {
                        if (enumColumn.IsEnumDataType || enumColumn.IsSetDataType)
                        {
                            WriteColumnEnum(enumColumn);
                            return;
                        }
                    }
                }
            }

            switch ((column.DataTypeDisplay ?? string.Empty).ToLower())
            {
                case "bit":
                case "bool":
                case "boolean": WriteColumnBool(column.IsNullable); break;

                case "binary":
                case "char byte":
                case "blob":
                case "longblob":
                case "mediumblob":
                case "tinyblob":
                case "varbinary": WriteColumnByteArray(); break;

                case "date":
                case "datetime":
                case "timestamp": WriteColumnDateTime(column.IsNullable); break;

                case "decimal":
                case "numeric":
                case "dec":
                case "fixed": WriteColumnDecimal(column.IsNullable); break;

                case "double":
                    if (column.IsUnsigned)
                        WriteColumnDecimal(column.IsNullable);
                    else
                        WriteColumnDouble(column.IsNullable);
                    break;

                case "float":
                    if (column.IsUnsigned)
                        WriteColumnDecimal(column.IsNullable);
                    else
                        WriteColumnFloat(column.IsNullable);
                    break;

                case "real":
                    if (support["REAL_AS_FLOAT"])
                    {
                        if (column.IsUnsigned)
                            WriteColumnDecimal(column.IsNullable);
                        else
                            WriteColumnFloat(column.IsNullable);
                    }
                    else
                    {
                        if (column.IsUnsigned)
                            WriteColumnDecimal(column.IsNullable);
                        else
                            WriteColumnDouble(column.IsNullable);
                    }
                    break;

                case "tinyint":
                    if (column.IsUnsigned)
                    {
                        WriteColumnByte(column.IsNullable);
                    }
                    else
                    {
                        if (column.NumericPrecision == 1)
                            WriteColumnBool(column.IsNullable);
                        else
                            WriteColumnSByte(column.IsNullable);
                    }
                    break;

                case "int":
                case "integer":
                    if (column.IsUnsigned)
                        WriteColumnLong(column.IsNullable);
                    else
                        WriteColumnInt(column.IsNullable);
                    break;

                case "mediumint": WriteColumnInt(column.IsNullable); break; // same for unsigned

                case "bigint":
                    if (column.IsUnsigned)
                        WriteColumnDecimal(column.IsNullable);
                    else
                        WriteColumnLong(column.IsNullable);
                    break;

                case "serial": WriteColumnDecimal(column.IsNullable); break;

                case "smallint":
                    if (column.IsUnsigned)
                        WriteColumnInt(column.IsNullable);
                    else
                        WriteColumnShort(column.IsNullable);
                    break;

                case "year": WriteColumnShort(column.IsNullable); break;

                case "char":
                case "character":
                case "longtext":
                case "mediumtext":
                case "nchar":
                case "national char":
                case "nvarchar":
                case "national varchar":
                case "text":
                case "tinytext":
                case "varchar":
                case "character varying":
                case "json":
                case "enum":
                case "set": WriteColumnString(); break;

                case "geometry":
                case "geometrycollection":
                case "geomcollection":
                case "linestring":
                case "multilinestring":
                case "point":
                case "multipoint":
                case "polygon":
                case "multipolygon": WriteColumnDbGeometry(); break;

                case "time": WriteColumnTimeSpan(column.IsNullable); break;

                default: WriteColumnObject(); break;
            }
        }

        protected virtual void WriteColumnDbGeometry()
        {
            if (settings.POCOIteratorSettings.Using == false)
                writer.Write("System.Data.Spatial.");
            writer.WriteUserType("DbGeometry");
        }

        #endregion

        #region Enums

        protected override List<IEnumColumn> GetEnumColumns(IDbObjectTraverse dbObject)
        {
            if (support.IsSupportEnumDataType)
            {
                if (settings.POCOIteratorSettings.EnumSQLTypeToString == false && (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort || settings.POCOIteratorSettings.EnumSQLTypeToEnumInt))
                {
                    if (dbObject.Columns != null && dbObject.Columns.Any(c => c is IEnumColumn))
                    {
                        return dbObject.Columns
                            .Where(c => c is IEnumColumn)
                            .Cast<IEnumColumn>()
                            .Where(c => c.IsEnumDataType || c.IsSetDataType)
                            .OrderBy(c => c.Column.ColumnOrdinal ?? 0)
                            .ToList();
                    }
                }
                else if (settings.POCOIteratorSettings.EnumSQLTypeToString && settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort == false && settings.POCOIteratorSettings.EnumSQLTypeToEnumInt == false)
                {
                    if (dbObject.Columns != null && dbObject.Columns.Any(c => c is IEnumColumn))
                    {
                        return dbObject.Columns
                            .Where(c => c is IEnumColumn)
                            .Cast<IEnumColumn>()
                            // If an ENUM column is declared NOT NULL, its default value is the first element of the list of permitted values.
                            // For string types other than ENUM, the default value is the empty string. For ENUM, the default is the first enumeration value
                            .Where(c => c.IsEnumDataType).Cast<ITableColumn>().Where(c => c.IsNullable == false && string.IsNullOrEmpty(c.ColumnDefault))
                            .Cast<IEnumColumn>()
                            .OrderBy(c => c.Column.ColumnOrdinal ?? 0)
                            .ToList();
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
