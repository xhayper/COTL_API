---
title: Objectives
description: Documentation on how to add a custom objective using Cult of the Lamb API
layout: ../layouts/MainLayout.astro
---

## Creating Objectives

To create a custom objective, you first need to make a class overriding `CustomObjective`. The example below creates a collect item quest using a custom item with a random number of items to collect.  
Example:

```csharp
using COTL_API.CustomObjectives;
using UnityEngine;
```

```csharp
public class ExampleObjective : CustomObjective
{
    public override string InternalName => "EXAMPLE_OBJECTIVE";

    public override string InitialQuestText => "EXAMPLE OBJECTIVE TEXT";

    public override ObjectivesData ObjectiveData => CustomObjectiveManager.Objective.CollectItem(ObjectiveKey, Plugin.ExampleItem, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
}
```

`CustomObjective` supports the following overrides:
| Type | Name | Default | Purpose |
|-|-|-|-|
| string | InternalName | \[REQUIRED\] |-|
| string | InitialQuestText | \[REQUIRED\] | This is the text the follower says when asking you to go on a quest. |
| ObjectivesData | ObjectiveData | \[REQUIRED\] | This is the actual quest data. |

## Adding Objectives

To add an objective to the game, simply use `CustomObjectiveManager.Add()`.  
Example:

```csharp
using COTL_API.CustomObjectives;
using UnityEngine;

public static (Objectives.CustomQuestTypes ObjectiveKey, ObjectivesData ObjectiveData) ExampleObjective { get; private set; }
```

```csharp
private void Awake()
{
    ExampleObjective = CustomObjectiveManager.Add(new ExampleObjective());

    //if you need to change something post adding, use:
    ExampleObjective.ObjectiveData.<method/field>
}
```

`CustomObjectiveManager.Add()` returns a tuple containing the assigned `ObjectiveKey` and `ObjectiveData` allowing easy access to the information elsewhere in the plugin.

Example:

```csharp
private void SomeMethod()
{
    if(Plugin.ExampleObjective.ObjectiveData.isComplete)
    {
        //do something
    }
}
```
