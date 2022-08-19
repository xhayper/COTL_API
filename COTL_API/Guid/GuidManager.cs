using System.Collections.Generic;
using COTL_API.Saves;
using System;

namespace COTL_API.Guid;

public static class GuidManager
{
    public static string GetFullyQualifiedName(string guid, string value)
    {
        return $"{guid}_{value}";
    }

    private static readonly Dictionary<int, Type> reverseMapper = new();

    private static readonly object lockObject = new object();

    public const int START_INDEX = 5000;

    public const string MAX_DATA = "maximumStoredValueForEnum";

    public static Type GetEnumType(int number)
    {
        reverseMapper.TryGetValue(number, out var res);
        return res;
    }

    unsafe public static List<T> GetValues<T>() where T : unmanaged, System.Enum
    {
        List<T> itemList = new();
        foreach(T item in Enum.GetValues(typeof(T)))
            itemList.Add(item);

        string startKey = typeof(T).Name + "_";
        foreach (var item in ModdedSaveManager.SaveData.SaveData[COTL_API.Plugin.PLUGIN_GUID])
        {
            if (item.Key.StartsWith(startKey))
            {
                int enumVal = int.Parse((string)item.Value);
                T convertedEnumVal = *(T*)&enumVal;
                itemList.Add(convertedEnumVal);
            }
        }
        
        return itemList;
    }

    unsafe public static T GetEnumValue<T>(string guid, string value) where T : unmanaged, System.Enum
    {
        if (sizeof(T) != sizeof(int))
            throw new NotSupportedException($"Cannot manage values of type {typeof(T).Name} in GuidManager.GetEnumValue");        

        string saveKey = $"{typeof(T).Name}_{guid}_{value}";
            
        int enumValue = ModdedSaveManager.SaveData.GetValueAsInt(COTL_API.Plugin.PLUGIN_GUID, saveKey);

        if (enumValue == default)
        {
            lock (lockObject)
            {
                enumValue = ModdedSaveManager.SaveData.GetValueAsInt(COTL_API.Plugin.PLUGIN_GUID, MAX_DATA);
                if (enumValue < START_INDEX)
                    enumValue = START_INDEX;
                
                ModdedSaveManager.SaveData.SetValue(COTL_API.Plugin.PLUGIN_GUID, MAX_DATA, enumValue+1);
                ModdedSaveManager.SaveData.SetValue(COTL_API.Plugin.PLUGIN_GUID, saveKey, enumValue);
            }
        }

        reverseMapper[enumValue] = typeof(T);

        return *(T*)&enumValue;
    }
}