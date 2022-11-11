using System.Linq;

namespace COTL_API.Helpers;

public static class HashCode
{
    public static int GetNameHashCode(string str)
    {
        return str.Aggregate(0, (current, c) => (current << 3) - current + c);
    }

    public static int GetValueHashCode(string str)
    {
        return str.Aggregate(0, (current, c) => ((current << 5) + current) ^ c);
    }
}