using System.Collections.Generic;

namespace COTL_API.Saves;

public class ModdedSaveData
{
    public Dictionary<string, Dictionary<string, object>> SaveData = new();

    public T GetValue<T>(string guid, string key)
    {
        if (SaveData == null)
            SaveData = new();

        if (!SaveData.ContainsKey(guid))
            SaveData.Add(guid, new());

        if (!SaveData[guid].ContainsKey(key))
            SaveData[guid].Add(key, default(T));

        return SaveData[guid][key] is T res ? res : default(T);
    }

    public string GetValueAsString(string guid, string key)
    {
        return GetValue<string>(guid, key);
    }

    public int GetValueAsInt(string guid, string key)
    {
        return GetValue<int>(guid, key);
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
        if (SaveData == null)
            SaveData = new();

        if (!SaveData.ContainsKey(guid))
            SaveData.Add(guid, new());

        if (!SaveData[guid].ContainsKey(key))
            SaveData[guid].Add(key, value);
        else
            SaveData[guid][key] = value;
    }
}