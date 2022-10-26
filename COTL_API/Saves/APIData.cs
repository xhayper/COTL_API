using System.Collections.Generic;

namespace COTL_API.Saves;

internal class APIData
{
    public Dictionary<string, object> Data = new();

    internal T GetValue<T>(string key)
    {
        Data ??= new Dictionary<string, object>();

        if (!Data.ContainsKey(key))
            Data.Add(key, default(T));

        return Data[key] is T res ? res : default;
    }

    internal string GetValueAsString(string key)
    {
        return GetValue<string>(key);
    }

    internal int GetValueAsInt(string key)
    {
        long value = GetValueAsLong(key);

        return value switch {
            > int.MaxValue => int.MaxValue,
            < int.MinValue => int.MinValue,
            _ => (int)value
        };
    }

    internal long GetValueAsLong(string key)
    {
        return GetValue<long>(key);
    }

    internal float GetValueAsFloat(string key)
    {
        return GetValue<float>(key);
    }

    internal bool GetValueAsBoolean(string key)
    {
        return GetValue<bool>(key);
    }

    internal void SetValue<T>(string key, T value)
    {
        Data ??= new Dictionary<string, object>();

        if (!Data.ContainsKey(key))
            Data.Add(key, value);
        else
            Data[key] = value;
    }
}