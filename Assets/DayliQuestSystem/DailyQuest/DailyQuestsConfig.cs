using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyQuestsConfig
{
    [field: SerializeField] public List<QuestConfigAndEventProvider> questConfigAndEventProvider { get; private set; }
}