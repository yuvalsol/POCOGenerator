using System;
using System.Data;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database parameter</summary>
    public interface IDbParameter
    {
        /// <summary>Gets the name of the parameter.</summary>
        /// <value>The name of the parameter.</value>
        string ParameterName { get; }

        /// <summary>Gets the data type of the parameter.</summary>
        /// <value>The data type of the parameter.</value>
        string ParameterDataType { get; }

        /// <summary>Gets a value indicating whether the parameter is unsigned.</summary>
        /// <value>
        ///   <c>true</c> if the parameter is unsigned; otherwise, <c>false</c>.</value>
        bool ParameterIsUnsigned { get; }

        /// <summary>Gets the ordinal position of the parameter.</summary>
        /// <value>The ordinal position of the parameter.</value>
        int? ParameterOrdinal { get; }

        /// <summary>
        /// Gets the string precision.
        /// <para>String precision is the maximum number of bytes to store characters.
        /// Returns <see langword="null" /> if the parameter data type doesn't have string precision.
        /// Returns <c>-1</c> to indicate the maximum possible string precision.</para></summary>
        /// <value>The string precision.</value>
        int? ParameterSize { get; }

        /// <summary>
        /// Gets the numeric precision.
        /// <para>Numeric precision is the number of digits in a number.
        /// Returns <see langword="null" /> if the parameter data type doesn't have numeric precision.</para></summary>
        /// <value>The numeric precision.</value>
        byte? ParameterPrecision { get; }

        /// <summary>
        /// Gets the numeric scale.
        /// <para>Numeric scale is the number of digits to the right of the decimal point in a number.
        /// Returns <see langword="null" /> if the parameter data type doesn't have numeric scale.</para></summary>
        /// <value>The numeric scale.</value>
        int? ParameterScale { get; }

        /// <summary>Gets the datetime precision.
        /// <para>Returns <see langword="null" /> if the parameter data type doesn't have datetime precision.</para></summary>
        /// <value>The datetime precision.</value>
        int? ParameterDateTimePrecision { get; }

        /// <summary>Gets a value indicating whether the parameter is an input parameter (IN) or an output parameter (OUT) or both (INOUT).</summary>
        /// <value>
        ///   <c>IN</c> if the parameter is an input parameter; <c>OUT</c> if the parameter is an output parameter; <c>INOUT</c> if the parameter is an input &amp; output parameter.</value>
        string ParameterMode { get; }

        /// <summary>Gets the type of parameter.
        /// <para>This value is depended on the value of <see cref="ParameterMode" />.</para></summary>
        /// <value>The type of parameter.</value>
        ParameterDirection ParameterDirection { get; }

        /// <summary>Gets a value indicating whether the parameter represents a return value.
        /// <para>If <c>true</c>, this value is equivalent to <see cref="ParameterDirection.ReturnValue" />.</para></summary>
        /// <value>
        ///   <c>true</c> if the parameter represents a return value; otherwise, <c>false</c>.</value>
        bool IsResult { get; }

        /// <summary>Gets the description of the parameter.</summary>
        /// <value>The description of the parameter.</value>
        string Description { get; }

        /// <summary>Gets the database object that this parameter belongs to.</summary>
        /// <value>The database object that this parameter belongs to.</value>
        IDbObject DbObject { get; }

        /// <summary>Returns a string that represents this parameter.</summary>
        /// <returns>A string that represents this parameter.</returns>
        string ToString();
    }
}
