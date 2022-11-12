using System.Collections.Generic;
using COTL_API.CustomObjectives;

namespace COTL_API.Saves;

public class ApiSlotData
{
    private string[] RandomEasteregg =
        { "Rawr~", "Hello world!", "Another day another unstable API! Hello!", "Proud bug maker!" };

    public string OwO => RandomEasteregg[UnityEngine.Random.Range(0, RandomEasteregg.Length)];

    public Dictionary<int, CustomObjective> QuestData { get; } = new();
}