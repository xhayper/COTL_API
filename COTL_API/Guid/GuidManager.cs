using System.Collections.Generic;
using COTL_API.Saves;
using System.Linq;
using System;

namespace COTL_API.Guid;

public static class GuidManager
{
    public static string GetFullyQualifiedName(string guid, string value)
    {
        return $"{guid}_{value}";
    }

    private static readonly Dictionary<int, Type> ReverseMapper = new();

    public const int START_INDEX = 5000;

    public const string MAX_DATA = "maximumStoredValueForEnum";

    public static Type GetEnumType(int number)
    {
        ReverseMapper.TryGetValue(number, out var res);
        return res;
    }

    unsafe public static List<T> GetValues<T>() where T : unmanaged, Enum
    {
        List<T> itemList = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        string startKey = typeof(T).Name + "_";
        foreach (var item in APIDataManager.apiData.data)
        {
            if (!item.Key.StartsWith(startKey)) continue;
            
            int enumVal = int.Parse((string)item.Value);
            T convertedEnumVal = *(T*)&enumVal;
            itemList.Add(convertedEnumVal);
        }

        return itemList;
    }

    unsafe public static T GetEnumValue<T>(string guid, string value) where T : unmanaged, Enum
    {
        if (sizeof(T) != sizeof(int))
            throw new NotSupportedException(
                $"Cannot manage values of type {typeof(T).Name} in GuidManager.GetEnumValue");

        string saveKey = $"{typeof(T).Name}_{guid}_{value}";

        int enumValue = APIDataManager.apiData.GetValueAsInt(saveKey);

        if (enumValue == default)
        {
            enumValue = APIDataManager.apiData.GetValueAsInt(MAX_DATA);
            if (enumValue < START_INDEX)
                enumValue = START_INDEX;

            APIDataManager.apiData.SetValue<long>(MAX_DATA, enumValue + 1);
            APIDataManager.apiData.SetValue<long>(saveKey, enumValue);

            APIDataManager.Save();
        }

        ReverseMapper[enumValue] = typeof(T);

        return *(T*)&enumValue;
    }
}