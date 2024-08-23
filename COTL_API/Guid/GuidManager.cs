namespace COTL_API.Guid;

public static class GuidManager
{
    private const int StartIndex = 5000;

    private const string MaxDataKey = "maximumStoredValueForEnum";

    private static readonly Dictionary<int, Type> ReverseMapper = [];

    public static string GetFullyQualifiedName(string guid, string value)
    {
        return $"{guid}_{value}";
    }

    public static Type? GetEnumType(int number)
    {
        ReverseMapper.TryGetValue(number, out var res);
        return res;
    }

    public static unsafe List<T> GetValues<T>() where T : unmanaged, Enum
    {
        var itemList = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        var startKey = typeof(T).Name + "_";
        // It cannot do pointer-hack stuff when using query
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var item in Plugin.EnumData!)
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
        var enumValue = Plugin.EnumData!.GetValueAsInt(saveKey);

        if (enumValue == default)
        {
            enumValue = Plugin.EnumData.GetValueAsInt(MaxDataKey);
            if (enumValue < StartIndex)
                enumValue = StartIndex;

            Plugin.EnumData.SetValue<long>(MaxDataKey, enumValue + 1);
            Plugin.EnumData.SetValue<long>(saveKey, enumValue);
            Plugin.Instance!.APIData.Save();
        }

        ReverseMapper[enumValue] = typeof(T);
        return *(T*)&enumValue;
    }
}