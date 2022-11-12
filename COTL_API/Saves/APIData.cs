using System.Collections.Generic;
using COTL_API.CustomObjectives;

namespace COTL_API.Saves;

internal class APIData
{
    public Dictionary<int, CustomObjective> QuestData = new();

    public readonly ObjectDictionary Enum = new();
}