using System;

namespace DBMySql
{
    public static class MySQLDBCommon
    {
        public static byte EmptyByte => byte.MinValue;
        public static int EmptyInt => int.MinValue;
        public static long EmptyLong => long.MinValue;
        public static double EmptyDouble => double.MinValue;
        public static float EmptyFloat => float.MinValue;
        public static decimal EmptyDecimal => decimal.MinValue;
        public static DateTime EmptyDateTime => DateTime.MinValue;
        public static char EmptyChar => char.MinValue;
        public static string EmptyString => string.Empty;

        public static string SetValueForSql(byte value) => value == EmptyByte ? "NULL" : value.ToString();
        public static string SetValueForSql(int value) => value == EmptyInt ? "NULL" : value.ToString();
        public static string SetValueForSql(long value) => value == EmptyLong ? "NULL" : value.ToString();
        public static string SetValueForSql(double value) => value == EmptyDouble ? "NULL" : value.ToString();
        public static string SetValueForSql(float value) => value == EmptyFloat ? "NULL" : value.ToString();
        public static string SetValueForSql(decimal value) => value == EmptyDecimal ? "NULL" : value.ToString();
        public static string SetValueForSql(DateTime value) => value == EmptyDateTime ? "NULL" : string.Format("'{0}'", value.ToString("yyyy-MM-dd hh:mm:ss"));
        public static string SetValueForSql(char value) => value == EmptyChar ? "NULL" : $"'{value}'";
        public static string SetValueForSql(string value) => value == EmptyString ? "NULL" : $"'{value}'";
        public static string SetValueForSql(bool value) => value ? "1" : "0";

        public static string SetValueForSql(object value)
        {
            if (value is byte)
                return SetValueForSql((byte)value);
            if (value is int)
                return SetValueForSql((int)value);
            if (value is long)
                return SetValueForSql((long)value);
            if (value is double)
                return SetValueForSql((double)value);
            if (value is float)
                return SetValueForSql((float)value);
            if (value is decimal)
                return SetValueForSql((decimal)value);
            if (value is DateTime)
                return SetValueForSql((DateTime)value);
            if (value is char)
                return SetValueForSql((char)value);
            if (value is string)
                return SetValueForSql((string)value);
            if (value is bool)
                return SetValueForSql((bool)value);
            if (value is null)
                return SetValueForSql(string.Empty);

            return string.Empty;
        }

        public static byte GetValueByteFromSql(object value)
        {
            if (value is byte)
                return (byte)value;
            else if (byte.TryParse(value.ToString(), out byte result))
                return result;
            else
                return EmptyByte;
        }

        public static int GetValueIntFromSql(object value)
        {
            if (value is int)
                return (int)value;
            else if (int.TryParse(value.ToString(), out int result))
                return result;
            else
                return EmptyInt;
        }

        public static long GetValueLongFromSql(object value)
        {
            if (value is long)
                return (long)value;
            else if (long.TryParse(value.ToString(), out long result))
                return result;
            else
                return EmptyLong;
        }

        public static double GetValueDoubleFromSql(object value)
        {
            if (value is double)
                return (double)value;
            else if (double.TryParse(value.ToString(), out double result))
                return result;
            else
                return EmptyDouble;
        }

        public static float GetValueFloatFromSql(object value)
        {
            if (value is float)
                return (float)value;
            else if (float.TryParse(value.ToString(), out float result))
                return result;
            else
                return EmptyFloat;
        }

        public static decimal GetValueDecimalFromSql(object value)
        {
            if (value is decimal)
                return (decimal)value;
            else if (decimal.TryParse(value.ToString(), out decimal result))
                return result;
            else
                return EmptyDecimal;
        }

        public static DateTime GetValueDateTimeFromSql(object value)
        {
            if (value is DateTime)
                return (DateTime)value;
            else if (DateTime.TryParse(value.ToString(), out DateTime result))
                return result;
            else
                return EmptyDateTime;
        }

        public static char GetValueCharFromSql(object value)
        {
            if (value is char)
                return (char)value;
            else if (char.TryParse(value.ToString(), out char result))
                return result;
            else
                return EmptyChar;
        }

        public static bool GetValueBoolFromSql(object value)
        {
            if (value is bool)
                return (bool)value;
            else if (bool.TryParse(value.ToString(), out bool result))
                return result;
            else
                return false;
        }

        public static string GetValueStringFromSql(object value) => value.ToString();
    }
}