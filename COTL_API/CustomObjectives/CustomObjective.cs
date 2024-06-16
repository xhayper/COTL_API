namespace COTL_API.CustomObjectives;

public class CustomObjective(int questID, string initialQuestText, ObjectivesData objectiveData)
{
    public int QuestID { get; internal set; } = questID;
    public string InitialQuestText { get; set; } = initialQuestText;
    public ObjectivesData ObjectiveData { get; internal set; } = objectiveData;
}