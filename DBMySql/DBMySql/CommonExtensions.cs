using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DBMySql;
using System.ComponentModel;

#region Attributes

public class TableFieldNameAttribute : Attribute
{
    public string FieldName { get; }

    public TableFieldNameAttribute(string fieldName)
    {
        FieldName = fieldName;

        if (FieldName == null) FieldName = string.Empty;
    }
}

public class TableFieldExcludeFromUpdateAttribute : Attribute
{
    public bool ExcludeFromUpdate { get; }

    public TableFieldExcludeFromUpdateAttribute(bool excludeFromUpdate)
    {
        ExcludeFromUpdate = excludeFromUpdate;
    }
}

public class TableFieldExcludeFromInsertAttribute : Attribute
{
    public bool ExcludeFromInsert { get; }

    public TableFieldExcludeFromInsertAttribute(bool excludeFromInsert)
    {
        ExcludeFromInsert = excludeFromInsert;
    }
}

public class TableNameAttribute : Attribute
{
    public string TableName { get; }

    public TableNameAttribute(string tableName)
    {
        TableName = tableName;

        if (TableName == null) TableName = string.Empty;
    }
}

#endregion

#region Class Extensions
public static class ClassExtension
{
    /// <summary>
    /// Returns an empty string if the [TableName] attribute isn't added to the property
    /// </summary>
    public static string TableName(this DatabaseModel value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableNameAttribute>()
                        .TableName;
    }
}

#endregion

#region Integer Extensions

public static class IntegerExtension
{
    public static bool ExcludeFromUpdate(this bool value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableFieldExcludeFromUpdateAttribute>()
                        .ExcludeFromUpdate;
    }

    public static bool ExcludeFromInsert(this bool value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableFieldExcludeFromInsertAttribute>()
                        .ExcludeFromInsert;
    }

    public static bool IsEmpty(this int value) => (value == MySQLDBCommon.EmptyInt || value == 0) ? true : false;
}
#endregion

#region Long Extensions

public static class LongExtension
{
    public static bool ExcludeFromUpdate(this bool value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableFieldExcludeFromUpdateAttribute>()
                        .ExcludeFromUpdate;
    }

    public static bool ExcludeFromInsert(this bool value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableFieldExcludeFromInsertAttribute>()
                        .ExcludeFromInsert;
    }

    public static bool IsEmpty(this long value) => (value == MySQLDBCommon.EmptyLong || value == 0) ? true : false;
}
#endregion

#region String Extensions

public static class StringExtension
{ 
    public static bool ExcludeFromUpdate(this bool value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableFieldExcludeFromUpdateAttribute>()
                        .ExcludeFromUpdate;
    }

    public static bool ExcludeFromInsert(this bool value)
    {
        return value.GetType()
                        .GetCustomAttribute<TableFieldExcludeFromInsertAttribute>()
                        .ExcludeFromInsert;
    }

    public static bool IsEmpty(this String value) => (value == null || value == MySQLDBCommon.EmptyString) ? true : false;

}

#endregion

#region  Enumerations Extensions

public static class EnumerationExtension
{
    public static string Description(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DescriptionAttribute>()
                        .Description;
    }

    public static string TableFieldName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<TableFieldNameAttribute>()
                        .FieldName;
    }
}

    #endregion