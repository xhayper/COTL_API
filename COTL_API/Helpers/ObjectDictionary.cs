using System.Runtime.Serialization;

namespace COTL_API.Saves;

[Serializable]
public class ObjectDictionary : Dictionary<string, object>
{
    public ObjectDictionary()
    {
    }

    protected ObjectDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public T? GetValue<T>(string key)
    {
        if (ContainsKey(key))
            return (T)this[key];

        return default;
    }

    public string? GetValueAsString(string key)
    {
        return GetValue<string>(key);
    }

    public int GetValueAsInt(string key)
    {
        var value = GetValueAsLong(key);

        return value switch
        {
            > int.MaxValue => int.MaxValue,
            < int.MinValue => int.MinValue,
            _ => (int)value
        };
    }

    public long GetValueAsLong(string key)
    {
        return GetValue<long>(key);
    }

    public float GetValueAsFloat(string key)
    {
        return Convert.ToSingle(GetValue<object>(key));
    }

    public bool GetValueAsBoolean(string key)
    {
        return GetValue<bool>(key);
    }

    public void SetValue<T>(string key, T? value)
    {
        if (value == null)
        {
            Remove(key);
            return;
        }

        if (ContainsKey(key))
            this[key] = value;
        else
            Add(key, value);
    }
}
