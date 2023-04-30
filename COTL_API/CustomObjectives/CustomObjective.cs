namespace COTL_API.CustomObjectives;

public class CustomObjective
{
    public CustomObjective(int questID, string initialQuestText, ObjectivesData objectiveData)
    {
        QuestID = questID;
        InitialQuestText = initialQuestText;
        ObjectiveData = objectiveData;
    }

    public int QuestID { get; internal set; }
    public string InitialQuestText { get; set; }
    public ObjectivesData ObjectiveData { get; internal set; }
}