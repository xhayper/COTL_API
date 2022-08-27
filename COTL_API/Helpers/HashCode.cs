namespace COTL_API.Helpers;

public class HashCode
{
    public static int GetNameHashCode(string str)
    {
        int hash = 0;
        foreach (char c in str) hash = ((hash << 3) - hash) + c;
        return hash;
    }

    public static int GetValueHashCode(string str)
    {
        int hash = 0;
        foreach (char c in str) hash = ((hash << 5) + hash) ^ c;
        return hash;
    }
}