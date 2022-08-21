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
        foreach (T item in Enum.GetValues(typeof(T)))
            itemList.Add(item);

        string startKey = typeof(T).Name + "_";
        foreach (var item in APIDataManager.apiData.data)
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

        int enumValue = APIDataManager.apiData.GetValueAsInt(saveKey);

        Plugin.logger.LogInfo($"{saveKey} = {enumValue}");

        if (enumValue == default)
        {
            enumValue = APIDataManager.apiData.GetValueAsInt(MAX_DATA);
            if (enumValue < START_INDEX)
                enumValue = START_INDEX;

            Plugin.logger.LogInfo($"{MAX_DATA} = {enumValue}");

            APIDataManager.apiData.SetValue<long>(MAX_DATA, enumValue + 1);
            APIDataManager.apiData.SetValue<long>(saveKey, enumValue);

            Plugin.logger.LogInfo($"{MAX_DATA} = {APIDataManager.apiData.GetValueAsInt(MAX_DATA)}");

            APIDataManager.Save();
        }

        reverseMapper[enumValue] = typeof(T);

        return *(T*)&enumValue;
    }
}