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

    private const int StartIndex = 5000;

    private const string MaxDataKey = "maximumStoredValueForEnum";

    public static Type GetEnumType(int number)
    {
        ReverseMapper.TryGetValue(number, out Type res);
        return res;
    }

    public static unsafe List<T> GetValues<T>() where T : unmanaged, Enum
    {
        var itemList = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        var startKey = typeof(T).Name + "_";
        // It cannot do pointer-hack stuff when using query
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var item in APIDataManager.APIData.Data)
        {
            if (!item.Key.StartsWith(startKey)) continue;

            var enumVal = int.Parse((string)item.Value);
            var convertedEnumVal = *(T*)&enumVal;
            itemList.Add(convertedEnumVal);
        }

        return itemList;
    }

    public static unsafe T GetEnumValue<T>(string guid, string value) where T : unmanaged, Enum
    {
        if (sizeof(T) != sizeof(int))
            throw new NotSupportedException(
                $"Cannot manage values of type {typeof(T).Name} in GuidManager.GetEnumValue");

        var saveKey = $"{typeof(T).Name}_{guid}_{value}";
        var enumValue = APIDataManager.APIData.GetValueAsInt(saveKey);

        if (enumValue == default)
        {
            enumValue = APIDataManager.APIData.GetValueAsInt(MaxDataKey);
            if (enumValue < StartIndex)
                enumValue = StartIndex;

            APIDataManager.APIData.SetValue<long>(MaxDataKey, enumValue + 1);
            APIDataManager.APIData.SetValue<long>(saveKey, enumValue);
            APIDataManager.Save();
        }

        ReverseMapper[enumValue] = typeof(T);

        return *(T*)&enumValue;
    }
}