using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data
{
    public static partial class DataTableExtensions
    {
        public delegate object ValueHandler(string columnName, Type columnType, object value);

        public static IEnumerable<T> Cast<T>(this DataTable table, ValueHandler getValue = null) where T : new()
        {
            return ToEnumerable<T>(table, () => new T(), getValue);
        }

        public static IEnumerable<T> Cast<T>(this DataTable table, Func<T> instanceHandler, ValueHandler getValue = null)
        {
            return ToEnumerable<T>(table, instanceHandler, getValue);
        }

        public static T[] ToArray<T>(this DataTable table, ValueHandler getValue = null) where T : new()
        {
            return ToEnumerable<T>(table, () => new T(), getValue);
        }

        public static T[] ToArray<T>(this DataTable table, Func<T> instanceHandler, ValueHandler getValue = null)
        {
            return ToEnumerable<T>(table, instanceHandler, getValue);
        }

        public static List<T> ToList<T>(this DataTable table, ValueHandler getValue = null) where T : new()
        {
            return ToEnumerable<T>(table, () => new T(), getValue).ToList<T>();
        }

        public static List<T> ToList<T>(this DataTable table, Func<T> instanceHandler, ValueHandler getValue = null)
        {
            return ToEnumerable<T>(table, instanceHandler, getValue).ToList<T>();
        }

        private static T[] ToEnumerable<T>(DataTable table, Func<T> instanceHandler, ValueHandler getValue)
        {
            if (table == null)
                return null;

            if (table.Rows.Count == 0)
                return new T[0];

            Type type = typeof(T);

            var columns =
                type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f => new
                    {
                        ColumnName = f.Name,
                        ColumnType = f.FieldType,
                        IsField = true,
                        MemberInfo = (MemberInfo)f
                    })
                    .Union(
                        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanWrite)
                            .Where(p => p.CanRead)
                            .Where(p => p.GetGetMethod(true).IsPublic)
                            .Where(p => p.GetIndexParameters().Length == 0)
                            .Select(p => new
                            {
                                ColumnName = p.Name,
                                ColumnType = p.PropertyType,
                                IsField = false,
                                MemberInfo = (MemberInfo)p
                            })
                    )
                    .Where(c => table.Columns.Contains(c.ColumnName)); // columns exist

            T[] instances = new T[table.Rows.Count];

            int index = 0;
            foreach (DataRow row in table.Rows)
            {
                T instance = instanceHandler();

                foreach (var column in columns)
                {
                    object value = row[column.ColumnName];
                    if (getValue != null)
                        value = getValue(column.ColumnName, column.ColumnType, value);

                    if (value is DBNull)
                    {
                        value = null;
                    }
                    else if (value != null && column.ColumnType != typeof(System.Type))
                    {
                        if (value.GetType() != column.ColumnType)
                        {
                            if (column.ColumnType.IsGenericType && column.ColumnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                if (value.GetType() != Nullable.GetUnderlyingType(column.ColumnType))
                                    value = Convert.ChangeType(value, Nullable.GetUnderlyingType(column.ColumnType));
                            }
                            else
                            {
                                value = Convert.ChangeType(value, column.ColumnType);
                            }
                        }
                    }

                    if (column.IsField)
                        ((FieldInfo)column.MemberInfo).SetValue(instance, value);
                    else
                        ((PropertyInfo)column.MemberInfo).SetValue(instance, value, null);
                }

                instances[index++] = instance;
            }

            return instances;
        }
    }
}