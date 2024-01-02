using System;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database column.</summary>
    public interface IDbColumn
    {
        /// <summary>Gets the name of the column.</summary>
        /// <value>The name of the column.</value>
        string ColumnName { get; }

        /// <summary>Gets the ordinal position of the column.</summary>
        /// <value>The ordinal position of the column.</value>
        int? ColumnOrdinal { get; }

        /// <summary>Gets the name of the data type.</summary>
        /// <value>The name of the data type.</value>
        string DataTypeName { get; }

        /// <summary>Gets the pretty-print display of the data type.</summary>
        /// <value>The pretty-print display of the data type.</value>
        string DataTypeDisplay { get; }

        /// <summary>Gets the string representation of the precision, based on the column data type.
        /// <para>Returns <see langword="null" /> if the column data type doesn't have precision.</para></summary>
        /// <value>The string representation of the column precision.</value>
        string Precision { get; }

        /// <summary>
        /// Gets the string precision.
        /// <para>String precision is the maximum number of bytes to store characters.
        /// Returns <see langword="null" /> if the column data type doesn't have string precision.</para></summary>
        /// <value>The string precision.</value>
        int? StringPrecision { get; }

        /// <summary>
        /// Gets the numeric precision.
        /// <para>Numeric precision is the number of digits in a number.
        /// Returns <see langword="null" /> if the column data type doesn't have numeric precision.</para></summary>
        /// <value>The numeric precision.</value>
        int? NumericPrecision { get; }

        /// <summary>
        /// Gets the numeric scale.
        /// <para>Numeric scale is the number of digits to the right of the decimal point in a number.
        /// Returns <see langword="null" /> if the column data type doesn't have numeric scale.</para></summary>
        /// <value>The numeric scale.</value>
        int? NumericScale { get; }

        /// <summary>Gets the datetime precision.
        /// <para>Returns <see langword="null" /> if the column data type doesn't have datetime precision.</para></summary>
        /// <value>The datetime precision.</value>
        int? DateTimePrecision { get; }

        /// <summary>Gets a value indicating whether the column is unsigned.</summary>
        /// <value>
        ///   <c>true</c> if the column is unsigned; otherwise, <c>false</c>.</value>
        bool IsUnsigned { get; }

        /// <summary>Gets a value indicating whether the column is nullable.</summary>
        /// <value>
        ///   <c>true</c> if the column is nullable; otherwise, <c>false</c>.</value>
        bool IsNullable { get; }

        /// <summary>Gets a value indicating whether the column is identity column.</summary>
        /// <value>
        ///   <c>true</c> if the column is identity column; otherwise, <c>false</c>.</value>
        bool IsIdentity { get; }

        /// <summary>Gets a value indicating whether the column is computed column.</summary>
        /// <value>
        ///   <c>true</c> if the column is computed column; otherwise, <c>false</c>.</value>
        bool IsComputed { get; }

        /// <summary>Gets the database object that this column belongs to.</summary>
        /// <value>The database object that this column belongs to.</value>
        IDbObject DbObject { get; }

        /// <summary>Returns a string that represents this column.</summary>
        /// <returns>A string that represents this column.</returns>
        string ToString();
    }
}
