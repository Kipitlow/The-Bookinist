using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject notificationIcon; 

    private HashSet<Button> readyToClaimQuests = new HashSet<Button>();

    public void SuccessQuest(Button quest)
    {
        quest.interactable = true;

        if (!readyToClaimQuests.Contains(quest))
        {
            readyToClaimQuests.Add(quest);
        }

        RefreshNotification();
    }


    public void ClaimQuest(Button quest)
    {
        readyToClaimQuests.Remove(quest);

        RefreshNotification();
    }


    private void RefreshNotification()
    {
        if (notificationIcon != null)
        {
            // Active l'icône si le compte est supérieur à 0
            notificationIcon.SetActive(readyToClaimQuests.Count > 0);
        }
    }

    public void ChangeColor(Image image)
    {
        image.color = new Color32(231, 154, 62, 255);
    }
}