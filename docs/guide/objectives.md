## Creating/Adding Objectives

Creating custom objectives is a little bit different to the other custom implementions. There are no classes to override for one!

Usage Example:

```csharp
using COTL_API.CustomObjectives;
using UnityEngine;

public CustomObjective CustomObjective { get; private set; }
```

```csharp
private void Awake()
{
    CustomObjective = CustomObjectiveManager.CollectItem(Plugin.ExampleItem, 5, false, FollowerLocation.Dungeon1_1, 4800f);
    CustomObjective.InitialQuestText = "This is the quest text.";
}
```

If you need to access the ObjectiveData created by the game, use:

```csharp
CustomObjective.ObjectiveData.<method/field/etc>
```
