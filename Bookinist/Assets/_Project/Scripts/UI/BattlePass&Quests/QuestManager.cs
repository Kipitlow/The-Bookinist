using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _notificationIcon;

    private HashSet<Button> _readyToClaimQuests = new HashSet<Button>();

    public void SuccessQuest(Button quest)
    {
        quest.interactable = true;

        if (!_readyToClaimQuests.Contains(quest))
        {
            _readyToClaimQuests.Add(quest);
        }

        RefreshNotification();
    }

    public void ClaimQuest(Button quest)
    {
        _readyToClaimQuests.Remove(quest);

        RefreshNotification();
    }

    private void RefreshNotification()
    {
        if (_notificationIcon != null)
        {
            // Active l'icône si le compte est supérieur à 0
            _notificationIcon.SetActive(_readyToClaimQuests.Count > 0);
        }
    }

    public void ChangeColor(Image image)
    {
        image.color = new Color32(231, 154, 62, 255);
    }
}