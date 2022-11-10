using System.Collections.Generic;

namespace COTL_API.Saves;

public class ModdedSaveData
{
    public Dictionary<string, Dictionary<string, object>> SaveData { get; private set; } = new();

    public T GetValue<T>(string guid, string key)
    {
        SaveData ??= new Dictionary<string, Dictionary<string, object>>();

        if (!SaveData.ContainsKey(guid))
            SaveData.Add(guid, new Dictionary<string, object>());

        if (!SaveData[guid].ContainsKey(key))
            SaveData[guid].Add(key, default(T));

        return SaveData[guid][key] is T res ? res : (T)SaveData[guid][key];
    }

    public string GetValueAsString(string guid, string key)
    {
        return GetValue<string>(guid, key);
    }

    public int GetValueAsInt(string guid, string key)
    {
        long value = GetValueAsLong(guid, key);

        return value switch {
            > int.MaxValue => int.MaxValue,
            < int.MinValue => int.MinValue,
            _ => (int)value
        };
    }

    public long GetValueAsLong(string guid, string key)
    {
        return GetValue<long>(guid, key);
    }

    public float GetValueAsFloat(string guid, string key)
    {
        return GetValue<float>(guid, key);
    }

    public bool GetValueAsBoolean(string guid, string key)
    {
        return GetValue<bool>(guid, key);
    }

    public void SetValue<T>(string guid, string key, T value)
    {
        SaveData = SaveData ?? new Dictionary<string, Dictionary<string, object>>();

        if (!SaveData.ContainsKey(guid))
            SaveData.Add(guid, new Dictionary<string, object>());

        if (!SaveData[guid].ContainsKey(key))
            SaveData[guid].Add(key, value);
        else
            SaveData[guid][key] = value;
    }
}