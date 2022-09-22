namespace COTL_API.CustomObjectives;

/// <summary>
/// The custom objective class.
/// </summary>
public abstract class CustomObjective
{
    /// <summary>
    /// Internal name of the quest.
    /// </summary>
    /// <returns>The string</returns>
    public abstract string InternalName { get; }
    internal string ModPrefix;

    /// <summary>
    /// This is the text that appears in the dialog when the follower is proposing the quest to the player.
    /// </summary>
    /// <returns>string</returns>
    public abstract string InitialQuestText { get; }

    /// <summary>
    /// This is where the custom objective is created and stored against the plugins ObjectiveKey in CustomObjectivesManager.
    /// </summary>
    /// <returns>ObjectivesData</returns>
    public abstract ObjectivesData ObjectiveData { get; }

    /// <summary>
    /// This is the objectives unique key. It is used to store the objective in CustomObjectivesManager.
    /// </summary>
    /// <returns>Objectives.CustomQuestTypes</returns>
    public Objectives.CustomQuestTypes ObjectiveKey { get; internal set; }
}