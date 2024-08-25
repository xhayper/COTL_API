using Random = UnityEngine.Random;

namespace COTL_API.Saves;

internal class ApiData
{
    private readonly string[] _randomEasterEgg =
    [
        "Rawr~", "Hewwo, twis pwoject was made bwy a fwwy, and I'm suwe you'ww wuv it!",
        "Another day another unstable API! Hello!", "Proud bug maker!"
    ];

    public string OwO => _randomEasterEgg[Random.Range(0, _randomEasterEgg.Length)];

    public ObjectDictionary EnumData { get; set; } = [];
}