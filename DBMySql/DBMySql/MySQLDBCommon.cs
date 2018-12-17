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
        public static string SetValueForSql(DateTime value) => value == EmptyDateTime ? "NULL" : $"'{value}'";
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

            return string.Empty;
        }

        public static byte GetValueByteFromSql(object value) => Convert.ToByte(value);
        public static int GetValueIntFromSql(object value) => Convert.ToInt32(value);
        public static long GetValueLongFromSql(object value) => Convert.ToInt64(value);
        public static double GetValueDoubleFromSql(object value) => Convert.ToDouble(value);
        public static float GetValueFloatFromSql(object value) => (float)Convert.ToDouble(value);
        public static decimal GetValueDecimalFromSql(object value) => Convert.ToDecimal(value);
        public static DateTime GetValueDateTimeFromSql(object value) => Convert.ToDateTime(value);
        public static char GetValueCharFromSql(object value) => Convert.ToChar(value);
        public static string GetValueStringFromSql(object value) => value.ToString();
        public static bool GetValueBoolFromSql(object value) => Convert.ToBoolean(value);
    }
}