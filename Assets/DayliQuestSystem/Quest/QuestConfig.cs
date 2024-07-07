using UnityEngine;

[CreateAssetMenu(fileName = "QuestConfig", menuName = "Quest/QuestConfig")]
public class QuestConfig : ScriptableObject
{
    [field: SerializeField] public QuestType QuestType { get; private set; }
    [field: SerializeField] public int MaxProgress { get; private set; }
    [field: SerializeField, Multiline] public string Description { get; private set; }
    [field: SerializeField, Multiline] public string RewardText { get; private set; }
}
