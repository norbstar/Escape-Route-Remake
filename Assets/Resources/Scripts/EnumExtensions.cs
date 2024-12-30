using System;

public static class EnumExtensions
{
    public static bool HasFlag<T>(this T value, T flags) where T : Enum
    {
        var lValue = Convert.ToInt64(value);
        var lFlags = Convert.ToInt64(flags);
        return (lValue & lFlags) == lValue;
    }

    public static T Set<T>(this T value, Enum flags) where T : Enum
    {
        var lValue = Convert.ToInt64(value);
        var lFlags = Convert.ToInt64(flags);
        return (T) Enum.ToObject(typeof(T), lValue |= lFlags);
    }

    public static T Unset<T>(this T value, Enum flag) where T : Enum
    {
        var lValue = Convert.ToInt64(value);
        var lFlag = Convert.ToInt64(flag);
        return (T) Enum.ToObject(typeof(T), lValue &= ~lFlag);
    }
}
