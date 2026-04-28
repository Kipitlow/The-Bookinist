using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private QuestManager _manager;

    public QuestData data;

    public void OnClaimClicked()
    {
        if (_manager != null && data != null)
        {
            _manager.ClaimQuest(data);
        }
    }
}