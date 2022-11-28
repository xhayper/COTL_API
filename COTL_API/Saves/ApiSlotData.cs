using COTL_API.CustomObjectives;

namespace COTL_API.Saves;

public class ApiSlotData
{
    private readonly string[] _randomEasterEgg =
    {
        "Rawr~", "Hewwo, twis pwoject was made bwy a fwwy, and I'm suwe you'ww wuv it!",
        "Another day another unstable API! Hello!", "Proud bug maker!"
    };

    public string OwO => _randomEasterEgg[UnityEngine.Random.Range(0, _randomEasterEgg.Length)];

    public Dictionary<int, CustomObjective> QuestData { get; set; } = new();
}