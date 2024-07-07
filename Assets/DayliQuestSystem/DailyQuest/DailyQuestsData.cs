using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class DailyQuestsData
{
    public List<QuestData> QuestDatas = new List<QuestData>();

    [JsonConstructor]
    public DailyQuestsData(List<QuestData> questData)
    {
        QuestDatas = questData;
    }
}