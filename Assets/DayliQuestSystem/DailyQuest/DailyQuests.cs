using System;
using System.Collections.Generic;

public class DailyQuests
{
    private readonly QuestManager questManager;
    private DailyQuestsData data;
    public List<QuestConfigAndEventProvider> QuestConfigAndEventProvider { get; private set; } = new();

    public List<Quest> Quests { get; private set; }

    public DailyQuests(QuestManager questManager, List<QuestConfigAndEventProvider> questConfigs, DailyQuestsData data)
    {
        this.questManager = questManager;
        this.QuestConfigAndEventProvider = questConfigs;
        this.data = data;

        InitQuest();
    }

    private void InitQuest()
    { 
        Quests = new();

        for (int i = 0; i < QuestConfigAndEventProvider.Count; i++)
        {
            Quest quest = new Quest(questManager, QuestConfigAndEventProvider[i].QuestConfig, QuestConfigAndEventProvider[i].OnClaim, data.QuestDatas[i]);
            Quests.Add(quest);
        }
    }

    public bool TryGetQuestWithType(QuestType questType, out Quest quest)
    {
        quest = null;

        if(!Quests.Exists(a => a.GetQuestType == questType))
        {
            return false;
        }

        quest = Quests.Find(a => a.GetQuestType == questType);
        return true;
    }
}