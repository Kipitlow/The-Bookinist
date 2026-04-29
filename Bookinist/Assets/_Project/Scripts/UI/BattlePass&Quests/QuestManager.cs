using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestData
{
    public string questID;
    public Button questButton;
    public Image questImage;
    [HideInInspector] public bool isClaimed = false;
}

public class QuestManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _notificationIcon;
    [SerializeField] private Color32 _successColor = new Color32(231, 154, 62, 255);

    [Header("Quests Lists")]
    public List<QuestData> dailyQuests = new List<QuestData>();
    public List<QuestData> hebdoQuests = new List<QuestData>();
    public List<QuestData> saisonQuests = new List<QuestData>();

    private HashSet<Button> _readyToClaimQuests = new HashSet<Button>();

    private void Start()
    {
        InitList(dailyQuests);
        InitList(hebdoQuests);
        InitList(saisonQuests);

        // La première quête Daily est réussie au départ si jamais faite
        if (dailyQuests.Count > 0 && !dailyQuests[0].isClaimed)
        {
            SetQuestSuccess(dailyQuests[0]);
        }

        RefreshNotification();
    }

    private void InitList(List<QuestData> list)
    {
        foreach (var quest in list)
        {
            // Sécurité : on nettoie et on lie le bouton au code
            quest.questButton.onClick.RemoveAllListeners();
            QuestData capturedQuest = quest;
            quest.questButton.onClick.AddListener(() => ClaimQuest(capturedQuest));

            // Chargement
            quest.isClaimed = PlayerPrefs.GetInt("Quest_" + quest.questID, 0) == 1;

            if (quest.isClaimed)
            {
                quest.questButton.interactable = false;
                // On peut laisser la couleur de succès ou mettre une couleur "Validée"
                ChangeColor(quest.questImage);
            }
            else
            {
                quest.questButton.interactable = false;
                quest.questImage.color = Color.white; // Couleur par défaut
            }
        }
    }

    public void SetQuestSuccess(QuestData quest)
    {
        if (quest == null || quest.isClaimed) return;

        quest.questButton.interactable = true;
        ChangeColor(quest.questImage);

        if (!_readyToClaimQuests.Contains(quest.questButton))
        {
            _readyToClaimQuests.Add(quest.questButton);
        }
        RefreshNotification();
    }

    public void ClaimQuest(QuestData quest)
    {
        if (quest.isClaimed) return;

        quest.isClaimed = true;
        _readyToClaimQuests.Remove(quest.questButton);
        quest.questButton.interactable = false;

        PlayerPrefs.SetInt("Quest_" + quest.questID, 1);
        PlayerPrefs.Save();

        RefreshNotification();
    }

    // --- RESETS ---
    public void ResetDaily() => ResetList(dailyQuests);
    public void ResetHebdo() => ResetList(hebdoQuests);
    public void ResetSaison() => ResetList(saisonQuests);

    private void ResetList(List<QuestData> list)
    {
        foreach (var quest in list)
        {
            quest.isClaimed = false;
            PlayerPrefs.DeleteKey("Quest_" + quest.questID);
            quest.questButton.interactable = false;
            quest.questImage.color = Color.white;
        }
        RefreshNotification();
    }

    private void ChangeColor(Image image)
    {
        if (image != null) image.color = _successColor;
    }

    private void RefreshNotification()
    {
        if (_notificationIcon != null)
        {
            _notificationIcon.SetActive(_readyToClaimQuests.Count > 0);
        }
    }
}