﻿using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DBMySql;

#region Attributes

public class TableFieldNameAttribute : Attribute
{
    public string FieldName { get; }

    internal TableFieldNameAttribute(string fieldName)
    {
        if (FieldName == null) FieldName = string.Empty;

        FieldName = fieldName;
    }
}

public class TableFieldExcludeFromUpdateAttribute : Attribute
{
    public bool ExcludeFromUpdate { get; }

    internal TableFieldExcludeFromUpdateAttribute(bool excludeFromUpdate)
    {
        ExcludeFromUpdate = excludeFromUpdate;
    }
}

public class TableFieldExcludeFromInsertAttribute : Attribute
{
    public bool ExcludeFromInsert { get; }

    internal TableFieldExcludeFromInsertAttribute(bool excludeFromInsert)
    {
        ExcludeFromInsert = excludeFromInsert;
    }
}

public class TableNameAttribute : Attribute
{
    public string TableName { get; }

    internal TableNameAttribute(string tableName)
    {
        if (TableName == null) TableName = string.Empty;

        TableName = tableName;
    }
}

#endregion

#region Class Extensions
public static class ClassExtension
{
    /// <summary>
    /// Returns an empty string if the [TableName] attribute isn't added to the property
    /// </summary>
    public static string TableName(this IDatabaseModel value)
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
    /// <summary>
    /// Returns an empty string if the [TableFieldName] attribute isn't added to the property
    /// </summary>
    public static string TableField(this int value)
    {
        return value.GetType()
                        .GetMember(value.ToString())
                        .First()
                        .GetCustomAttribute<TableFieldNameAttribute>()
                        .FieldName;
    }

    public static string ExcludeFromUpdate(this bool value)
    {
        return value.GetType()
                        .GetMember(value.ToString())
                        .First()
                        .GetCustomAttribute<TableFieldNameAttribute>()
                        .FieldName;
    }

    public static bool IsEmpty(this int value) => value == MySQLDBCommon.EmptyInt ? true : false;
}
#endregion

#region String Extensions

public static class StringExtension
{
    /// <summary>
    /// Returns an empty string if the [TableFieldName] attribute isn't added to the property
    /// </summary>
    public static string TableField(this string value)
    {
        return value.GetType()
                        .GetMember(value)
                        .First()
                        .GetCustomAttribute<TableFieldNameAttribute>()
                        .FieldName;
    }

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

    public static bool IsEmpty(this String value) => value == MySQLDBCommon.EmptyString ? true : false;
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
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetDescription();
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