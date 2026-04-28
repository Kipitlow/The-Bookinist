using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Cette classe permet de définir ce qu'est une quête dans l'inspecteur
[System.Serializable]
public class QuestData
{
    public string questID; // ID unique pour la sauvegarde (ex: "daily_1")
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
        LoadAllQuests();

        // Condition spécifique : La première quête Daily est réussie au départ
        if (dailyQuests.Count > 0 && !dailyQuests[0].isClaimed)
        {
            SetQuestSuccess(dailyQuests[0]);
        }

        RefreshNotification();
    }

    // --- LOGIQUE CORE ---

    // Rend une quête réussie (prête à être cliquée)
    public void SetQuestSuccess(QuestData quest)
    {
        if (quest.isClaimed) return;

        quest.questButton.interactable = true;
        ChangeColor(quest.questImage);

        if (!_readyToClaimQuests.Contains(quest.questButton))
        {
            _readyToClaimQuests.Add(quest.questButton);
        }

        RefreshNotification();
    }

    // Appelé quand on clique sur le bouton de la quête
    public void ClaimQuest(QuestData quest)
    {
        quest.isClaimed = true;
        _readyToClaimQuests.Remove(quest.questButton);

        // On désactive le bouton car la récompense est prise
        quest.questButton.interactable = false;

        // Sauvegarde de l'état
        PlayerPrefs.SetInt("Quest_" + quest.questID, 1);
        PlayerPrefs.Save();

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

    // --- SAUVEGARDE ET CHARGEMENT ---

    private void LoadAllQuests()
    {
        CheckAndLoadList(dailyQuests);
        CheckAndLoadList(hebdoQuests);
        CheckAndLoadList(saisonQuests);
    }

    private void CheckAndLoadList(List<QuestData> list)
    {
        foreach (var quest in list)
        {
            // On récupère 1 si complété, 0 sinon
            bool savedState = PlayerPrefs.GetInt("Quest_" + quest.questID, 0) == 1;
            quest.isClaimed = savedState;

            if (quest.isClaimed)
            {
                quest.questButton.interactable = false;
                // Optionnel : mettre une couleur "grisée" ici
            }
            else
            {
                quest.questButton.interactable = false; // Par défaut non cliquable tant que pas success
            }
        }
    }

    // --- FONCTIONS DE RESET ---

    public void ResetDailyQuests() => ResetList(dailyQuests);
    public void ResetHebdoQuests() => ResetList(hebdoQuests);
    public void ResetSaisonQuests() => ResetList(saisonQuests);

    private void ResetList(List<QuestData> list)
    {
        foreach (var quest in list)
        {
            quest.isClaimed = false;
            PlayerPrefs.DeleteKey("Quest_" + quest.questID);
            quest.questButton.interactable = false;
            quest.questImage.color = Color.white; // Retour couleur d'origine
        }
        RefreshNotification();
    }
}