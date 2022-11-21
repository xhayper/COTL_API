namespace COTL_API.Saves;

internal class ApiData
{
    private readonly string[] _randomEasteregg =
    {
        "Rawr~", "Hewwo, twis pwoject was made bwy a fwwy, and I'm suwe you'ww wuv it!",
        "Another day another unstable API! Hello!", "Proud bug maker!"
    };

    public string OwO => _randomEasteregg[UnityEngine.Random.Range(0, _randomEasteregg.Length)];

    public ObjectDictionary EnumData { get; } = new();
}