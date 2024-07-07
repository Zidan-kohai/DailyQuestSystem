using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [field: SerializeField] public List<DailyQuestsConfig> dailyQuestConfigs { get; private set; } = new();
    [SerializeField] private bool resetable;
    [SerializeField] private bool loop;

    private List<DailyQuests> dailyQuests = new();
    private TimeTracker timeTracker;
    
    #region SaveData
    private const string SaveDataKey = "DailyQuestSaveDataKey";
    public List<DailyQuestsData> dailyQuestData { get; private set; } = new();
    #endregion

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;    
        DontDestroyOnLoad(gameObject);

        timeTracker = new TimeTracker(this, resetable, dailyQuestConfigs.Count, loop);

        Load();

        InitDailyQuests();
    }

    private void InitDailyQuests()
    {
        for (int i = 0; i < dailyQuestConfigs.Count; i++)
        {
            DailyQuests dayliQuest = new DailyQuests(this, dailyQuestConfigs[i].questConfigAndEventProvider, dailyQuestData[i]);

            this.dailyQuests.Add(dayliQuest);
        }
    }

    public bool TryGetCurrentDailyQuests(out DailyQuests dailyQuest)
    {
        dailyQuest = null; 

        if (dailyQuests.Count < timeTracker.GetDayCount)
        {
            Debug.Log($"Have`t Quest for {timeTracker.GetDayCount} day");
            return false;
        }

        dailyQuest = dailyQuests[timeTracker.GetDayCount - 1];
        return true;
    }

    private void Load()
    {
        string json;

        if (PlayerPrefs.HasKey(SaveDataKey))
        {
            json = PlayerPrefs.GetString(SaveDataKey);
        }
        else
        {
            List<DailyQuestsData> emptyList = new List<DailyQuestsData>();
            for (int i = 0; i < dailyQuestConfigs.Count; i++)
            {
                List<QuestData> questDatas = new List<QuestData>();

                for (int j = 0; j < 10; j++)
                {
                    QuestData data = new QuestData();
                    questDatas.Add(data);
                }

                emptyList.Add(new DailyQuestsData(questDatas));
            }

            json = JsonConvert.SerializeObject(emptyList);
        }

        dailyQuestData = JsonConvert.DeserializeObject<List<DailyQuestsData>>(json);

        if(dailyQuestConfigs.Count > dailyQuestData.Count)
        {
            for(int i = dailyQuestData.Count; i < dailyQuestConfigs.Count; i++)
            {
                List<QuestData> questDatas = new List<QuestData>();

                for (int j = 0; j < 10; j++)
                {
                    QuestData data = new QuestData();
                    questDatas.Add(data);
                }

                dailyQuestData.Add(new DailyQuestsData(questDatas));
            }
        }
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(dailyQuestData);

        PlayerPrefs.SetString(SaveDataKey,json);

        timeTracker.Save();
    }

    private void OnApplicationFocus()
    {
        timeTracker.Save();
    }

    private void OnApplicationQuit()
    {
        timeTracker.Save();
    }
}