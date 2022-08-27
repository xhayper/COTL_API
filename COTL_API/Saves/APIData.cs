using System.Collections.Generic;

namespace COTL_API.Saves;

internal class APIData
{
    internal Dictionary<string, object> data = new();

    internal T GetValue<T>(string key)
    {
        data ??= new Dictionary<string, object>();

        if (!data.ContainsKey(key))
            data.Add(key, default(T));

        return data[key] is T res ? res : default;
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
        data ??= new Dictionary<string, object>();

        if (!data.ContainsKey(key))
            data.Add(key, value);
        else
            data[key] = value;
    }
}